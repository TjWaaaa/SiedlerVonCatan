using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Enums;
using Networking;
using Networking.ClientSide;
using Networking.Communication;
using Networking.ServerSide;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Networking
{
    /// <summary>
    /// Test if all packets are constructed correctly.
    /// </summary>
    public class ServerRequestTest
    {
        private ServerRequest serverRequest;
        private int clientID;
        
        [OneTimeSetUp]
        public void setUp()
        {
            Server.setupServer(new MockServerReceive());
            Client.initClient("127.0.0.1", new MockClientReceive());
            Thread.Sleep(50);
            
            clientID = MockServerReceive.playerID;
            serverRequest = new ServerRequest();
        }
        
        
        [Test]
        public void notifyClientJoinedTest()
        {
            string playerName = "TestPlayer";
            PLAYERCOLOR playerColor = PLAYERCOLOR.RED;
            ArrayList lobbyContent = new ArrayList(){clientID, playerName, playerColor};
            string lobbyIP = "127.0.0.1";
        
            // send data
            serverRequest.notifyClientJoined(lobbyContent, lobbyIP);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();
        
            ArrayList recievedLobbyContent = MockClientReceive.packethandleClientJoined.lobbyContent;
            int receivedCurrentPlayerID = (int)(long) recievedLobbyContent[0];
            string receivedPlayerName = (string) recievedLobbyContent[1];
            PLAYERCOLOR receivedPlayerColor = (PLAYERCOLOR)(long) recievedLobbyContent[2];
            
            string recievedLobbyIP = MockClientReceive.packethandleClientJoined.lobbyIP;
            int packetType = MockClientReceive.packethandleClientJoined.type;
        
            Assert.AreEqual(clientID, receivedCurrentPlayerID);
            Assert.AreEqual(playerName, receivedPlayerName);
            Assert.AreEqual(playerColor, receivedPlayerColor);
            Assert.AreEqual(lobbyIP, recievedLobbyIP);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_CLIENT_JOINED, packetType);
        }
        
        [Test]
        public void gamestartInitializeTest()
        {
           Hexagon[][] gameBoard = {};
                       
            // send data
            serverRequest.gamestartInitialize(gameBoard);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();

            Hexagon[][] recievedGameBoard = MockClientReceive.packethandleGameStartInitialize.gameBoard;

            int packetType = MockClientReceive.packethandleGameStartInitialize.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_GAMESTART_INITIALIZE, packetType);
            Assert.AreEqual(gameBoard, recievedGameBoard);
        }
        
        
        [Test]
        public void notifyObjectPlacementTest()
        {
            int buildID = 1;
            BUYABLES buildType = BUYABLES.VILLAGE;
            PLAYERCOLOR playerColor = PLAYERCOLOR.RED;
                       
            // send data
            serverRequest.notifyObjectPlacement(buildType, buildID, playerColor);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();
       
            int? receivedBuildID = MockClientReceive.packethandleObjectPlacement.buildID;
            BUYABLES receivedBuildType = (BUYABLES) MockClientReceive.packethandleObjectPlacement.buildType;
            PLAYERCOLOR receivedPlayerColor = MockClientReceive.packethandleObjectPlacement.buildColor;

            int packetType = MockClientReceive.packethandleObjectPlacement.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_OBJECT_PLACEMENT, packetType);
            Assert.AreEqual(buildID, receivedBuildID);
            Assert.AreEqual(buildType, receivedBuildType);
            Assert.AreEqual(playerColor, receivedPlayerColor);
        }
        
        [Test]
        public void notifyNextPlayerTest()
        {
            int currentPlayer = 0;
            int previousPlayer = -1;

            // send data
            serverRequest.notifyNextPlayer(currentPlayer, previousPlayer);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();

            int packetType = MockClientReceive.packethandleNextPlayer.type;
            int? receivedCurrentPlayer = MockClientReceive.packethandleNextPlayer.currentPlayerID;
            int? receivedPreviousPlayer = MockClientReceive.packethandleNextPlayer.previousPlayerID;
            
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_NEXT_PLAYER, packetType);
            Assert.AreEqual(currentPlayer, receivedCurrentPlayer);
            Assert.AreEqual(previousPlayer, receivedPreviousPlayer);
        }
        
        [Test]
        public void notifyVictoryTest()
        {
            string playerName = "Test";
            PLAYERCOLOR playerColor = PLAYERCOLOR.RED;
        
            // send data
            serverRequest.notifyVictory(playerName, playerColor);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();

            string receivedPlayerName = MockClientReceive.packethandleVictory.playerName;
            PLAYERCOLOR receivedPlayerColor = MockClientReceive.packethandleVictory.playerColor;
            
            int packetType = MockClientReceive.packethandleVictory.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_VICTORY, packetType);
            Assert.AreEqual(playerName, receivedPlayerName);
            Assert.AreEqual(playerColor, receivedPlayerColor);
        }
        
        [Test]
        public void notifyClientDisconnectTest()
        {
            string playerName = "Hallo Herr Hahn, wenn Sie das sehen, koennen Sie uns eine 1 geben :)";
            PLAYERCOLOR playerColor = PLAYERCOLOR.NONE;
            
            // send Data
            serverRequest.notifyClientDisconnect(playerName, playerColor);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();

            int packetType = MockClientReceive.packethandleClientDisconnect.type;
            string receivedPlayerName = MockClientReceive.packethandleClientDisconnect.playerName;
            PLAYERCOLOR receivedPlayerColor = MockClientReceive.packethandleClientDisconnect.playerColor;
            
            Assert.AreEqual(playerName, receivedPlayerName);
            Assert.AreEqual(playerColor, receivedPlayerColor);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_CLIENT_DISCONNECT, packetType);
        }
        
        // return requested information/resources ------------------------
        
        [Test]
        public void notifyRejectionTest()
        {
            string errorMessage = "Well something went wrong";

            // send data
            serverRequest.notifyRejection(clientID, errorMessage);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();

            int receivedPlayerID = MockClientReceive.packethandleRejection.myPlayerID;
            string receivedErrorMessage = MockClientReceive.packethandleRejection.errorMessage;

            int packetType = MockClientReceive.packethandleRejection.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_REJECTION, packetType);
            Assert.AreEqual(clientID, receivedPlayerID);
            Assert.AreEqual(errorMessage, receivedErrorMessage);
        }
        
        [Test]
        public void notifyPlayerReadyTest()
        {
            string playerName = "Hallo Herr Tutor, wenn du das siehst, du bist toll!";
            bool readyStatus = false;
            
            //send data
            serverRequest.notifyPlayerReady(-1, playerName, readyStatus); // -1 because this can not be a client ID and
                                                                                    // on serverside the sendToAllButOne method is called
            Thread.Sleep(50);
            ThreadManager.updateMainThread();

            int packetType = MockClientReceive.packethandlePlayerReadyNotification.type;
            string receivedPlayerName = MockClientReceive.packethandlePlayerReadyNotification.playerName;
            bool receivedReady = MockClientReceive.packethandlePlayerReadyNotification.isReady;
            
            Assert.AreEqual(playerName, receivedPlayerName);
            Assert.AreEqual(readyStatus, receivedReady);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY_NOTIFICATION, packetType);
        }
        
        [Test]
        public void notifyRollDiceTest()
        {
            int[] diceResult = {6,6};

             // send Data
            serverRequest.notifyRollDice(diceResult);
            Thread.Sleep(50);       
            ThreadManager.updateMainThread();

            int[] receivedDiceResult = MockClientReceive.packethandleAccpetBeginRound.diceResult;

            int packetType = MockClientReceive.packethandleAccpetBeginRound.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BEGIN_ROUND, packetType);
            Assert.AreEqual(diceResult, receivedDiceResult);
        }
        
        [Test]
        public void acceptBuyDevelopementTest()
        {
            int leftDevCards = 123456789;
            
            //send data
            serverRequest.acceptBuyDevelopement(leftDevCards);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();

            int packetType = MockClientReceive.packethandleAcceptBuyDevelopement.type;
            int? receivedLeftDevCards = MockClientReceive.packethandleAcceptBuyDevelopement.leftDevCards;
            
            Assert.AreEqual(leftDevCards, receivedLeftDevCards);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BUY_DEVELOPMENT_CARD, packetType);
        }
        
        [Test]
        public void notifyAcceptPlayDevelopementTest()
        {
            DEVELOPMENT_TYPE devCard = DEVELOPMENT_TYPE.VICTORY_POINT;
            string playerName = "Test";        

            // send Data
            serverRequest.notifyAcceptPlayDevelopement(clientID, devCard, playerName);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();
            
            int receivedPlayerID = MockClientReceive.packethandleAcceptPlayDevelopement.myPlayerID;
            DEVELOPMENT_TYPE receivedDevCard = MockClientReceive.packethandleAcceptPlayDevelopement.developmentCard;
            string receivedPlayerName = MockClientReceive.packethandleAcceptPlayDevelopement.playerName;

            int packetType = MockClientReceive.packethandleAcceptPlayDevelopement.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_ACCEPT_PLAY_DEVELOPMENT_CARD, packetType);
            Assert.AreEqual(clientID, receivedPlayerID);
            Assert.AreEqual(devCard, receivedDevCard);
            Assert.AreEqual(playerName, receivedPlayerName);
        }
        
        [Test]
        public void notifyAcceptTradeOfferTest()
        {
            int buttonNumber = 'C' + 'o' + 'o' + 'l';
            
            //send data
            serverRequest.notifyAcceptTradeOffer(clientID, buttonNumber);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();

            int packetType = MockClientReceive.packethandleAcceptTradeOffer.type;
            int? receivedButtonNumber = MockClientReceive.packethandleAcceptTradeOffer.buttonNumber;
            int? receivedPlayerID = MockClientReceive.packethandleAcceptTradeOffer.myPlayerID;
            
            Assert.AreEqual(clientID, receivedPlayerID);
            Assert.AreEqual(buttonNumber, receivedButtonNumber);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_ACCEPT_TRADE_OFFER, packetType);
        }
        
        [Test]
        public void updateRepPlayersTest()
        {
            int[][] updateNumbers = {new int[] {1,2,3}, new int[] {4,5,6}};

            // sendData 
            serverRequest.updateRepPlayers(updateNumbers);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();
            
            int[][] receivedUpdateNumbers = MockClientReceive.packethandleUpdateRP.updateRP;
            int packetType = MockClientReceive.packethandleUpdateRP.type;
            
            Assert.AreEqual(updateNumbers, receivedUpdateNumbers);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_UPDATE_RP, packetType);
        }
        
        [Test]
        public void updateOwnPlayerTest()
        {
            int[] updateOP = {};
            Dictionary<RESOURCETYPE, int> updateResourcesOnOP = new Dictionary<RESOURCETYPE, int>();
            Dictionary<DEVELOPMENT_TYPE, int> updateDevCardsOnOP = new Dictionary<DEVELOPMENT_TYPE, int>();   

            // send Data
            serverRequest.updateOwnPlayer(updateOP, updateResourcesOnOP, updateDevCardsOnOP, clientID);
            Thread.Sleep(50);
            ThreadManager.updateMainThread();

            int[] receivedUpdateOP = MockClientReceive.packethandleUpdateOP.updateOP;
            Dictionary<RESOURCETYPE, int> receivedUpdateResourcesOnOP = MockClientReceive.packethandleUpdateOP.updateResourcesOnOP;
            Dictionary<DEVELOPMENT_TYPE, int> receivedUpdateDevCardsOnOP = MockClientReceive.packethandleUpdateOP.updateDevCardsOnOP;

            int packetType = MockClientReceive.packethandleUpdateOP.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_UPDATE_OP, packetType);
            Assert.AreEqual(updateOP, receivedUpdateOP);
            Assert.AreEqual(updateResourcesOnOP, receivedUpdateResourcesOnOP);
            Assert.AreEqual(updateDevCardsOnOP, receivedUpdateDevCardsOnOP);
        }


        [OneTimeTearDown]
        public void tearDown()
        {
            Client.shutDownClient();
            Server.shutDownServer();
        }
    }
}
