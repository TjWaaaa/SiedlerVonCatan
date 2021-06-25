using System.Threading;
using Enums;
using Networking.ClientSide;
using Networking.Communication;
using Networking.ServerSide;
using NUnit.Framework;

namespace Tests.Networking
{
    /// <summary>
    /// Test if all Packets are constructed correctly an transferred to the Server
    /// </summary>

    public class ClientRequestTest
    {
        private ClientRequest clientRequest;
    
        [OneTimeSetUp]
        public void setUp()
        {
            Server.setupServer(new MockServerReceive());
            Client.initClient("127.0.0.1", new MockClientReceive());

            clientRequest = new ClientRequest();
        }
    
        [Test]
        public void requestJoinLobbyTest()
        {
            string testPlayerName = "TestPlayer";
        
            // send data
            clientRequest.requestJoinLobby(testPlayerName);
            Thread.Sleep(50);
        
            string receivedPlayerName = MockServerReceive.packetHandleRequestJoinLobby.playerName;
            int packetType = MockServerReceive.packetHandleRequestJoinLobby.type;
        
            Assert.AreEqual(testPlayerName, receivedPlayerName);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_REQUEST_JOIN_LOBBY, packetType);
        }
    
        [Test]
        public void requestPlayerReadyTest()
        {
            clientRequest.requestPlayerReady(true);
            Thread.Sleep(50);

            Assert.True(MockServerReceive.packethandleRequestPlayerReady.isReady);
            int packetType = MockServerReceive.packethandleRequestPlayerReady.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY, packetType);

            // ************************************************************************************
        
            clientRequest.requestPlayerReady(false);
            Thread.Sleep(50);
        
            Assert.False(MockServerReceive.packethandleRequestPlayerReady.isReady);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY, packetType);
        }


        [Test]
        public void requestTradeBankTest()
        {
            var offer = new int[] {0, 0, 0, 0, 0};
            var expect = new int[] {0, 0, 0, 0, 0};

            clientRequest.requestTradeBank(offer, expect);
            Thread.Sleep(50);

            Assert.AreEqual(offer, MockServerReceive.packethandleTradeBank.tradeResourcesOffer);
            Assert.AreEqual(expect, MockServerReceive.packethandleTradeBank.tradeResourcesExpect);
            int packetType = MockServerReceive.packethandleTradeBank.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_TRADE_BANK, packetType);
        
            // ************************************************************************************
        
            offer = new int[] {0, 4, 0, 0, 0};
            expect = new int[] {0, 0, 1, 0, 0};

            clientRequest.requestTradeBank(offer, expect);
            Thread.Sleep(50);
        
            Assert.AreEqual(offer, MockServerReceive.packethandleTradeBank.tradeResourcesOffer);
            Assert.AreEqual(expect, MockServerReceive.packethandleTradeBank.tradeResourcesExpect);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_TRADE_BANK, packetType);
        }


        [Test]
        public void requestTradeOfferTest()
        {
            var ressourceType = RESOURCETYPE.SHEEP;
            var buttonNumber = 1;

            clientRequest.requestTradeOffer(ressourceType, buttonNumber);
            Thread.Sleep(50);

            int packetType = MockServerReceive.packethandleTradeOffer.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_TRADE_OFFER, packetType);
            Assert.AreEqual(ressourceType, (RESOURCETYPE) MockServerReceive.packethandleTradeOffer.resourceType);
            Assert.AreEqual(buttonNumber, MockServerReceive.packethandleTradeOffer.buttonNumber);

            // ************************************************************************************

            ressourceType = RESOURCETYPE.ORE;
            buttonNumber = 55;

            clientRequest.requestTradeOffer(ressourceType, buttonNumber);
            Thread.Sleep(50);
        
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_TRADE_OFFER, packetType);
            Assert.AreEqual(ressourceType, (RESOURCETYPE) MockServerReceive.packethandleTradeOffer.resourceType);
            Assert.AreEqual(buttonNumber, MockServerReceive.packethandleTradeOffer.buttonNumber);
        }


        [Test]
        public void requestBuildTest()
        {
            int buildID = 1;
            BUYABLES buildType = BUYABLES.CITY;

            clientRequest.requestBuild(buildType, buildID);
            Thread.Sleep(50);

            Assert.AreEqual(buildID, MockServerReceive.packethandleBuild.buildID);
            Assert.AreEqual(buildType, (BUYABLES) MockServerReceive.packethandleBuild.buildType);
            int packetType = MockServerReceive.packethandleBuild.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_BUILD, packetType);
        
            // ************************************************************************************
        
            buildID = 5;
            buildType = BUYABLES.ROAD;
        
            clientRequest.requestBuild(buildType, buildID);
            Thread.Sleep(50);
        
            Assert.AreEqual(buildID, MockServerReceive.packethandleBuild.buildID);
            Assert.AreEqual(buildType, (BUYABLES) MockServerReceive.packethandleBuild.buildType);
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_BUILD, packetType);
        }


        [Test]
        public void requestBuyDevelopementTest()
        {
            clientRequest.requestBuyDevelopement();
            Thread.Sleep(50);
        
            int packetType = MockServerReceive.packethandleBuyDevelopement.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_BUY_DEVELOPMENT, packetType);
        }


        [Test]
        public void requestPlayDevelopementTest()
        {
            DEVELOPMENT_TYPE developmentCard = DEVELOPMENT_TYPE.VICTORY_POINT;

            clientRequest.requestPlayDevelopement(developmentCard);
            Thread.Sleep(50);

            int packetType = MockServerReceive.packethandlePlayDevelopement.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_PLAY_DEVELOPMENT, packetType);
            Assert.AreEqual(developmentCard, MockServerReceive.packethandlePlayDevelopement.developmentCard);
        }


        [Test]
        public void requestEndTurnTest()
        {
            clientRequest.requestEndTurn();
            Thread.Sleep(50);
        
            int packetType = MockServerReceive.packethandleEndTurn.type;
            Assert.AreEqual((int) COMMUNICATION_METHODS.HANDLE_END_TURN, packetType);
        }


        [OneTimeTearDown]
        public void tearDown()
        {
            Client.shutDownClient();
            Server.shutDownServer();
        }
    }
}
