using System;
using Enums;
using Newtonsoft.Json.Linq;
using Networking.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Networking.Package;
using Networking.Communication;

namespace Networking.ClientSide
{
    public class ClientGameLogic : MonoBehaviour, INetworkableClient
    {
        public int myID { get; private set; }

        private PrefabFactory prefabFactory;
        private GameObject scrollViewContent;
        private int playerNumber = 1;
        private readonly ClientRequest clientRequest = new ClientRequest();


        private GameObject diceHolder;

        /// <summary>
        /// Create a persistent ClientGameLogicObject that stays over scene changes.
        /// </summary>
        public void Start()
        {
            prefabFactory = GameObject.Find("PrefabFactory").GetComponent<PrefabFactory>();
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

        
        /// <summary>
        /// Add a player list entry to the lobby.
        /// If a player already exists, its values are updated.
        /// </summary>
        /// <param name="playerName">Name of the player</param>
        /// <param name="playerColor">Color of the player</param>
        /// <param name="currentPlayerID">ID of the player</param>
        public void representNewPlayer(int currentPlayerID, string playerName, Color playerColor)
        {
            try
            {
                scrollViewContent = GameObject.Find("Canvas/Scroll View/Viewport/Content");
                
                GameObject listItem = GameObject.Find(currentPlayerID.ToString()); // search for already existing list entries.

                if (listItem == null) // If the list entry for a player doesn't exist --> instantiate new.
                {
                    listItem = prefabFactory.getPrefab(PREFABS.PLAYER_LIST_ITEM, scrollViewContent.transform);
                    listItem.transform.Find("No.").GetComponent<Text>().text = playerNumber.ToString();
                    playerNumber++;
                    listItem.transform.Find("No.").GetComponent<Text>().color = playerColor;
                    listItem.transform.Find("Player").GetComponent<Text>().text = playerName;
                    //listItem.transform.Find("Color").GetComponent<Image>().color = playerColor;

                    if (currentPlayerID != myID) // Disable all toggle components which don't belong to the local client
                    {
                        listItem.transform.Find("IsReady").GetComponent<Toggle>().enabled = false;
                        listItem.transform.Find("IsReady").GetComponent<PlayerReady>().enabled = false;
                    }
                    listItem.name = currentPlayerID.ToString();
                }
                else // List entry does already exist --> update name and color 
                {
                    listItem.transform.Find("Player").GetComponent<Text>().text = playerName;
                    listItem.transform.Find("Player").GetComponent<Text>().color = playerColor;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        //---------------------------------------------- Interface INetworkableClient implementation ----------------------------------------------
        
        public void handleClientJoined(Packet serverPacket)
        {
            // for each player:
            // initiialize prefab with data
            Debug.Log("Client recieved new package: " + PacketSerializer.objectToJsonString(serverPacket));

            myID = serverPacket.playerNumber;
            foreach (JArray item in serverPacket.lobbyContent)
            {
                try
                {
                    int currentPlayerID = item[0].ToObject<int>();
                    string playerName = item[1].ToObject<string>();
                    Color playerColor = decodeColor(item[2].ToObject<float[]>());

                    Debug.Log($"Client joined: Name: {playerName}, Color: {playerColor}, ID: {currentPlayerID}");
                    representNewPlayer(currentPlayerID, playerName, playerColor);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }

        public void handlePlayerReadyNotification(Packet serverPacket)
        {
            var gameObject = GameObject.Find(serverPacket.currentPlayerNumber.ToString());
            gameObject.transform.Find("IsReady").GetComponent<Toggle>().isOn = serverPacket.isReady;
        }

        public void handleGameStartInitialize(Packet serverPacket)
        {
            SceneManager.LoadScene("2_GameScene");
            Debug.Log("Client: Sie haben ein Spielbrett erhalten :)");
        }

        public void handleObjectPlacement(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleNextPlayer()
        {   
            Debug.Log("handleNextPlayer has been called");
            clientRequest.requestEndTurn();
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
            Debug.Log("handleAcceptBeginRound has been called");
            diceHolder = GameObject.FindGameObjectWithTag("diceHolder");
            Debug.Log(diceHolder.name + " diceHolder object");
            diceHolder.GetComponent<RenderRollDices>().renderRollDices(serverPacket.diceResult);
            Debug.Log(serverPacket.diceResult);
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