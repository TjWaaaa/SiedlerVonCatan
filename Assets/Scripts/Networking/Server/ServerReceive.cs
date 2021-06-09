using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<int, ServerPlayer> allPlayer = new Dictionary<int, ServerPlayer>();

        private int playerAmount = 0;
        private int currentPlayer = 0;
        private int mandatoryNodeID;
        private bool firstRound = true;
        private bool inGameStartupPhase = true;
        private bool villageBuilt = false;
        private readonly Stack<PLAYERCOLOR> possibleColors = new Stack<PLAYERCOLOR>();
        private readonly ServerRequest serverRequest = new ServerRequest();

        private Board gameBoard = new Board();
        private Stack<DEVELOPMENT_TYPE> shuffledDevCardStack = new Stack<DEVELOPMENT_TYPE>();
        private DEVELOPMENT_TYPE[] unshuffledDevCardArray = { DEVELOPMENT_TYPE.VICTORY_POINT, 
            DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT, 
            DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT };

        public ServerReceive()
        {
            possibleColors.Push(PLAYERCOLOR.YELLOW);
            possibleColors.Push(PLAYERCOLOR.WHITE);
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
                    PLAYERCOLOR playerColor = player.getPlayerColor(); // needs to be done, because Color is not serializable ¯\_(ツ)_/¯
                    allPlayerInformation.Add(new object[] { player.getPlayerID(), player.getPlayerName(), player.getPlayerColor() });
                }
            }

            serverRequest.notifyClientJoined(allPlayerInformation, Server.serverIP.ToString());
        }

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
                serverRequest.notifyNextPlayer(currentPlayer);
                Debug.Log($"SERVER: Starting the game with player {currentPlayer}");
            }

            // Send error if no player was found
            // Todo: send error to all?
            // serverRequest.notifyRejection(currentClientID, "You seem to be not existing...");
        }

        public void handleBeginRound(Packet clientPacket)
        {
            // if (isCurrentPlayer(clientPacket.myPlayerID))
            // {
            //     Debug.LogWarning($"SERVER: Client request rejected from client {clientPacket.myPlayerID}");
            //     serverRequest.notifyRejection(clientPacket.myPlayerID, "You are not allowed to begin round!");
            //     return;
            // }

            // Roll dices

            int[] diceNumbers = rollDices();
            serverRequest.notifyRollDice(diceNumbers);
            
            Debug.Log("Würfel gewürfelt");
            // Distribute ressources
            for (int playerIndex = 0; playerIndex < allPlayer.Count; playerIndex++)
            {
                ServerPlayer player = allPlayer.ElementAt(playerIndex).Value;
                int[] distributedResources = gameBoard.distributeResources(diceNumbers[0] + diceNumbers[1], player.getPlayerColor());
                Debug.Log("Player " + playerIndex + " gets: " + distributedResources[0] + distributedResources[1] + distributedResources[2] + distributedResources[3] + distributedResources[4]);

                for (int i = 0; i < distributedResources.Length; i++)
                {
                    player.setResourceAmount((RESOURCETYPE) i, distributedResources[i]);
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
                serverRequest.notifyRejection(clientPacket.myPlayerID, "You are not allowed to trade with bank!");
                return;
            }
            Debug.LogWarning("offer: " + clientPacket.tradeResourcesOffer + "; expect: " + clientPacket.tradeResourcesExpect);
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
                int buttonNumber = clientPacket.buttonNumber;
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
            int posInArray = clientPacket.buildID;

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
                    serverRequest.notifyRejection(clientPacket.myPlayerID, "You are not allowed to buy a developmentcard!");
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
            }
            else
            {
                serverRequest.notifyRejection(clientPacket.myPlayerID, "Method HANDLE_BUY_DEVELOPMENT during game startphase prohibited");
            }
        }

        public void handlePlayDevelopement(Packet clientPacket)
        {
            if (!inGameStartupPhase)
            {
                if (isNotCurrentPlayer(clientPacket.myPlayerID))
                {
                    Debug.LogWarning($"SERVER: Client request rejected from client {clientPacket.myPlayerID}");
                    serverRequest.notifyRejection(clientPacket.myPlayerID, "You are not allowed to play a developmentcard!");
                    return;
                }
                Debug.Log(clientPacket.developmentCard);
                Debug.Log("SERVER: CurrentPlayer has enough cards: " + allPlayer.ElementAt(currentPlayer).Value.getDevCardAmount(clientPacket.developmentCard));
                if (allPlayer.ElementAt(currentPlayer).Value.getDevCardAmount(clientPacket.developmentCard)>0)
                {
                    allPlayer.ElementAt(currentPlayer).Value.playDevCard(clientPacket.developmentCard);

                    // Sending Packages
                    updateRepPlayers();
                    updateOwnPlayer(currentPlayer);
                    serverRequest.notifyAcceptPlayDevelopement(allPlayer.ElementAt(currentPlayer).Key, clientPacket.developmentCard, allPlayer.ElementAt(currentPlayer).Value.getPlayerName());
                }
                else
                {
                    serverRequest.notifyRejection(allPlayer.ElementAt(currentPlayer).Key, "You can't play this developement Card");
                }
            }
            else
            {
                serverRequest.notifyRejection(clientPacket.myPlayerID, "Method HANDLE_PLAY_DEVELOPMENT during game startphase prohibited");
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
                serverRequest.notifyVictory(allPlayer.ElementAt(currentPlayer).Value.getPlayerName(), allPlayer.ElementAt(currentPlayer).Value.getPlayerColor());
                return;
            }

            // Begin next round
            if (!inGameStartupPhase) 
            {
                changeCurrentPlayer(clientPacket);
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

        private bool isNotCurrentPlayer(int clientID)
        {
            var currentPlayerObject = allPlayer.ElementAt(currentPlayer).Value;
            //Debug.LogWarning($"comparing clientID: {clientID} and currentID: {currentPlayerObject.getPlayerID()}");
            if (currentPlayerObject.getPlayerID() == clientID)
            {
                return false;
            }

            return true;
        }

        private int[] rollDices()
        {
            Debug.Log("SERVER: Dices are being rolled");
            System.Random r = new System.Random();
            int[] diceNumbers = new int[2];
            diceNumbers[0] = r.Next(1, 7);
            diceNumbers[1] = r.Next(1, 7);
            Debug.Log($"SERVER: Dice value 1: {diceNumbers[0]} // Dice value 2: {diceNumbers[1]}");
            return diceNumbers;
        }

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

        public void generatePlayer(int playerId)
        {
            ServerPlayer newPlayer = new ServerPlayer(playerId);
            allPlayer.Add(playerId, newPlayer);
            playerAmount++;
        }

        private Stack<DEVELOPMENT_TYPE> generateRandomDevCardStack(DEVELOPMENT_TYPE[] array)
        {
            return new Stack<DEVELOPMENT_TYPE>(array.OrderBy(n => Guid.NewGuid()).ToArray());
        }

        private void updateOwnPlayer(int playerIndex)
        {
            serverRequest.updateOwnPlayer(
                allPlayer.ElementAt(playerIndex).Value.convertFromSPToOP(), // int[] with left buildings
                allPlayer.ElementAt(playerIndex).Value.convertSPToOPResources(), // Resource Dictionary
                allPlayer.ElementAt(playerIndex).Value.convertSPToOPDevCards(), // DevCard Dictonary
                allPlayer.ElementAt(playerIndex).Key);
        }

        private void updateRepPlayers()
        {
            serverRequest.updateRepPlayers(convertSPAToRPA());
        }

        private bool didThisPlayerWin(int playerIndex)
        {
            if (allPlayer.ElementAt(playerIndex).Value.getVictoryPoints() >= 10)
            {
                return true;
            }
            return false;
        }

        private void changeCurrentPlayer(Packet clientPacket)
        {
            if (!firstRound)
            {
                if (currentPlayer == playerAmount - 1 && inGameStartupPhase)
                {
                    inGameStartupPhase = false;
                    handleBeginRound(clientPacket);
                    Debug.Log("SERVER: StartupPhase is over now");
                }

                if (currentPlayer == playerAmount - 1)
                {
                    currentPlayer = 0;
                }
                else
                {
                    currentPlayer++;
                }
                serverRequest.notifyNextPlayer(currentPlayer);
            }
            else
            {
                if (currentPlayer == 0)
                {
                    firstRound = false;

                    if (currentPlayer == playerAmount - 1)
                    {
                        currentPlayer = 0;
                    }
                    else
                    {
                        currentPlayer++;
                    }
                }
                else
                {
                    currentPlayer--;
                }
                serverRequest.notifyNextPlayer(currentPlayer);
            }
        }

        private void buildStructure(ServerPlayer currentServerPlayer, BUYABLES buildingType, int posInArray, PLAYERCOLOR playerColor, Packet clientPacket)
        {
            // if (currentServerPlayer.canBuyBuyable(buildingType))
            // {
            switch (buildingType)
            {
                case BUYABLES.VILLAGE:
                    if (inGameStartupPhase
                        && !villageBuilt 
                        && gameBoard.canPlaceBuilding(posInArray, playerColor, BUILDING_TYPE.VILLAGE, inGameStartupPhase))
                    {
                        mandatoryNodeID = posInArray;
                        villageBuilt = true;
                        currentServerPlayer.reduceLeftVillages();
                        gameBoard.placeBuilding(posInArray, playerColor, BUILDING_TYPE.VILLAGE);
                        serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);

                        // distribute ressources for first 2 villages
                        int[] distributedResources = gameBoard.distributeFirstResources(posInArray);
                        for (int i = 0; i < distributedResources.Length; i++)
                        {
                            currentServerPlayer.setResourceAmount((RESOURCETYPE) i, distributedResources[i]);
                        }

                        updateOwnPlayer(currentPlayer);
                        updateRepPlayers();
                        return;
                    }
                    if (!inGameStartupPhase
                        && currentServerPlayer.canBuyBuyable(buildingType)
                        && gameBoard.canPlaceBuilding(posInArray, playerColor, BUILDING_TYPE.VILLAGE, inGameStartupPhase))
                    {
                        currentServerPlayer.buyBuyable(buildingType);
                        currentServerPlayer.reduceLeftVillages();
                        gameBoard.placeBuilding(posInArray, playerColor, BUILDING_TYPE.VILLAGE);
                        serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);
                        updateOwnPlayer(currentPlayer);
                        updateRepPlayers();
                        return;
                    }
                    break;
                case BUYABLES.CITY:
                    if (!inGameStartupPhase
                        && currentServerPlayer.canBuyBuyable(buildingType)
                        && gameBoard.canPlaceBuilding(posInArray, playerColor, BUILDING_TYPE.CITY, inGameStartupPhase))
                    {
                        currentServerPlayer.buyBuyable(buildingType);
                        currentServerPlayer.reduceLeftCities();
                        gameBoard.placeBuilding(posInArray, playerColor, BUILDING_TYPE.CITY);
                        serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);
                        updateOwnPlayer(currentPlayer);
                        updateRepPlayers();
                        return;
                    }
                    break;
                case BUYABLES.ROAD:
                    if (inGameStartupPhase
                        && gameBoard.canPlaceRoad(posInArray, mandatoryNodeID, playerColor))
                    {
                        mandatoryNodeID = -1;
                        villageBuilt = false;
                        currentServerPlayer.reduceLeftRoads();
                        gameBoard.placeRoad(posInArray, playerColor);
                        serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);
                        updateOwnPlayer(currentPlayer);
                        updateRepPlayers();
                        changeCurrentPlayer(clientPacket);
                        return;
                    }
                    if (!inGameStartupPhase
                        && currentServerPlayer.canBuyBuyable(buildingType)
                        && gameBoard.canPlaceRoad(posInArray, playerColor))
                    {
                        currentServerPlayer.buyBuyable(buildingType);
                        currentServerPlayer.reduceLeftRoads();
                        gameBoard.placeRoad(posInArray, playerColor);
                        serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);
                        updateOwnPlayer(currentPlayer);
                        updateRepPlayers();
                        return;
                    }
                    break;
                default: Debug.Log("SERVER: handleBuild(): wrong BUYABLES"); break;
            }

            serverRequest.notifyRejection(currentServerPlayer.getPlayerID(), "Building cant be built");
        }
            // else
            // {
            //     serverRequest.notifyRejection(currentServerPlayer.getPlayerID(), "You don't have enough resources");
            //     Debug.Log("SERVER: not enough resources");
            // }
    }
}