using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Networking
{
    public class ClientGameLogic : MonoBehaviour, INetworkableClient
    {
        /// <summary>
        /// Create a persistent ClientGameLogicObject that stays over scene changes.
        /// </summary>
        public void Start()
        {
            DontDestroyOnLoad(this);
        }

        
        /// <summary>
        /// Call the ThreadManager's updateMainThread() method every frame.
        /// </summary>
        public void Update()
        {
            ThreadManager.updateMainThread();
        }

        /// <summary>
        /// converts a float array with r,g,b,a to a Color object
        /// </summary>
        /// <param name="values">r,g,b,a float values</param>
        /// <returns>Color object</returns>
        private Color decodeColor(float[] values)
        {
            Color color = new Color(values[0], values[1], values[2], values[3]);
            return color;
        }

        public void handleClientJoined(Packet serverPacket)
        {
            // für jeden player:
            // prefab instanziieren mit daten
            Debug.Log("Client recieved new package: " + PacketSerializer.objectToJsonString(serverPacket));

            foreach (JArray item in serverPacket.lobbyContent)
            {
                try
                {
                    string playerName = item[0].ToObject<string>();
                    Color playerColor = decodeColor(item[1].ToObject<float[]>());
                    Debug.Log("Client PlayerName: " + playerName); //done? fix verhexeln von Namen (auf serverseite gehen zeichen kaputt und werden zu ?)
                    Debug.Log("Client PlayerColor: " + playerColor); // ¯\_(ツ)_/¯
                    
                    // DODO: display this fancy data in the Lobby
                    RepresentJoinigClients.representNewPlayer(playerName, playerColor);
                } catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }

        public void handlePlayerReadyNotification(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleGameStartInitialize(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleObjectPlacement(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleNextPlayer()
        {
            throw new System.NotImplementedException();
        }

        public void handleVictory(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleClientDisconnect(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleRejection(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAccpetBeginRound(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAcceptTradeBank(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAcceptBuild(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleGetResources(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAcceptBuyDevelopement(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAcceptPlayDevelopement(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }
    }
}