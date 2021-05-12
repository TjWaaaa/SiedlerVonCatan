using System.Collections;
using System.Collections.Generic;
using Networking.Communication;
using UnityEngine;

namespace Networking
{
    public class ServerGameLogic : INetworkableServer
    {
        private List<Player> allPlayer = new List<Player>();
        private Stack<Color> possibleColors = new Stack<Color>();

        public ServerGameLogic()
        {
            possibleColors.Push(Color.blue);
            possibleColors.Push(Color.red);
            possibleColors.Push(Color.green);
            possibleColors.Push(Color.white);
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
                        {player.GetName(), new float[] {playerColor.r, playerColor.g, playerColor.b, playerColor.a}});
                }
            }

            ServerRequest serverRequest = new ServerRequest();
            serverRequest.notifyClientJoined(allPlayerInformation);
        }

        public void handleRequestPlayerReady(Packet clientPacket, int currentClientID)
        {
            throw new System.NotImplementedException();
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