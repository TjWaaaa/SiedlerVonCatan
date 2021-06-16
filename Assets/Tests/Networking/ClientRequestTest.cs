using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Enums;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Networking;
using Networking.ClientSide;
using Networking.Communication;
using Networking.ServerSide;
using Tests.Networking;

/// <summary>
/// Test if all Packets are constructed correctly
/// </summary>
public class ClientRequestTest
{
    private ClientRequest clientRequest;
    
    [OneTimeSetUp]
    public void SetUp()
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
        
        Assert.Equals(testPlayerName, receivedPlayerName);
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_REQUEST_JOIN_LOBBY, packetType);
    }
    
    [Test]
    public void requestPlayerReadyTest()
    {
        clientRequest.requestPlayerReady(true);
        Thread.Sleep(50);
        int packetType = MockServerReceive.packethandleRequestPlayerReady.type;
        
        Assert.True(MockServerReceive.packethandleRequestPlayerReady.isReady);
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY, packetType);
        
        // ************************************************************************************
        
        clientRequest.requestPlayerReady(false);
        Thread.Sleep(50);
        
        Assert.False(MockServerReceive.packethandleRequestPlayerReady.isReady);
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY, packetType);
    }


    [Test]
    public void requestTradeBankTest()
    {
        var offer = new int[] {0, 0, 0, 0, 0};
        var expect = new int[] {0, 0, 0, 0, 0};
        int packetType = MockServerReceive.packethandleTradeBank.type;
        
        clientRequest.requestTradeBank(offer, expect);
        Thread.Sleep(50);
        
        Assert.Equals(offer, MockServerReceive.packethandleTradeBank.tradeResourcesOffer);
        Assert.Equals(expect, MockServerReceive.packethandleTradeBank.tradeResourcesExpect);
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_TRADE_BANK, packetType);
        
        // ************************************************************************************
        
        offer = new int[] {0, 4, 0, 0, 0};
        expect = new int[] {0, 0, 1, 0, 0};

        clientRequest.requestTradeBank(offer, expect);
        Thread.Sleep(50);
        
        Assert.Equals(offer, MockServerReceive.packethandleTradeBank.tradeResourcesOffer);
        Assert.Equals(expect, MockServerReceive.packethandleTradeBank.tradeResourcesExpect);
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_TRADE_BANK, packetType);
    }


    [Test]
    public void requestTradeOfferTest()
    {
        int packetType = MockServerReceive.packethandleTradeOffer.type;
        var packet = MockServerReceive.packethandleTradeOffer;
        
        var ressourceType = RESOURCETYPE.SHEEP;
        var buttonNumber = 1;

        clientRequest.requestTradeOffer(ressourceType, buttonNumber);
        Thread.Sleep(50);
        
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_TRADE_OFFER, packetType);
        Assert.Equals(ressourceType, (RESOURCETYPE)packet.resourceType);
        Assert.Equals(buttonNumber, packet.buttonNumber);

        // ************************************************************************************

        ressourceType = RESOURCETYPE.ORE;
        buttonNumber = 55;

        clientRequest.requestTradeOffer(ressourceType, buttonNumber);
        Thread.Sleep(50);
        
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_TRADE_OFFER, packetType);
        Assert.Equals(ressourceType, (RESOURCETYPE)packet.resourceType);
        Assert.Equals(buttonNumber, packet.buttonNumber);
    }


    [Test]
    public void requestBuildTest()
    {
        int buildID = 1;
        BUYABLES buildType = BUYABLES.CITY;
        int packetType = MockServerReceive.packethandleBuild.type;
        
        clientRequest.requestBuild(buildType, buildID);
        Thread.Sleep(50);
        
        Assert.Equals(buildID, MockServerReceive.packethandleBuild.buildID);
        Assert.Equals(buildType, MockServerReceive.packethandleBuild.buildType);
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_BUILD, packetType);
        
        // ************************************************************************************
        
        buildID = 5;
        buildType = BUYABLES.ROAD;
        
        clientRequest.requestBuild(buildType, buildID);
        Thread.Sleep(50);
        
        Assert.Equals(buildID, MockServerReceive.packethandleBuild.buildID);
        Assert.Equals(buildType, MockServerReceive.packethandleBuild.buildType);
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_BUILD, packetType);
    }


    [Test]
    public void requestBuyDevelopementTest()
    {
        int packetType = MockServerReceive.packethandleBuyDevelopement.type;

        clientRequest.requestBuyDevelopement();
        Thread.Sleep(50);
        
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_BUY_DEVELOPMENT, packetType);
    }


    [Test]
    public void requestPlayDevelopementTest()
    {
        DEVELOPMENT_TYPE developmentCard = DEVELOPMENT_TYPE.VICTORY_POINT;
        int packetType = MockServerReceive.packethandlePlayDevelopement.type;
        
        clientRequest.requestPlayDevelopement(developmentCard);
        Thread.Sleep(50);
        
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_PLAY_DEVELOPMENT, packetType);
        Assert.Equals(developmentCard, MockServerReceive.packethandlePlayDevelopement.developmentCard);
    }


    [Test]
    public void requestEndTurnTest()
    {
        int packetType = MockServerReceive.packethandleEndTurn.type;
        
        clientRequest.requestEndTurn();
        Thread.Sleep(50);
        
        Assert.Equals((int) COMMUNICATION_METHODS.HANDLE_PLAY_DEVELOPMENT, packetType);
    }


    [TearDown]
    public void TearDown()
    {
        Client.shutDownClient();
        Server.shutDownServer();
    }
}
