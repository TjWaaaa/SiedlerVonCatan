﻿using System.Collections;
using System.Collections.Generic;
using Enums.Communication;
using UnityEngine;

namespace Enums
{
    public class ServerGameLogic : INetworkableServer
    {
        private readonly List<Player> allPlayer = new List<Player>();
        private readonly Stack<Color> possibleColors = new Stack<Color>();
        private readonly ServerRequest serverRequest = new ServerRequest();

        public ServerGameLogic()
        {
            possibleColors.Push(Color.blue);
            possibleColors.Push(Color.red);
            possibleColors.Push(Color.white);
            possibleColors.Push(Color.green);
        }    

        public void generatePlayer(int playerId)
        {
            allPlayer.Add(new Player(playerId));
        }

        public void handleRequestJoinLobby(Packet clientPacket, int currentClientID)
        {
            // ankommender spieler: name setzten + farbe zuweisen
            // alle lobby daten zurücksenden

            ArrayList allPlayerInformation = new ArrayList();
            foreach (var player in allPlayer)
            {
                if (player.GetPlayerID() == currentClientID)
                {
                    player.setPlayerName(clientPacket.playerName);
                    player.setColor(possibleColors.Pop());
                }

                // look for all Players that are fully initialized and add it to ArrayList that updates client lobbies. 
                if (player.GetName() != null)
                {
                    Color playerColor = player.GetColor(); // needs to be done, because Color is not serializable ¯\_(ツ)_/¯
                    allPlayerInformation.Add(new object[]
                        {player.GetPlayerID(), player.GetName(), new float[] {playerColor.r, playerColor.g, playerColor.b, playerColor.a}});
                }
            }
            
            serverRequest.notifyClientJoined(allPlayerInformation);
        }

        
        public void handleRequestPlayerReady(Packet clientPacket, int currentClientID)
        {
        
            bool runGame = true;
        
            foreach (Player player in allPlayer)
            {
                if (player.GetPlayerID() == currentClientID)
                {
                    player.setIsReady(clientPacket.isReady);
                    serverRequest.notifyPlayerReady(currentClientID, player.GetName(), clientPacket.isReady);
                }
                
                // check if all players are ready
                if (!player.getIsReady())
                {
                    runGame = false;
                }
            }
            
            // start game if all player are ready
            //todo: Boardgenerator!
            serverRequest.gamestartInitialize(new int[][]{});
            
            // send error if no player was found
            // todo: send error to all?
            // serverRequest.notifyRejection(currentClientID, "You seem to be not existing...");
        }

        public void handleBeginRound(Packet clientPacket)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public void handleClientDisconnectServerCall()
        {
            throw new System.NotImplementedException();
        }
    }
}