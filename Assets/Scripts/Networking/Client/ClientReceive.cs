using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Newtonsoft.Json.Linq;
using Networking.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Networking.Package;
using Networking.ServerSide;
using Player;
using TMPro;
using Trade;
using UI;

namespace Networking.ClientSide
{
    public class ClientReceive : MonoBehaviour, INetworkableClient
    {
        public int myID { get; private set; }

        private bool runFixedUpdate = true;

        private PrefabFactory prefabFactory;

        // UI
        private Scene currentScene;
        private GameObject diceHolder;
        private GameObject scrollViewContent;
        private PlayerRepresentation playerRepresentation = new PlayerRepresentation();
        private OwnPlayerRepresentation ownPlayerRepresentation = new OwnPlayerRepresentation();
        private DevCardsMenu _devCardsMenu;
        private TradeMenu _tradeMenu;

        // Player
        private List<RepresentativePlayer> representativePlayers = new List<RepresentativePlayer>();
        private OwnClientPlayer ownClientPlayer;
        private int playerNumber = 1;
        private int currentPlayer = 0;

        // Board
        private Hexagon[][] gameBoard;
        private BoardGenerator boardGenerator;

        private Text rejectionText;



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


        /// <summary>
        /// Load everything, when the GameScene is started. This happens only once
        /// </summary>
        public void FixedUpdate()
        {
            if (runFixedUpdate)
            {
                currentScene = SceneManager.GetActiveScene();

                if (currentScene.name == "2_GameScene")
                {
                    runFixedUpdate = false;
                    boardGenerator.instantiateGameBoard(gameBoard);
                    playerRepresentation.represent(representativePlayers.ToArray());
                    ownPlayerRepresentation.represent(ownClientPlayer);
                    playerRepresentation.showNextPlayer(0, currentPlayer);
                    _devCardsMenu = GameObject.Find("_UI").GetComponent<DevCardsMenu>();
                    _tradeMenu = GameObject.Find("_UI").GetComponent<TradeMenu>();
                    rejectionText = GameObject.Find("RejectionMessageText").GetComponent<Text>();
                }
            }
        }


        public void OnApplicationQuit()
        {
#if UNITY_EDITOR
            quitGame();
            UnityEditor.EditorApplication.isPlaying = false;
#else
                quitGame();
                Application.Quit();
#endif
        }


        /// <summary>
        /// Shuts down both client and server.
        /// </summary>
        public static void quitGame()
        {
            Client.shutDownClient();
            Server.shutDownServer();

        }

        /// <summary>
        /// Converts PLAYERCOLOR to a Color object
        /// </summary>
        /// <param name="playerColor"></param>
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
        /// Create RepresentativePlayers and OwnClientPlayer
        /// </summary>
        /// <param name="playerName">Name of the player</param>
        /// <param name="playerColor">Color of the player</param>
        /// <param name="currentPlayerID">ID of the player</param>
        private void representNewPlayer(int currentPlayerID, string playerName, PLAYERCOLOR playerColor)
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

                    if (currentPlayerID != myID) // Disable all toggle components which don't belong to the local client and create representativePlayers
                    {
                        listItem.transform.Find("IsReady").GetComponent<Toggle>().enabled = false;
                        listItem.transform.Find("IsReady").GetComponent<PlayerReady>().enabled = false;
                        representativePlayers.Add(new RepresentativePlayer(currentPlayerID, playerName, decodeColor(playerColor)));
                    }
                    else
                    {
                        representativePlayers.Add(new RepresentativePlayer(currentPlayerID, playerName + " (you)", decodeColor(playerColor)));
                        ownClientPlayer = new OwnClientPlayer(currentPlayerID);
                        Debug.Log("CLIENT: Created OwnClientPlayer with ID" + currentPlayerID);
                    }

                    listItem.name = currentPlayerID.ToString();
                    Debug.Log("CLIENT: " + playerName + " created. Player Number " + representativePlayers.Count);
                }

                else // List entry does already exist --> update name and color 
                {
                    listItem.transform.Find("Player").GetComponent<Text>().text = playerName;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("CLIENT: " + e);
            }
        }

        /// <summary>
        /// Loads the endscene. Performs cleanup operation on game scene in background.
        /// </summary>
        /// <param name="winnerName"> name of the winner</param>
        /// <returns>null</returns>
        private IEnumerator loadEndScene(string winnerName)
        {

            AsyncOperation promise = SceneManager.LoadSceneAsync("3_EndScene");

            while (!promise.isDone)
            {
                yield return null;
            }

            string win = $"Congratulations!\nPlayer {winnerName} won the game!";
            GameObject textObj = GameObject.Find("Canvas/victoryPanel/Congrats");
            var comp = textObj.GetComponent<TMP_Text>();
            comp.text = win;
        }

        //---------------------------------------------- Interface INetworkableClient implementation ----------------------------------------------

        public void handleClientJoined(Packet serverPacket)
        {
            // Set Lobby IP
            GameObject.Find("Canvas/LobbyIP").GetComponent<Text>().text = serverPacket.lobbyIP;

            // for each player:
            // initialize prefab with data
            Debug.Log("CLIENT: Client recieved new package: " + PacketSerializer.objectToJsonString(serverPacket));

            myID = serverPacket.myPlayerID;
            foreach (JArray item in serverPacket.lobbyContent)
            {
                try
                {
                    int currentPlayerID = item[0].ToObject<int>();
                    string playerName = item[1].ToObject<string>();
                    PLAYERCOLOR playerColor = item[2].ToObject<PLAYERCOLOR>();

                    Debug.Log($"CLIENT: Client joined: Name: {playerName}, Color: {playerColor}, ID: {currentPlayerID}");
                    representNewPlayer(currentPlayerID, playerName, playerColor);
                }
                catch (Exception e)
                {
                    Debug.LogError("CLIENT: " + e.Message);
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
            // SceneManager.SetActiveScene(SceneManager.GetSceneByName("2_GameScene"));
            //
            // // Clean up lobby
            // var parent = GameObject.Find("Scroll View/Viewport/Content").transform;
            // foreach (Transform child in parent)
            // {
            //     Destroy(child.gameObject);
            // }
            //
            // SceneManager.UnloadSceneAsync("1_LobbyScene");

            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("2_GameScene");
            gameBoard = serverPacket.gameBoard;
            Debug.Log("CLIENT: Sie haben ein Spielbrett erhalten :)");
        }

        public void handleObjectPlacement(Packet serverPacket)
        {
            Debug.Log("CLIENT: Place building " + serverPacket.buildType + " on " + serverPacket.buildID);
            BUYABLES buildType = (BUYABLES)serverPacket.buildType;
            int buildId = serverPacket.buildID.GetValueOrDefault();
            PLAYERCOLOR buildColor = serverPacket.buildColor;
            Debug.Log("CLIENT: client recieved color: " + buildColor);

            boardGenerator.placeBuilding(buildType, buildId, buildColor);
            //InputController.stopBuildMode(); //TODO WHY HIER?

            // Render the new Object
            // Update Resources displayed for own player if you are the one who placed it
            // Update the currentPlayers amount of cards
            //throw new System.NotImplementedException();
        }

        public void handleNextPlayer(Packet serverPacket)
        {
            Debug.Log("CLIENT: Current Player: " + serverPacket.previousPlayerID);
            Debug.LogWarning($"CLIENT: It was {serverPacket.previousPlayerID}'s turn and now it's {serverPacket.currentPlayerID}'s turn!");
            if (!runFixedUpdate) { playerRepresentation.showNextPlayer(serverPacket.previousPlayerID.GetValueOrDefault(), serverPacket.currentPlayerID.GetValueOrDefault()); }
            currentPlayer = serverPacket.currentPlayerID.GetValueOrDefault();
            Debug.Log($"CLIENT: CurrentPlayer is player {serverPacket.currentPlayerID}");
        }

        public void handleVictory(Packet serverPacket)
        {
            // Show victorious Player
            // Load the post game Scene or Lobby so a new game can be started
            Debug.Log($"CLIENT: Yeay somebody won and it is {serverPacket.playerName} with the color {serverPacket.playerColor}");

            StartCoroutine(loadEndScene(serverPacket.playerName));
        }

        public void handleClientDisconnect(Packet serverPacket)
        {
            Debug.LogError($"CLIENT: Client named {serverPacket.playerName} lost its connection");
            // todo: display in UI
        }

        public void handleRejection(Packet serverPacket)
        {
            string errorMessage = serverPacket.errorMessage;
            Debug.Log("CLIENT: " + errorMessage);
            rejectionText.text = errorMessage;
            StartCoroutine(TimeYield());
            TimeYield();
        }

        public void handleAccpetBeginRound(Packet serverPacket)
        {
            Debug.Log("CLIENT: New Round initiated");
            Debug.Log("CLIENT: Current Player index: " + currentPlayer);
            // Render dice rolling
            GameObject.FindGameObjectWithTag("diceHolder").GetComponent<RenderRollDices>().renderRollDices(serverPacket.diceResult);
            // Render gained ressources
        }

        public void handleAcceptTradeOffer(Packet serverPacket)
        {
            int buttonNumber = serverPacket.buttonNumber.GetValueOrDefault();
            _tradeMenu.markOfferResource(buttonNumber);
        }

        public void handleAcceptBuyDevelopement(Packet serverPacket)
        {
            _devCardsMenu.updateLeftDevCards(serverPacket.leftDevCards.GetValueOrDefault());
            Debug.Log("CLIENT: One Development card was bought. There are " + serverPacket.leftDevCards + " cards left.");
        }

        public void handleAcceptPlayDevelopement(Packet serverPacket)
        {
            Debug.Log($"CLIENT: {serverPacket.playerName} played a devCard: {serverPacket.developmentCard}");
        }

        public void handleUpdateRP(Packet serverPacket)
        {
            int i = 0;
            foreach (RepresentativePlayer rp in representativePlayers)
            {
                rp.updateNumbers(serverPacket.updateRP[i]);
                playerRepresentation.updateUiPR(i, rp);
                i++;
            }
            Debug.Log("CLIENT: update RP");
        }

        public void handleUpdateOP(Packet serverPacket)
        {
            ownClientPlayer.updateOP(serverPacket.updateOP, serverPacket.updateResourcesOnOP, serverPacket.updateDevCardsOnOP);
            ownPlayerRepresentation.updaetOwnPlayerUI(ownClientPlayer);
            _devCardsMenu.showDevCards(ownClientPlayer);
            Debug.Log("CLIENT: update OP");
        }
        private IEnumerator TimeYield()
        {
            Debug.Log("Deleting Error Message in 5 Seconds");
            yield return new WaitForSeconds(5f);
            rejectionText.text = "";
        }
    }
}