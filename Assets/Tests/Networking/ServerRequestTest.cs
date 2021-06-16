using System.Collections;
using System.Threading;
using Enums;
using Networking.ClientSide;
using Networking.Communication;
using Networking.ServerSide;
using NUnit.Framework;

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
            ArrayList lobbyContent = new ArrayList(){clientID, "TestPlayer", PLAYERCOLOR.RED};
            string lobbyIP = "127.0.0.1";
        
            // send data
            serverRequest.notifyClientJoined(lobbyContent, lobbyIP);
            Thread.Sleep(50);
        
            string recievedLobbyContent = MockClientReceive.packethandleClientJoined.playerName;
            string recievedLobbyIP = MockClientReceive.packethandleClientJoined.playerName;
            int packetType = MockClientReceive.packethandleClientJoined.type;
        
            Assert.AreEqual(lobbyContent, recievedLobbyContent);
            Assert.AreEqual(lobbyIP, recievedLobbyIP);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_CLIENT_JOINED, packetType);
        }
        
        [Test]
        public void gamestartInitializeTest()
        {
           

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_GAMESTART_INITIALIZE, packetType);
        }
        
        [Test]
        public void distributeResourcesTest()
        {
           

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_GET_RESOURCES, packetType);
        }
        
        [Test]
        public void notifyObjectPlacementTest()
        {
           

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_OBJECT_PLACEMENT, packetType);
        }
        
        [Test]
        public void notifyNextPlayerTest()
        {
           

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_NEXT_PLAYER, packetType);
        }
        
        [Test]
        public void notifyVictoryTest()
        {
        
            
            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_VICTORY, packetType);
        }
        
        [Test]
        public void notifyClientDisconnectTest()
        {
        
            
            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_CLIENT_DISCONNECT, packetType);
        }
        
        // return requested information/resources ------------------------
        
        [Test]
        public void notifyRejectionTest()
        {
            

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_REJECTION, packetType);
        }
        
        [Test]
        public void notifyPlayerReadyTest()
        {
            

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY_NOTIFICATION, packetType);
        }
        
        [Test]
        public void notifyRollDiceTest()
        {
                    

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BEGIN_ROUND, packetType);
        }
        
        [Test]
        public void acceptBuyDevelopementTest()
        {
                    

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BUY_DEVELOPMENT_CARD, packetType);
        }
        
        [Test]
        public void notifyAcceptPlayDevelopementTest()
        {
                    

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_ACCEPT_PLAY_DEVELOPMENT_CARD, packetType);
        }
        
        [Test]
        public void notifyAcceptTradeOfferTest()
        {
                    

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_ACCEPT_TRADE_OFFER, packetType);
        }
        
        [Test]
        public void updateRepPlayersTest()
        {
                    

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_UPDATE_RP, packetType);
        }
        
        [Test]
        public void updateOwnPlayerTest()
        {
                    

            int packetType = MockClientReceive.packethandleClientJoined.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_UPDATE_OP, packetType);
        }


        [OneTimeTearDown]
        public void tearDown()
        {
            Client.shutDownClient();
            Server.shutDownServer();
        }
    }
}
