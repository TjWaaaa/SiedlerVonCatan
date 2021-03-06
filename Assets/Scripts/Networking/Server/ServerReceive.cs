using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Enums;
using Networking.Communication;
using Networking.Interfaces;
using Networking.Package;
using Player;
using UnityEngine;

namespace Networking.ServerSide
{
    public class ServerReceive : INetworkableServer
    {
        private readonly ServerToClientCommunication serverRequest;

        private int mandatoryNodeID;

        // Player
        private Dictionary<int, ServerPlayer> allPlayer = new Dictionary<int, ServerPlayer>();
        private int playerAmount;
        private int currentPlayer;
        private readonly Stack<PLAYERCOLOR> possibleColors = new Stack<PLAYERCOLOR>();

        // Gamestage params 
        private bool firstRound = true;
        private bool inGameStartupPhase = true;
        private bool villageBuilt;

        // Board
        private Board gameBoard = new Board();

        // Development Cards
        private Stack<DEVELOPMENT_TYPE> shuffledDevCardStack = new Stack<DEVELOPMENT_TYPE>();
        private DEVELOPMENT_TYPE[] unshuffledDevCardArray = { DEVELOPMENT_TYPE.VICTORY_POINT,
            DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT,
            DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT };

        // Test
        private bool isTestRunning;

        public ServerReceive(ServerToClientCommunication serverRequest)
        {
            this.serverRequest = serverRequest;
            possibleColors.Push(PLAYERCOLOR.YELLOW);
            possibleColors.Push(PLAYERCOLOR.WHITE);
            possibleColors.Push(PLAYERCOLOR.BLUE);
            possibleColors.Push(PLAYERCOLOR.RED);
        }

        public ServerReceive(ServerToClientCommunication serverRequest, bool isTestRunning)
        {
            this.isTestRunning = isTestRunning;
            this.serverRequest = serverRequest;
            possibleColors.Push(PLAYERCOLOR.WHITE);
            possibleColors.Push(PLAYERCOLOR.YELLOW);
            possibleColors.Push(PLAYERCOLOR.BLUE);
            possibleColors.Push(PLAYERCOLOR.RED);
        }


        //---------------------------------------------- Interface INetworkableServer implementation ----------------------------------------------

        public void handleRequestJoinLobby(Packet clientPacket, int currentClientID)
        {
            // ankommender spieler: name setzen + farbe zuweisen
            // alle lobby daten zurücksenden

            ArrayList allPlayerInformation = new ArrayList();
            foreach (var player in allPlayer.Values)
            {
                if (player.getPlayerID() == currentClientID)
                {
                    player.setPlayerName(clientPacket.playerName);
                    player.setPlayerColor(possibleColors.Pop());
                }

                // look for all Players that are fully initialized and add it to ArrayList that updates client lobbies. 
                if (player.getPlayerName() != null)
                {
                    allPlayerInformation.Add(new object[] { player.getPlayerID(), player.getPlayerName(), player.getPlayerColor() });
                }
            }

            serverRequest.notifyClientJoined(allPlayerInformation, Server.serverIP.ToString());
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="clientPacket"></param>
        /// <param name="currentClientID"></param>
        public void handleRequestPlayerReady(Packet clientPacket, int currentClientID)
        {
            bool runGame = true;

            foreach (ServerPlayer player in allPlayer.Values)
            {
                if (player.getPlayerID() == currentClientID)
                {
                    player.setIsReady(clientPacket.isReady);
                    serverRequest.notifyPlayerReady(currentClientID, player.getPlayerName(), clientPacket.isReady);
                }

                // check if all players are ready
                if (!player.getIsReady())
                {
                    runGame = false;
                }
            }

            // Start game if all player are ready
            // Todo: Boardgenerator!
            if (runGame)
            {
                currentPlayer = playerAmount - 1;
                serverRequest.gamestartInitialize(gameBoard.getHexagonsArray());
                shuffledDevCardStack = generateRandomDevCardStack(unshuffledDevCardArray);
                serverRequest.notifyNextPlayer(currentPlayer, 0);
                Debug.Log($"SERVER: Starting the game with player {currentPlayer}");
            }

            // Send error if no player was found
            // Todo: send error to all?
            // serverRequest.notifyRejection(currentClientID, "You seem to be not existing...");
        }

        public void handleBeginRound(Packet clientPacket)
        {
            // Roll dices
            int[] diceNumbers = rollDices();
            serverRequest.notifyRollDice(diceNumbers);

            // Distribute resources
            for (int playerIndex = 0; playerIndex < allPlayer.Count; playerIndex++)
            {
                ServerPlayer player = allPlayer.ElementAt(playerIndex).Value;
                int[] distributedResources = gameBoard.distributeResources(diceNumbers[0] + diceNumbers[1], player.getPlayerColor());
                Debug.Log("Player " + playerIndex + " gets: " + distributedResources[0] + distributedResources[1] + distributedResources[2] + distributedResources[3] + distributedResources[4]);

                for (int i = 0; i < distributedResources.Length; i++)
                {
                    player.setResourceAmount((RESOURCETYPE)i, distributedResources[i]);
                }
                updateOwnPlayer(playerIndex);
            }
            updateRepPlayers();
        }

        public void handleTradeBank(Packet clientPacket)
        {
            if (isNotCurrentPlayer(clientPacket.myPlayerID))
            {
                Debug.LogWarning($"SERVER: Client request rejected from client {clientPacket.myPlayerID}");
                serverRequest.notifyRejection(clientPacket.myPlayerID, "You are not allowed to trade with the bank!");
                return;
            }

            Debug.Log("offer: " + clientPacket.tradeResourcesOffer + "; expect: " + clientPacket.tradeResourcesExpect);
            allPlayer.ElementAt(currentPlayer).Value.trade(clientPacket.tradeResourcesOffer, clientPacket.tradeResourcesExpect);

            updateRepPlayers();
            updateOwnPlayer(currentPlayer);
        }

        public void handleTradeOffer(Packet clientPacket)
        {
            if (!inGameStartupPhase)
            {
                if (isNotCurrentPlayer(clientPacket.myPlayerID))
                {
                    Debug.LogWarning($"SERVER: Client request rejected from client {clientPacket.myPlayerID}");
                    serverRequest.notifyRejection(clientPacket.myPlayerID, "You are not allowed to offer a trade!");
                    return;
                }

                ServerPlayer currentServerPlayer = allPlayer.ElementAt(currentPlayer).Value;
                RESOURCETYPE resourcetype = (RESOURCETYPE)clientPacket.resourceType;
                int buttonNumber = clientPacket.buttonNumber.GetValueOrDefault();
                if (currentServerPlayer.canTrade(resourcetype))
                {
                    serverRequest.notifyAcceptTradeOffer(currentServerPlayer.getPlayerID(), buttonNumber);
                }
                else
                {
                    serverRequest.notifyRejection(allPlayer.ElementAt(currentPlayer).Value.getPlayerID(), "Not enough resources to offer");

                }
            }
            else
            {
                serverRequest.notifyRejection(clientPacket.myPlayerID, "Method HANDLE_TRADE_OFFER during game startphase prohibited");
            }
        }

        public void handleBuild(Packet clientPacket)
        {
            if (isNotCurrentPlayer(clientPacket.myPlayerID))
            {
                Debug.LogWarning($"SERVER: Client request rejected from client {clientPacket.myPlayerID}");
                serverRequest.notifyRejection(clientPacket.myPlayerID, "You are not allowed to build!");
                return;
            }

            ServerPlayer currentServerPlayer = allPlayer.ElementAt(currentPlayer).Value;
            BUYABLES buildingType = (BUYABLES)clientPacket.buildType;
            int posInArray = clientPacket.buildID.GetValueOrDefault();

            PLAYERCOLOR playerColor = allPlayer.ElementAt(currentPlayer).Value.getPlayerColor();

            buildStructure(currentServerPlayer, buildingType, posInArray, playerColor, clientPacket);
        }

        public void handleBuyDevelopement(Packet clientPacket)
        {
            if (!inGameStartupPhase)
            {
                if (isNotCurrentPlayer(clientPacket.myPlayerID))
                {
                    Debug.LogWarning($"SERVER: Client request rejected from client {clientPacket.myPlayerID}");
                    serverRequest.notifyRejection(clientPacket.myPlayerID, "You are not allowed to buy a development card!");
                    return;
                }

                if (allPlayer.ElementAt(currentPlayer).Value.canBuyBuyable(BUYABLES.DEVELOPMENT_CARDS) && shuffledDevCardStack.Count != 0)
                {
                    Debug.Log("SERVER: there are so many devCards left:" + shuffledDevCardStack.Count);
                    allPlayer.ElementAt(currentPlayer).Value.buyBuyable(BUYABLES.DEVELOPMENT_CARDS);
                    //DEVELOPMENT_TYPE temp = shuffledDevCardStack.Pop();
                    Debug.Log("Server: A devCard has been popped out of the stack, there are only so much more: " + shuffledDevCardStack.Count);
                    allPlayer.ElementAt(currentPlayer).Value.setNewDevCard(shuffledDevCardStack.Pop());

                    // Sending Packages
                    updateRepPlayers();
                    updateOwnPlayer(currentPlayer);
                    serverRequest.acceptBuyDevelopement(shuffledDevCardStack.Count);
                }
                else
                {
                    serverRequest.notifyRejection(currentPlayer, "There are no development cards left to buy");
                }
            }
            else
            {
                serverRequest.notifyRejection(clientPacket.myPlayerID, "Method HANDLE_BUY_DEVELOPMENT during game start phase prohibited");
            }
        }

        public void handlePlayDevelopement(Packet clientPacket)
        {
            if (!inGameStartupPhase)
            {
                if (isNotCurrentPlayer(clientPacket.myPlayerID))
                {
                    Debug.LogWarning($"SERVER: Client request rejected from client {clientPacket.myPlayerID}");
                    serverRequest.notifyRejection(clientPacket.myPlayerID, "You are not allowed to play a development card!");
                    return;
                }
                Debug.Log(clientPacket.developmentCard);
                Debug.Log("SERVER: CurrentPlayer has enough cards: " + allPlayer.ElementAt(currentPlayer).Value.getDevCardAmount(clientPacket.developmentCard));
                if (allPlayer.ElementAt(currentPlayer).Value.getDevCardAmount(clientPacket.developmentCard) > 0)
                {
                    allPlayer.ElementAt(currentPlayer).Value.playDevCard(clientPacket.developmentCard);

                    // Sending Packages
                    updateRepPlayers();
                    updateOwnPlayer(currentPlayer);
                    serverRequest.notifyAcceptPlayDevelopement(allPlayer.ElementAt(currentPlayer).Key, clientPacket.developmentCard, allPlayer.ElementAt(currentPlayer).Value.getPlayerName());
                }
                else
                {
                    serverRequest.notifyRejection(allPlayer.ElementAt(currentPlayer).Key, "You can't play this developement card");
                }
            }
            else
            {
                serverRequest.notifyRejection(clientPacket.myPlayerID, "Method HANDLE_PLAY_DEVELOPMENT during game start phase prohibited");
            }
        }

        public void handleEndTurn(Packet clientPacket)
        {
            Debug.Log("SERVER: handleEndTurn has been called");

            if (isNotCurrentPlayer(clientPacket.myPlayerID))
            {
                Debug.LogWarning($"SERVER: Client request rejected from client {clientPacket.myPlayerID}");
                serverRequest.notifyRejection(clientPacket.myPlayerID, "You are not allowed to end someone elses turn");
                return;
            }
            if (didThisPlayerWin(currentPlayer))
            {
                Debug.LogWarning("SERVER: Game is over");
                serverRequest.notifyVictory(allPlayer.ElementAt(currentPlayer).Value.getPlayerName(), allPlayer.ElementAt(currentPlayer).Value.getPlayerColor());
                return;
            }

            // Begin next round
            if (!inGameStartupPhase)
            {
                changeCurrentPlayer(clientPacket, currentPlayer);
                Debug.Log("SERVER: Current Player index: " + currentPlayer);
                updateRepPlayers();
                updateOwnPlayer(currentPlayer);
                handleBeginRound(clientPacket);
            }
        }

        public void handleClientDisconnectServerCall(int disconnectedClientID)
        {
            var player = allPlayer[disconnectedClientID];
            serverRequest.notifyClientDisconnect(player.getPlayerName(), player.getPlayerColor());
        }

        //---------------------------------------------- All logical methods ----------------------------------------------

        /// <summary>
        /// Only the actions of the currentPlayer are allowed to be handled.
        /// Returns false if the server can continue.
        /// </summary>
        private bool isNotCurrentPlayer(int clientID)
        {
            var currentPlayerObject = allPlayer.ElementAt(currentPlayer).Value;
            Debug.Log("Comparing currentPlayer ID: " + currentPlayerObject.getPlayerID() + " with clientID: " + clientID);
            if (currentPlayerObject.getPlayerID() == clientID)
            {
                return false;
            }
            Debug.Log("SERVER: Current Player is: " + currentPlayer + ", instead of: " + clientID);
            return true;
        }

        /// <summary>
        /// Returns an array of random numbers in range 1-6(included) as a rolled dice simulation.
        /// </summary>
        public int[] rollDices()
        {
            Debug.Log("SERVER: Dices are being rolled");
            System.Random r = new System.Random();
            int[] diceNumbers = new int[2];
            diceNumbers[0] = r.Next(1, 7);
            diceNumbers[1] = r.Next(1, 7);
            Debug.Log($"SERVER: Dice value 1: {diceNumbers[0]} // Dice value 2: {diceNumbers[1]}");
            return diceNumbers;
        }

        /// <summary>
        /// Calls the convertFromSPToRP function for each player to convert the server player in representative player.
        /// </summary>
        private int[][] convertSPAToRPA() // ServerPlayerArray / RepPlayerArray
        {
            int i = 0;
            int[][] cache = new int[playerAmount][];
            foreach (ServerPlayer player in allPlayer.Values) // Goes though 0 1 2 3 in allPlayer
            {
                cache[i] = player.convertFromSPToRP();
                i++;
            }
            return cache;
        }

        /// <summary>
        /// Generates a player. Adds resources if the test is running.
        /// </summary>
        public void generatePlayer(int playerId)
        {
            ServerPlayer newPlayer = new ServerPlayer(playerId);
            allPlayer.Add(playerId, newPlayer);
            playerAmount++;

            if (isTestRunning)
            {
                newPlayer.setResourceAmount(RESOURCETYPE.SHEEP, 10);
                newPlayer.setResourceAmount(RESOURCETYPE.ORE, 10);
                newPlayer.setResourceAmount(RESOURCETYPE.WHEAT, 10);
                newPlayer.setResourceAmount(RESOURCETYPE.WOOD, 10);
                newPlayer.setResourceAmount(RESOURCETYPE.BRICK, 10);
            }
        }

        /// <summary>
        /// Shuffles the start card deck into a random deck of cards.
        /// </summary>
        private Stack<DEVELOPMENT_TYPE> generateRandomDevCardStack(DEVELOPMENT_TYPE[] array)
        {
            return new Stack<DEVELOPMENT_TYPE>(array.OrderBy(n => Guid.NewGuid()).ToArray());
        }

        /// <summary>
        /// Converts the Serverplayer of the currentplayer into an own player to send an updateOwnPlayer().
        /// </summary>
        public void updateOwnPlayer(int playerIndex)
        {
            serverRequest.updateOwnPlayer(
                allPlayer.ElementAt(playerIndex).Value.convertFromSPToOP(), // int[] with left buildings
                allPlayer.ElementAt(playerIndex).Value.convertSPToOPResources(), // Resource Dictionary
                allPlayer.ElementAt(playerIndex).Value.convertSPToOPDevCards(), // DevCard Dictionary
                allPlayer.ElementAt(playerIndex).Key);
        }

        /// <summary>
        /// Just makes the code look cleaner inside the previous methods when updateRepPlayers is called.
        /// </summary>
        private void updateRepPlayers()
        {
            serverRequest.updateRepPlayers(convertSPAToRPA());
        }

        /// <summary>
        /// Checks if a player has 10 or more victory points.
        /// </summary>
        private bool didThisPlayerWin(int playerIndex)
        {
            if (playerIndex > playerAmount || playerIndex < 0)
            {
                serverRequest.notifyRejection(currentPlayer, "This player cannot exist");
                return false;
            }
            return allPlayer.ElementAt(playerIndex).Value.getVictoryPoints() >= 10;
        }
        
        /// <summary>
        /// Changes the currentPlayer index depending on which state of game we are currently in.
        /// </summary>
        /// <param name="clientPacket">a paket, the client sent, containing all relevant information about the client</param>
        /// <param name="playersId">integer representing the current player</param>
        private void changeCurrentPlayer(Packet clientPacket, int playersId)
        {
            if (!firstRound)
            {
                if (inGameStartupPhase && currentPlayer == playerAmount - 1) //2nd round in start phase
                {
                    inGameStartupPhase = false; //after last players turn in 2nd round the startphase is over
                    currentPlayer = 0; //start with first player again
                    handleBeginRound(clientPacket);
                    Debug.LogWarning("SERVER: StartupPhase is over now");
                }
                else if (currentPlayer == playerAmount - 1) //start phase over
                {
                    currentPlayer = 0;
                }
                else
                {
                    currentPlayer++; //set currentPlayer to next player
                }
                serverRequest.notifyNextPlayer(currentPlayer, playersId);
            }
            else //first round in the game
            {
                if (currentPlayer == 0)
                {
                    firstRound = false;
                }
                else
                {
                    currentPlayer--; //in first round turn order is reversed
                }
                serverRequest.notifyNextPlayer(currentPlayer, playersId);
            }
        }

        /// <summary>
        /// Checks, if current player is allowed to build either a village, city or road. Reacts different for the startup phase and the rest of the game.
        /// </summary>
        /// <param name="currentServerPlayer">represents the current player</param>
        /// <param name="buildingType">specifies what building is wished to be build by the curent player</param>
        /// <param name="posInArray">integer representation of the node or edge where the building shall be placed</param>
        /// <param name="playerColor">same color as the current player. the builded structure will be colored in the same way.</param>
        /// <param name="clientPacket">a paket, the client sent, containing all relevant information about the client</param>
        private void buildStructure(ServerPlayer currentServerPlayer, BUYABLES buildingType, int posInArray, PLAYERCOLOR playerColor, Packet clientPacket)
        {
            switch (buildingType)
            {
                case BUYABLES.VILLAGE:
                    //in the first 2 rounds only villages and roads are allowed to be build in alternating order
                    if (inGameStartupPhase
                        && !villageBuilt //player allowed to build if no village was build last
                        && gameBoard.canPlaceBuilding(posInArray, playerColor, BUILDING_TYPE.VILLAGE, inGameStartupPhase))
                    {
                        mandatoryNodeID = posInArray; //represents the mandatory node where a road has to be build adjacent to
                        villageBuilt = true; //next building placeable for the current player has to be a road

                        currentServerPlayer.reduceLeftVillages();
                        gameBoard.placeBuilding(posInArray, playerColor, BUILDING_TYPE.VILLAGE);
                        serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);

                        // distribute ressources for first 2 villages
                        int[] distributedResources = gameBoard.distributeFirstResources(posInArray);
                        for (int i = 0; i < distributedResources.Length; i++)
                        {
                            currentServerPlayer.setResourceAmount((RESOURCETYPE)i, distributedResources[i]);
                        }
                        //notify clients about changes
                        updateOwnPlayer(currentPlayer);
                        updateRepPlayers();
                        return;
                    }


                    if (!inGameStartupPhase //game start is over
                        && currentServerPlayer.canBuyBuyable(buildingType) //check if resource amount of current player is sufficient
                        && gameBoard.canPlaceBuilding(posInArray, playerColor, BUILDING_TYPE.VILLAGE, inGameStartupPhase))
                    {
                        currentServerPlayer.buyBuyable(buildingType);
                        currentServerPlayer.reduceLeftVillages();
                        gameBoard.placeBuilding(posInArray, playerColor, BUILDING_TYPE.VILLAGE);
                        serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);

                        //notify clients about changes
                        updateOwnPlayer(currentPlayer);
                        updateRepPlayers();
                        return;
                    }
                    break;
                case BUYABLES.CITY:
                    if (!inGameStartupPhase //cities prohibited to be build in start phase 
                        && currentServerPlayer.canBuyBuyable(buildingType) //check if resource amount of current player is sufficient
                        && gameBoard.canPlaceBuilding(posInArray, playerColor, BUILDING_TYPE.CITY, inGameStartupPhase))
                    {
                        currentServerPlayer.buyBuyable(buildingType);
                        currentServerPlayer.reduceLeftCities();
                        gameBoard.placeBuilding(posInArray, playerColor, BUILDING_TYPE.CITY);
                        serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);

                        //notify clients about changes
                        updateOwnPlayer(currentPlayer);
                        updateRepPlayers();
                        return;
                    }
                    break;
                case BUYABLES.ROAD:
                    if (inGameStartupPhase //in the first 2 rounds only villages and roads are allowed to be build in alternating order
                        && villageBuilt //player allowed to build if a village was build last
                        && gameBoard.canPlaceRoad(posInArray, mandatoryNodeID, playerColor))
                    {
                        villageBuilt = false; //next building placeable for the current player has to be a road
                        currentServerPlayer.reduceLeftRoads();
                        gameBoard.placeRoad(posInArray, playerColor);
                        mandatoryNodeID = -1; //reset mandatoryNode
                        serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);
                        //notify clients about changes
                        updateOwnPlayer(currentPlayer);
                        updateRepPlayers();
                        //end round, only in start phase
                        changeCurrentPlayer(clientPacket, currentPlayer);
                        return;
                    }
                    if (!inGameStartupPhase //start phase over
                        && currentServerPlayer.canBuyBuyable(buildingType) //check if resource amount of current player is sufficient
                        && gameBoard.canPlaceRoad(posInArray, playerColor))
                    {
                        currentServerPlayer.buyBuyable(buildingType);
                        currentServerPlayer.reduceLeftRoads();
                        gameBoard.placeRoad(posInArray, playerColor);
                        serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);

                        //notify clients about changes
                        updateOwnPlayer(currentPlayer);
                        updateRepPlayers();
                        return;
                    }
                    break;
                default: Debug.Log("SERVER: handleBuild(): wrong BUYABLES"); break;
            }
            //reject request if no condition is met
            serverRequest.notifyRejection(currentServerPlayer.getPlayerID(), "Building can't be built");
        }
    }
}