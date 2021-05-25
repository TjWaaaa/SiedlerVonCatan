using System.Collections;
using System.Collections.Generic;
using Enums;
using Networking.Communication;
using UnityEngine;
using Networking.Interfaces;
using Networking.Package;
using Player;

namespace Networking.ServerSide
{
    public class ServerGameLogic : INetworkableServer
    {
        private readonly Dictionary<int,ServerPlayer> allPlayer = new Dictionary<int, ServerPlayer>();

        //private RepresentativePlayer[] representativePlayerArray;
        private int playerAmount = 0;
        private int currentPlayer = 0;
        private readonly Stack<Color> possibleColors = new Stack<Color>();
        private readonly ServerRequest serverRequest = new ServerRequest();

        private Board gameBoard = new Board();

        public ServerGameLogic()
        {
            possibleColors.Push(Color.yellow);
            possibleColors.Push(Color.white);
            possibleColors.Push(Color.blue);
            possibleColors.Push(Color.red);
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
                    Color playerColor = player.getPlayerColor(); // needs to be done, because Color is not serializable ¯\_(ツ)_/¯
                    allPlayerInformation.Add(new object[]
                        {player.getPlayerID(), player.getPlayerName(), new float[] {playerColor.r, playerColor.g, playerColor.b, playerColor.a}});
                }
            }
            
            serverRequest.notifyClientJoined(allPlayerInformation);
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
            throw new System.NotImplementedException();
        }

        public void handleBuild(Packet clientPacket)
        {
            throw new System.NotImplementedException();
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
            // Change currentPlayer
            if (currentPlayer == playerAmount-1)
            {
                currentPlayer = 0;
            }
            else
            {
                currentPlayer++;
            }
            // Updating Representative Players
            serverRequest.updateRepPlayers(convertSPAToRPA());
            
            // TODO change method call => handleBeginRound should only be called after the new player is already set and all have been notified
            Debug.Log("handleEndTurn has been called");
            handleBeginRound(clientPacket);
        }

        public void handleClientDisconnectServerCall()
        {
            throw new System.NotImplementedException();
        }

    // Here come all the Logical methods
        public int[] rollDices()
        {
            Debug.Log("Dices are being rolled");
            System.Random r = new System.Random();
            int[] diceNumbers = new int[2];
            diceNumbers[0] = r.Next(1,7);
            diceNumbers[1] = r.Next(1,7);
            Debug.Log($"Dice value 1: {diceNumbers[0]} // Dice value 2: {diceNumbers[1]}");
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
            allPlayer.Add(playerId,newPlayer);
            playerAmount++;
        }
    }
}