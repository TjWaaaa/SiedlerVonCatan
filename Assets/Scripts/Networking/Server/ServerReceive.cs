using System.Collections;
using System.Collections.Generic;
using Networking.Communication;
using UnityEngine;
using Networking.Interfaces;
using Networking.Package;
using Player;

namespace Networking.ServerSide
{
    public class ServerGameLogic : INetworkableServer
    {
        private readonly List<ServerPlayer> allPlayer = new List<ServerPlayer>();
        private readonly Stack<Color> possibleColors = new Stack<Color>();
        private readonly ServerRequest serverRequest = new ServerRequest();

        public ServerGameLogic()
        {
            possibleColors.Push(Color.yellow);
            possibleColors.Push(Color.white);
            possibleColors.Push(Color.blue);
            possibleColors.Push(Color.red);
        }    

        public void generatePlayer(int playerId)
        {
            allPlayer.Add(new ServerPlayer(playerId));
        }

        public int[] rollDices()
        {
            Debug.Log("handleBeginRound has been called");
            int[] diceNumbers = new int[2];
            // diceNumbers[0] = (int)Random.Range(1,7);
            // diceNumbers[1] = (int)Random.Range(1,7);

            System.Random r = new System.Random();
            diceNumbers[0] = r.Next(1,7);
            diceNumbers[1] = r.Next(1,7);
            return diceNumbers;
        }

        public void handleRequestJoinLobby(Packet clientPacket, int currentClientID)
        {
            // ankommender spieler: name setzten + farbe zuweisen
            // alle lobby daten zurücksenden

            ArrayList allPlayerInformation = new ArrayList();
            foreach (var player in allPlayer)
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
        
            foreach (ServerPlayer player in allPlayer)
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
                serverRequest.gamestartInitialize(new int[][]{});
            }

            // send error if no player was found
            // todo: send error to all?
            // serverRequest.notifyRejection(currentClientID, "You seem to be not existing...");
        }

        public void handleBeginRound(Packet clientPacket)
        {
            // Roll dices
            int[] diceNumbers = rollDices();
            
            Debug.Log(diceNumbers[0] + " " + diceNumbers[1]);
            serverRequest.notifyRollDice(diceNumbers);
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
            // TODO change method call => handleBeginRound should only be called after the new player is already set and all have been notified
            Debug.Log("handleEndTurn has been called");
            handleBeginRound(clientPacket);
        }

        public void handleClientDisconnectServerCall()
        {
            throw new System.NotImplementedException();
        }
        
    }
}