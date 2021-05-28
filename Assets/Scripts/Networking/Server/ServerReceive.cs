using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Networking.Communication;
using UnityEngine;
using Networking.Interfaces;
using Networking.Package;
using Player;
using PlayerColor;

namespace Networking.ServerSide
{
    public class ServerReceive : INetworkableServer
    {
        private Dictionary<int, ServerPlayer> allPlayer = new Dictionary<int, ServerPlayer>();

        private int playerAmount = 0;
        private int currentPlayer = 0;
        private bool firstRound = true;
        private bool inGameStartupPhase = true;
        private readonly Stack<PLAYERCOLOR> possibleColors = new Stack<PLAYERCOLOR>();
        private readonly ServerRequest serverRequest = new ServerRequest();

        private Board gameBoard = new Board();

        public ServerReceive()
        {
            possibleColors.Push(PLAYERCOLOR.YELLOW);
            possibleColors.Push(PLAYERCOLOR.WHITE);
            possibleColors.Push(PLAYERCOLOR.BLUE);
            possibleColors.Push(PLAYERCOLOR.RED);
        }

        // Here come all handling methods
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
            }

            // send error if no player was found
            // todo: send error to all?
            // serverRequest.notifyRejection(currentClientID, "You seem to be not existing...");
        }

        public void handleBeginRound(Packet clientPacket)
        {
            // Roll dices
            int[] diceNumbers = rollDices();
            serverRequest.notifyRollDice(diceNumbers);
            // Distribute ressources
        }

        public void handleTradeBank(Packet clientPacket)
        {
            allPlayer.ElementAt(currentPlayer).Value.trade(clientPacket.tradeResourcesOffer, clientPacket.tradeResourcesExpect);

            serverRequest.updateRepPlayers(convertSPAToRPA());
            serverRequest.updateOwnPlayer(
                allPlayer.ElementAt(currentPlayer).Value.convertFromSPToOP(), // int[] with left buildings
                allPlayer.ElementAt(currentPlayer).Value.convertSPToOPResources(), // Resource Dictionary
                allPlayer.ElementAt(currentPlayer).Key);


        }

        public void handleTradeOffer(Packet clientPacket)
        {
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

        public void handleBuild(Packet clientPacket)
        {
            ServerPlayer currentServerPlayer = allPlayer.ElementAt(currentPlayer).Value;
            BUYABLES buildingType = (BUYABLES)clientPacket.buildType;
            int posInArray = clientPacket.buildID;

            PLAYERCOLOR playerColor = allPlayer.ElementAt(currentPlayer).Value.getPlayerColor();

            if ((inGameStartupPhase && buildingType != BUYABLES.CITY) || currentServerPlayer.canBuyBuyable(buildingType))
            {
                switch (buildingType)
                {
                    case BUYABLES.VILLAGE:
                    case BUYABLES.CITY:
                        {
                            // Missing restrictions!
                            if (!inGameStartupPhase) { currentServerPlayer.buyBuyable(buildingType); }
                            serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);
                            serverRequest.updateRepPlayers(convertSPAToRPA());
                            serverRequest.updateOwnPlayer(
                                allPlayer.ElementAt(currentPlayer).Value.convertFromSPToOP(), // int[] with left buildings
                                allPlayer.ElementAt(currentPlayer).Value.convertSPToOPResources(), // Resource Dictionary
                                allPlayer.ElementAt(currentPlayer).Key);
                            return;
                        }
                    case BUYABLES.ROAD:
                        if (gameBoard.placeRoad(posInArray, playerColor))
                        {
                            // Missing restrictions!
                            if (!inGameStartupPhase) { currentServerPlayer.buyBuyable(buildingType); }
                            serverRequest.notifyObjectPlacement(buildingType, posInArray, playerColor);
                            serverRequest.updateRepPlayers(convertSPAToRPA());
                            serverRequest.updateOwnPlayer(
                                allPlayer.ElementAt(currentPlayer).Value.convertFromSPToOP(), // int[] with left buildings
                                allPlayer.ElementAt(currentPlayer).Value.convertSPToOPResources(), // Resource Dictionary
                                allPlayer.ElementAt(currentPlayer).Key);
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

        public void handleBuyDevelopement(Packet clientPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handlePlayDevelopement(Packet clientPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleEndTurn(Packet clientPacket)
        {
            Debug.Log("SERVER: handleEndTurn has been called");

            // Change currentPlayer
            if (!firstRound)
            {
                if(currentPlayer == playerAmount-1 && inGameStartupPhase){inGameStartupPhase = false;Debug.Log("SERVER: StartupPhase is over now");}
                currentPlayer = currentPlayer == playerAmount - 1 ? 0 : ++currentPlayer;
            }
            else
            {
                if (currentPlayer == 0) 
                { 
                    firstRound = false; 
                    currentPlayer = currentPlayer == playerAmount - 1 ? 0 : ++currentPlayer; 
                }
                else 
                { 
                    currentPlayer--; 
                }

            }
            Debug.Log("SERVER: Current Player index: " + currentPlayer);

            // Updating Representative Players
            serverRequest.updateRepPlayers(convertSPAToRPA());
            serverRequest.updateOwnPlayer(
                allPlayer.ElementAt(currentPlayer).Value.convertFromSPToOP(), // int[] with left buildings
                allPlayer.ElementAt(currentPlayer).Value.convertSPToOPResources(), // Resource Dictionary
                allPlayer.ElementAt(currentPlayer).Key);

            // Begin next round
            if (!inGameStartupPhase) { handleBeginRound(clientPacket); }


        }

        public void handleClientDisconnectServerCall(int disconnectedClientID)
        {
            var player = allPlayer[disconnectedClientID];
            serverRequest.notifyClientDisconnect(player.getPlayerName(), player.getPlayerColor());
        }

        // Here come all the Logical methods
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

        public int[][] convertSPAToRPA() // ServerPlayerArray / RepPlayerArray
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
    }
}