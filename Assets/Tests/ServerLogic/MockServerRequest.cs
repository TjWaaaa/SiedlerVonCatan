using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Enums;
using Networking.Interfaces;
using NUnit.Framework;

namespace Tests.ServerLogic
{
    /// <summary>
    /// All Methods in here are called indirectly from ServerReceive.
    /// </summary>
    public class MockServerRequest : ServerToClientCommunication
    {
        private bool joinedOnce = false; // flag that tests in notifyClientJoined aren't run twice.


        public void notifyClientJoined(ArrayList playerInformation, string lobbyIP)
        {
            // make sure this runs only once
            if (!joinedOnce) {
                // determine own IP address
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ipAddress = null;
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip.ToString();
                }
            }
            
            
            Assert.AreEqual(ipAddress, lobbyIP);
            Assert.AreEqual(PLAYERCOLOR.RED, ((object[]) playerInformation[0])[2]);
            Assert.AreEqual("Horst", ((object[]) playerInformation[0])[1]);
            Assert.AreEqual(1, ((object[]) playerInformation[0])[0]);

            joinedOnce = true;
            }
        }

        public void gamestartInitialize(Hexagon[][] gameBoard)
        {
            // check of Board is not empty, a random content cant be checked
            Assert.IsNotEmpty(gameBoard);
        }

        public void notifyObjectPlacement(BUYABLES buildType, int buildID, PLAYERCOLOR color)
        {
            throw new System.NotImplementedException();
        }

        public void notifyNextPlayer(int playerIndex, int previousPlayerIndex)
        {
            Assert.AreEqual(0, previousPlayerIndex);
            Assert.AreEqual(1, playerIndex);
        }

        public void notifyVictory(string playerName, PLAYERCOLOR playerColor)
        {
            throw new System.NotImplementedException();
        }

        public void notifyClientDisconnect(string playerName, PLAYERCOLOR playerColor)
        {
            throw new System.NotImplementedException();
        }

        public void notifyRejection(int playerID, string errorMessage)
        {
            throw new System.NotImplementedException();
        }

        public void notifyPlayerReady(int currentClientID, string playerName, bool readyStatus)
        {
            Assert.AreEqual(true, readyStatus);
        }

        public void notifyRollDice(int[] diceResult)
        {
            throw new System.NotImplementedException();
        }

        public void acceptBuyDevelopement(int leftDevCards)
        {
            throw new System.NotImplementedException();
        }

        public void notifyAcceptPlayDevelopement(int playerID, DEVELOPMENT_TYPE developmentCard, string playerName)
        {
            throw new System.NotImplementedException();
        }

        public void notifyAcceptTradeOffer(int playerID, int buttonNumber)
        {
            throw new System.NotImplementedException();
        }

        public void updateOwnPlayer(int[] updateLeftBuildings, Dictionary<RESOURCETYPE, int> updateResources, Dictionary<DEVELOPMENT_TYPE, int> updateDevCards, int playerID)
        {
            throw new System.NotImplementedException();
        }

        public void updateRepPlayers(int[][] updateNumbers)
        {
            throw new System.NotImplementedException();
        }
    }
}