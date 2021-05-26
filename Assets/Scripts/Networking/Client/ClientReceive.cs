﻿using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Newtonsoft.Json.Linq;
using Networking.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Networking.Package;
using Networking.Communication;
using Player;
using PlayerColor;
using UI;

namespace Networking.ClientSide
{
    public class ClientGameLogic : MonoBehaviour, INetworkableClient
    {
        public int myID { get; private set; }

        private PrefabFactory prefabFactory;
        private GameObject scrollViewContent;
        private PlayerRepresentation playerRepresentation = new PlayerRepresentation();
        private OwnPlayerRepresentation ownPlayerRepresentation = new OwnPlayerRepresentation();

        //private RepresentativePlayer[] representativePlayerArray;
        public List<RepresentativePlayer> representativePlayers = new List<RepresentativePlayer>();
        public OwnClientPlayer ownClientPlayer;
        private int playerNumber = 1;

        private int currentPlayer = 0;
        private Hexagon[][] gameBoard;

        private Scene currentScene;
        private bool runFixedUpdate = true;
        private BoardGenerator boardGenerator;

        private GameObject diceHolder;

        /// <summary>
        /// Create a persistent ClientGameLogicObject that stays over scene changes.
        /// </summary>
        public void Start()
        {
            prefabFactory = GameObject.Find("PrefabFactory").GetComponent<PrefabFactory>();
            DontDestroyOnLoad(this);
        
            currentScene = SceneManager.GetActiveScene();
            boardGenerator = GetComponent<BoardGenerator>();
        }

        /// <summary>
        /// Call the ThreadManager's updateMainThread() method every frame.
        /// </summary>
        public void Update()
        {
            ThreadManager.updateMainThread();
        }

        public void FixedUpdate()
        {
            if (runFixedUpdate)
            {
                currentScene = SceneManager.GetActiveScene();

                if (currentScene.name == "2_GameScene")
                {
                    boardGenerator.instantiateGameBoard(gameBoard);
                    playerRepresentation.represent(representativePlayers.ToArray());
                    ownPlayerRepresentation.represent(ownClientPlayer);
                    runFixedUpdate = false;
                }
            }
        }

        /// <summary>
        /// converts a float array with r,g,b,a to a Color object
        /// </summary>
        /// <param name="values">r,g,b,a float values</param>
        /// <returns>Color object</returns>
        private Color decodeColor(PLAYERCOLOR playerColor)
        {
            switch (playerColor)
            {
                case PLAYERCOLOR.RED: return Color.red;
                case PLAYERCOLOR.BLUE: return Color.blue;
                case PLAYERCOLOR.WHITE: return Color.white;
                case PLAYERCOLOR.YELLOW: return Color.yellow;
                case PLAYERCOLOR.NONE:
                default: return Color.magenta;
            }
        }

        
        /// <summary>
        /// Add a player list entry to the lobby.
        /// If a player already exists, its values are updated.
        /// </summary>
        /// <param name="playerName">Name of the player</param>
        /// <param name="playerColor">Color of the player</param>
        /// <param name="currentPlayerID">ID of the player</param>
        public void representNewPlayer(int currentPlayerID, string playerName, PLAYERCOLOR playerColor)
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
                    listItem.transform.Find("No.").GetComponent<Text>().color = decodeColor(playerColor);
                    listItem.transform.Find("Player").GetComponent<Text>().text = playerName;

                    if (currentPlayerID != myID) // Disable all toggle components which don't belong to the local client
                    {
                        listItem.transform.Find("IsReady").GetComponent<Toggle>().enabled = false;
                        listItem.transform.Find("IsReady").GetComponent<PlayerReady>().enabled = false;
                    }
                    else
                    {
                        ownClientPlayer = new OwnClientPlayer(currentPlayerID);
                        Debug.Log("Created OwnClientPlayer with ID" + currentPlayerID);
                    }
                    listItem.name = currentPlayerID.ToString();
                    representativePlayers.Add( new RepresentativePlayer(currentPlayerID, playerName, decodeColor(playerColor)));
                    Debug.Log("client: "+ playerName + " created. Player Number " + representativePlayers.Count);
                }
                else // List entry does already exist --> update name and color 
                {
                    listItem.transform.Find("Player").GetComponent<Text>().text = playerName;
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
            //set Loby IP
            GameObject.Find("Canvas/LobbyIP").GetComponent<Text>().text = serverPacket.lobbyIP;
            
            // for each player:
            // initiialize prefab with data
            Debug.Log("Client recieved new package: " + PacketSerializer.objectToJsonString(serverPacket));

            myID = serverPacket.myPlayerID;
            foreach (JArray item in serverPacket.lobbyContent)
            {
                try
                {
                    int currentPlayerID = item[0].ToObject<int>();
                    string playerName = item[1].ToObject<string>();
                    PLAYERCOLOR playerColor = item[2].ToObject<PLAYERCOLOR>();

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
            var gameObject = GameObject.Find(serverPacket.currentPlayerID.ToString());
            gameObject.transform.Find("IsReady").GetComponent<Toggle>().isOn = serverPacket.isReady;
        }

        public void handleGameStartInitialize(Packet serverPacket)
        {
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("2_GameScene");
            gameBoard = serverPacket.gameBoard;
            Debug.Log("Client: Sie haben ein Spielbrett erhalten :)");
        }

        public void handleObjectPlacement(Packet serverPacket)
        {
            Debug.Log("Place building " + serverPacket.buildType + " on " + serverPacket.buildID);
            BUYABLES buildType = (BUYABLES) serverPacket.buildType;
            int buildId = serverPacket.buildID;
            PLAYERCOLOR buildColor = serverPacket.buildColor;
            Debug.Log("client recieved color: " + buildColor);

            boardGenerator.placeBuilding(buildType, buildId, buildColor);

            // Render the new Object
            // Update Resources displayed for own player if you are the one who placed it
            // Update the currentPlayers amount of cards
            //throw new System.NotImplementedException();
        }

        public void handleNextPlayer(Packet serverPacket)
        {   
            throw new System.NotImplementedException();
        }

        public void handleVictory(Packet serverPacket)
        {
            // Show victorious Player
            // Load the post game Scene or Lobby so a new game can be started
            throw new System.NotImplementedException();
        }

        public void handleClientDisconnect(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleRejection(Packet serverPacket)
        {
            Debug.Log("Place building rejected");
            //throw new System.NotImplementedException();
        }

        public void handleAccpetBeginRound(Packet serverPacket)
        {
            Debug.Log("New Round initiated");
            // Show new currentPlayer
            int cache = currentPlayer;
            if (currentPlayer == representativePlayers.Count - 1)
            {
                currentPlayer = 0;
            }
            else
            {
                currentPlayer++;
            }

            playerRepresentation.showNextPlayer(cache,currentPlayer);
            // Render dice rolling
            GameObject.FindGameObjectWithTag("diceHolder").GetComponent<RenderRollDices>().renderRollDices(serverPacket.diceResult);
            // Render gained ressources
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

        public void handleUpdateRPandOP(Packet serverPacket)
        {   
            Debug.Log("handleUpdateRP in Client has been called");
            int i = 0;
            foreach(RepresentativePlayer rp in representativePlayers)
            {
                rp.updateNumbers(serverPacket.updateRP[i]);
                playerRepresentation.updateUiPR(i,rp);
                i++;   
            }
            ownClientPlayer.updateOP(serverPacket.updateOP,serverPacket.updateResourcesOnOP);
            ownPlayerRepresentation.updaetOwnPlayerUI(ownClientPlayer);
        }
        
    }
}