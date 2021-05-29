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
        private bool villageBuild = false;
        private readonly Stack<PLAYERCOLOR> possibleColors = new Stack<PLAYERCOLOR>();
        private readonly ServerRequest serverRequest = new ServerRequest();

        private Board gameBoard = new Board();
        private Stack<DEVELOPMENT_TYPE> shuffledDevCardStack = new Stack<DEVELOPMENT_TYPE>();
        private DEVELOPMENT_TYPE[] unshuffledDevCardArray = { DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT, DEVELOPMENT_TYPE.VICTORY_POINT };

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

            // start game if all player are ready
            //todo: Boardgenerator!
            if (runGame)
            {
                //int[][] updateRepPlayers = convertSPAToRPA();
                currentPlayer = playerAmount - 1;
                serverRequest.gamestartInitialize(gameBoard.getHexagonsArray());
                shuffledDevCardStack = generateRandomDevCardStack(unshuffledDevCardArray);
            }

            // send error if no player was found
            // todo: send error to all?
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

                for (int i = 0; i < distributedResources.Length; i++)
                {
                    player.setResourceAmount((RESOURCETYPE) i, distributedResources[i]);
                }
                updateOwnPlayer(currentPlayer);
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
                    DEVELOPMENT_TYPE temp = shuffledDevCardStack.Peek();
                    Debug.Log("Server: A devCard has been popped out of the stack, there are only so much more: " + shuffledDevCardStack.Count);
                    allPlayer.ElementAt(currentPlayer).Value.setNewDevCard(temp);

                    // Sending Packages
                    updateRepPlayers();
                    updateOwnPlayer(currentPlayer);
                    serverRequest.acceptBuyDevelopement(allPlayer.ElementAt(currentPlayer).Key, shuffledDevCardStack.Pop());
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
                Debug.Log("SERVER: CurrentPlayer has enough cards: " + allPlayer.ElementAt(currentPlayer).Value.enoughDevCards(clientPacket.developmentCard));
                if (allPlayer.ElementAt(currentPlayer).Value.enoughDevCards(clientPacket.developmentCard))
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

            // Updating Representative Players TODO: DELETE WHEN RESOURCE DISTRIBUTION IS IMPLEMENTED
            updateRepPlayers();
            updateOwnPlayer(currentPlayer);

            // Begin next round
            if (!inGameStartupPhase) 
            {
                changeCurrentPlayer(clientPacket);
                Debug.Log("SERVER: Current Player index: " + currentPlayer); 
                serverRequest.notifyNextPlayer(currentPlayer);
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
            newPlayer.setResourceAmount(RESOURCETYPE.SHEEP, 15);
            newPlayer.setResourceAmount(RESOURCETYPE.WOOD, 15);
            newPlayer.setResourceAmount(RESOURCETYPE.BRICK, 15);
            newPlayer.setResourceAmount(RESOURCETYPE.ORE, 15);
            newPlayer.setResourceAmount(RESOURCETYPE.WHEAT, 15);
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

        public void changeCurrentPlayer(Packet clientPacket)
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
            }
        }

        private void buildStructure(ServerPlayer currentServerPlayer, BUYABLES buildingType, int posInArray, PLAYERCOLOR playerColor, Packet clientPacket)
        {
            if (currentServerPlayer.canBuyBuyable(buildingType))
            {
                switch (buildingType)
                {
                    case BUYABLES.VILLAGE:
                    case BUYABLES.CITY:
                        bool buildSuccessfull = gameBoard.placeBuilding(posInArray, playerColor, inGameStartupPhase);
                        if (inGameStartupPhase && buildSuccessfull && buildingType == BUYABLES.VILLAGE && !villageBuild && currentServerPlayer.getLeftVillages() > 3)
                        {
                            currentServerPlayer.buildVillage();
                            mandatoryNodeID = posInArray;
                            villageBuild = true;
                            serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);
                            updateOwnPlayer(currentPlayer);
                            updateRepPlayers();
                            return;
                        }

                        if (!inGameStartupPhase && buildSuccessfull)
                        {
                            // Missing restrictions!
                            currentServerPlayer.buyBuyable(buildingType);
                            serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);
                            updateOwnPlayer(currentPlayer);
                            updateRepPlayers();
                            return;
                        }
                        break;

                    case BUYABLES.ROAD:
                        if (inGameStartupPhase && gameBoard.placeRoad(posInArray, mandatoryNodeID, playerColor) && currentServerPlayer.getLeftStreets() > 13)
                        {
                            currentServerPlayer.buildStreet();
                            mandatoryNodeID = -1;
                            villageBuild = false;
                            serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);
                            updateOwnPlayer(currentPlayer);
                            updateRepPlayers();
                            changeCurrentPlayer(clientPacket);
                            return;
                        }

                        if (!inGameStartupPhase && gameBoard.placeRoad(posInArray, playerColor))
                        {
                            currentServerPlayer.buyBuyable(buildingType);
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
            else
            {
                serverRequest.notifyRejection(currentServerPlayer.getPlayerID(), "You don't have enough resources");
                Debug.Log("SERVER: not enough resources");
            }
        }
    }
}