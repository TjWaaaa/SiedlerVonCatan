using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Enums;
using Networking.ClientSide;
using Networking.Package;
using Networking.ServerSide;
using NUnit.Framework;
using Tests.Networking;
using Tests.ServerLogic;
using UnityEngine;
using UnityEngine.TestTools;


/// <summary>
/// This calls all ServerReceive methods.
/// After that the corresponding outputs are asserted.
/// </summary>
public class ServerReceiveTest
{
    private ServerReceive serverReceive;
    private readonly int playerID = (int) PLAYERCOLOR.RED;
    private readonly string playerName = "Horst";

    private bool firstPlayerJoined;

    /// <summary>
    /// Prepare serverReceive and Server for testing
    /// </summary>
    [OneTimeSetUp]
    public void setUp()
    {
        serverReceive = new ServerReceive(new MockServerRequest(), true);
        Server.setupServer(new MockServerReceive());
        serverReceive.generatePlayer(playerID);
    }
    
    /// <summary>
    /// Lets a player Join the lobby
    /// </summary>
    [Test]
    public void A_handleRequestJoinLobbyTest()
    {
        Packet packet = new Packet();
        packet.playerName = playerName;
        serverReceive.handleRequestJoinLobby(packet, playerID);
        
        // determine own IP-Address
        var host = Dns.GetHostEntry(Dns.GetHostName());
        string ipAddress = null;
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddress = ip.ToString();
            }
        }

        ArrayList playerInformation = MockServerRequest.notifyClientJoinedPlayerInformation;
        Assert.AreEqual(ipAddress, MockServerRequest.notifyClientJoinedLobbyIP);
        Assert.AreEqual("Horst", ((object[]) playerInformation[0])[1]);
        Assert.AreEqual(PLAYERCOLOR.RED, ((object[]) playerInformation[0])[2]);
        Assert.AreEqual(0, ((object[]) playerInformation[0])[0]);

        firstPlayerJoined = true;
    }
    
    /// <summary>
    /// Lets a second Player join.
    /// Sets both players ready status to true.
    /// </summary>
    [Test]
    public void B_handleRequestPlayerReadyTest()
    {
        if (!firstPlayerJoined) // join first player, if it hasn't happened already.
        {
            Packet _packet = new Packet();
            _packet.playerName = playerName;
            serverReceive.handleRequestJoinLobby(_packet, playerID);
            firstPlayerJoined = true;
        }
        
        // join second player
        serverReceive.generatePlayer((int) PLAYERCOLOR.BLUE);
        Packet _joinPacket = new Packet();
        _joinPacket.playerName = "Guenter";
        serverReceive.handleRequestJoinLobby(_joinPacket, 1);
        
        // Test new players information
        ArrayList playerInformation = MockServerRequest.notifyClientJoinedPlayerInformation;
        Assert.AreEqual(PLAYERCOLOR.BLUE, ((object[]) playerInformation[1])[2]);
        Assert.AreEqual("Guenter", ((object[]) playerInformation[1])[1]);
        Assert.AreEqual(1, ((object[]) playerInformation[1])[0]);

        
        // set first player to isReady == false
        Packet packet = new Packet();
        packet.isReady = false;
        serverReceive.handleRequestPlayerReady(packet, playerID);
        // test received data
        Assert.AreEqual(playerID, MockServerRequest.notifyPlayerReadyCurrentClientID);
        Assert.AreEqual(playerName, MockServerRequest.notifyPlayerReadyPlayerName);
        Assert.AreEqual(false, MockServerRequest.notifyPlayerReadyReadyStatus);
        
        // set first player to isReady == true
        packet.isReady = true;
        serverReceive.handleRequestPlayerReady(packet, playerID);
        // test received data
        Assert.AreEqual(playerID, MockServerRequest.notifyPlayerReadyCurrentClientID);
        Assert.AreEqual(playerName, MockServerRequest.notifyPlayerReadyPlayerName);
        Assert.AreEqual(true, MockServerRequest.notifyPlayerReadyReadyStatus);
        
        // set second player to isReady == true
        serverReceive.handleRequestPlayerReady(packet, 1);
        Assert.AreEqual(1, MockServerRequest.notifyPlayerReadyCurrentClientID);
        Assert.AreEqual("Guenter", MockServerRequest.notifyPlayerReadyPlayerName);
        Assert.AreEqual(true, MockServerRequest.notifyPlayerReadyReadyStatus);

        
        // check of Board is not empty, a random content cant be checked
        Assert.IsNotEmpty(MockServerRequest.gamestartInitializeGameBoard);
        
        
        Assert.AreEqual(0, MockServerRequest.notifyNextPlayerPreviousPlayerIndex);
        Assert.AreEqual(1, MockServerRequest.notifyNextPlayerPlayerIndex);
    }
    
    [Test]
    public void C_handleBeginRoundTest()
    {
        serverReceive.handleBeginRound(new Packet());
        
        // Testing if the function works properly
        Assert.IsNotEmpty(MockServerRequest.notifyRollDiceDiceResult);

        // Testing the results over 1000 attempts
        bool testSuccessful = true;
        for(int i = 0; i<=1000;i++)
        {
            int[] diceResults = serverReceive.rollDices();
            if(diceResults[0] >= 1 && diceResults[0] <= 6 && diceResults[1] >= 1 && diceResults[1] <= 6)
            {
                continue;
            }
            else{testSuccessful = false;break;}
        }
        Assert.IsTrue(testSuccessful);

        
        Dictionary<RESOURCETYPE, int> testResources = new Dictionary<RESOURCETYPE, int>
        {
            {RESOURCETYPE.SHEEP, 10},
            {RESOURCETYPE.ORE, 10},
            {RESOURCETYPE.BRICK, 10},
            {RESOURCETYPE.WOOD, 10},
            {RESOURCETYPE.WHEAT, 10}
        };
        Assert.AreEqual(testResources,MockServerRequest.updateOwnPlayerUpdateResources );
    }
    
    [Test]
    public void D_handleBuildPreGamePhaseTest()
    {
        MockServerRequest.notifyObjectPlacementPlayerColor = PLAYERCOLOR.NONE;
        MockServerRequest.notifyObjectPlacementBuildType = BUYABLES.NONE;
        MockServerRequest.notifyObjectPlacementBuildID = 0;
        MockServerRequest.notifyRejectionErrorMessage = "";
        
        // build first blue village on node with id 0
        Packet packet = new Packet();
        packet.myPlayerID = (int) PLAYERCOLOR.BLUE;
        packet.buildType = (int) BUYABLES.VILLAGE;
        packet.buildID = 0;
        
        serverReceive.handleBuild(packet);
        //Assert.AreEqual("You are not allowed to build!", MockServerRequest.notifyRejectionErrorMessage);
        Assert.AreEqual(PLAYERCOLOR.BLUE, MockServerRequest.notifyObjectPlacementPlayerColor);
        Assert.AreEqual(BUYABLES.VILLAGE, MockServerRequest.notifyObjectPlacementBuildType);
        Assert.AreEqual(0, MockServerRequest.notifyObjectPlacementBuildID);
        MockServerRequest.notifyObjectPlacementPlayerColor = PLAYERCOLOR.NONE;
        MockServerRequest.notifyObjectPlacementBuildType = BUYABLES.NONE;
        MockServerRequest.notifyObjectPlacementBuildID = 0;
        
        
        // reject build city
        packet = new Packet();
        packet.myPlayerID = (int) PLAYERCOLOR.BLUE;
        packet.buildType = (int) BUYABLES.CITY;
        packet.buildID = 0;
        
        serverReceive.handleBuild(packet);
        Assert.AreEqual("Building cant be built", MockServerRequest.notifyRejectionErrorMessage);
        MockServerRequest.notifyRejectionErrorMessage = "";
        
        
        // build blue road on edge with id 0
        packet = new Packet();
        packet.myPlayerID = (int) PLAYERCOLOR.BLUE;
        packet.buildType = (int) BUYABLES.ROAD;
        packet.buildID = 0;
        
        serverReceive.handleBuild(packet);
        Assert.AreEqual(PLAYERCOLOR.BLUE, MockServerRequest.notifyObjectPlacementPlayerColor);
        Assert.AreEqual(BUYABLES.ROAD, MockServerRequest.notifyObjectPlacementBuildType);
        Assert.AreEqual(0, MockServerRequest.notifyObjectPlacementBuildID);
        MockServerRequest.notifyObjectPlacementPlayerColor = PLAYERCOLOR.NONE;
        MockServerRequest.notifyObjectPlacementBuildType = BUYABLES.NONE;
        MockServerRequest.notifyObjectPlacementBuildID = 0;
        
        Assert.AreEqual((int) PLAYERCOLOR.BLUE, MockServerRequest.notifyNextPlayerPreviousPlayerIndex);
        Assert.AreEqual((int) PLAYERCOLOR.RED, MockServerRequest.notifyNextPlayerPlayerIndex);
        MockServerRequest.notifyNextPlayerPreviousPlayerIndex = (int) PLAYERCOLOR.NONE;
        MockServerRequest.notifyNextPlayerPlayerIndex = (int) PLAYERCOLOR.NONE;


        // reject build village with wrong player
        packet = new Packet();
        packet.myPlayerID = (int) PLAYERCOLOR.BLUE;
        packet.buildType = (int) BUYABLES.VILLAGE;
        packet.buildID = 2;
        
        serverReceive.handleBuild(packet);
        Assert.AreEqual("You are not allowed to build!", MockServerRequest.notifyRejectionErrorMessage);
        MockServerRequest.notifyRejectionErrorMessage = "";
        
        
        // build first red village on node with id 2
        packet = new Packet();
        packet.myPlayerID = (int) PLAYERCOLOR.RED;
        packet.buildType = (int) BUYABLES.VILLAGE;
        packet.buildID = 2;
        
        serverReceive.handleBuild(packet);
        Assert.AreEqual(PLAYERCOLOR.RED, MockServerRequest.notifyObjectPlacementPlayerColor);
        Assert.AreEqual(BUYABLES.VILLAGE, MockServerRequest.notifyObjectPlacementBuildType);
        Assert.AreEqual(2, MockServerRequest.notifyObjectPlacementBuildID);
        MockServerRequest.notifyObjectPlacementPlayerColor = PLAYERCOLOR.NONE;
        MockServerRequest.notifyObjectPlacementBuildType = BUYABLES.NONE;
        MockServerRequest.notifyObjectPlacementBuildID = 0;
        
        
        // build red road on edge with id 5
        packet = new Packet();
        packet.myPlayerID = (int) PLAYERCOLOR.RED;
        packet.buildType = (int) BUYABLES.ROAD;
        packet.buildID = 5;
        
        serverReceive.handleBuild(packet);
        Assert.AreEqual(PLAYERCOLOR.RED, MockServerRequest.notifyObjectPlacementPlayerColor);
        Assert.AreEqual(BUYABLES.ROAD, MockServerRequest.notifyObjectPlacementBuildType);
        Assert.AreEqual(5, MockServerRequest.notifyObjectPlacementBuildID);
        MockServerRequest.notifyObjectPlacementPlayerColor = PLAYERCOLOR.NONE;
        MockServerRequest.notifyObjectPlacementBuildType = BUYABLES.NONE;
        MockServerRequest.notifyObjectPlacementBuildID = 0;
        
        
        // build second red village on node with id 7 
        packet = new Packet();
        packet.myPlayerID = (int) PLAYERCOLOR.RED;
        packet.buildType = (int) BUYABLES.VILLAGE;
        packet.buildID = 7;
        
        serverReceive.handleBuild(packet);
        Assert.AreEqual(PLAYERCOLOR.RED, MockServerRequest.notifyObjectPlacementPlayerColor);
        Assert.AreEqual(BUYABLES.VILLAGE, MockServerRequest.notifyObjectPlacementBuildType);
        Assert.AreEqual(7, MockServerRequest.notifyObjectPlacementBuildID);
        MockServerRequest.notifyObjectPlacementPlayerColor = PLAYERCOLOR.NONE;
        MockServerRequest.notifyObjectPlacementBuildType = BUYABLES.NONE;
        MockServerRequest.notifyObjectPlacementBuildID = 0;

        // build second red road on edge with id 11 
        packet.buildType = (int) BUYABLES.ROAD;
        packet.buildID = 11;
        
        serverReceive.handleBuild(packet);
        Assert.AreEqual(PLAYERCOLOR.RED, MockServerRequest.notifyObjectPlacementPlayerColor);
        Assert.AreEqual(BUYABLES.ROAD, MockServerRequest.notifyObjectPlacementBuildType);
        Assert.AreEqual(11, MockServerRequest.notifyObjectPlacementBuildID);
        MockServerRequest.notifyObjectPlacementPlayerColor = PLAYERCOLOR.NONE;
        MockServerRequest.notifyObjectPlacementBuildType = BUYABLES.NONE;
        MockServerRequest.notifyObjectPlacementBuildID = 0;
        
        
        // build second blue village on node with id 9 & road on edge with id 15
        packet = new Packet();
        packet.myPlayerID = (int) PLAYERCOLOR.BLUE;
        packet.buildType = (int) BUYABLES.VILLAGE;
        packet.buildID = 9;
        
        serverReceive.handleBuild(packet);
        Assert.AreEqual(PLAYERCOLOR.BLUE, MockServerRequest.notifyObjectPlacementPlayerColor);
        Assert.AreEqual(BUYABLES.VILLAGE, MockServerRequest.notifyObjectPlacementBuildType);
        Assert.AreEqual(9, MockServerRequest.notifyObjectPlacementBuildID);
        MockServerRequest.notifyObjectPlacementPlayerColor = PLAYERCOLOR.NONE;
        MockServerRequest.notifyObjectPlacementBuildType = BUYABLES.NONE;
        MockServerRequest.notifyObjectPlacementBuildID = 0;
        
        packet.buildType = (int) BUYABLES.ROAD;
        packet.buildID = 15;
        
        serverReceive.handleBuild(packet);
        Assert.AreEqual(PLAYERCOLOR.BLUE, MockServerRequest.notifyObjectPlacementPlayerColor);
        Assert.AreEqual(BUYABLES.ROAD, MockServerRequest.notifyObjectPlacementBuildType);
        Assert.AreEqual(15, MockServerRequest.notifyObjectPlacementBuildID);
        MockServerRequest.notifyObjectPlacementPlayerColor = PLAYERCOLOR.NONE;
        MockServerRequest.notifyObjectPlacementBuildType = BUYABLES.NONE;
        MockServerRequest.notifyObjectPlacementBuildID = 0;
    }

    [Test]
    public void E_handleTradeBankTest()
    {
        // Dictionary to compare
        serverReceive.updateOwnPlayer(0);
        Dictionary<RESOURCETYPE, int> testResources = new Dictionary<RESOURCETYPE, int>
        {
            {RESOURCETYPE.SHEEP, MockServerRequest.updateOwnPlayerUpdateResources[RESOURCETYPE.SHEEP]},
            {RESOURCETYPE.ORE, MockServerRequest.updateOwnPlayerUpdateResources[RESOURCETYPE.ORE]+2},
            {RESOURCETYPE.BRICK, MockServerRequest.updateOwnPlayerUpdateResources[RESOURCETYPE.BRICK]-10},
            {RESOURCETYPE.WOOD, MockServerRequest.updateOwnPlayerUpdateResources[RESOURCETYPE.WOOD]},
            {RESOURCETYPE.WHEAT, MockServerRequest.updateOwnPlayerUpdateResources[RESOURCETYPE.WHEAT]}
        };
        
        // Current player wants to trade
        Packet packet = new Packet();
        packet.myPlayerID = 0;
        packet.tradeResourcesOffer = new[] {0, 0, 10, 0, 0};
        packet.tradeResourcesExpect = new[] {0, 2, 0, 0, 0};
        serverReceive.handleTradeBank(packet);
        Assert.AreEqual(testResources,MockServerRequest.updateOwnPlayerUpdateResources);
        
        // not current player can't trade
        Packet packet2 = new Packet();
        packet2.myPlayerID = 1;
        packet2.tradeResourcesOffer = new[] {4, 0, 0, 0, 0};
        packet2.tradeResourcesExpect = new[] {0, 1, 0, 0, 0};
        serverReceive.handleTradeBank(packet2);
        Assert.AreEqual("You are not allowed to trade with bank!",MockServerRequest.notifyRejectionErrorMessage);
    }

    [Test]
    public void F_handleTradeOfferTest()
    {
        // Current player want's to trade something he can trade
        Packet packet = new Packet();
        packet.myPlayerID = 0;
        packet.resourceType = (int) RESOURCETYPE.WOOD;
        packet.buttonNumber = 3;
        serverReceive.handleTradeOffer(packet);
        Assert.AreEqual(3, MockServerRequest.notifyAcceptTradeOfferButtonNumber);
        
        // Current player want's to trade something he can't trade (even if he get's three brick, it's not enough)
        Packet packet2 = new Packet();
        packet2.myPlayerID = 0;
        packet2.resourceType = (int) RESOURCETYPE.BRICK;
        packet2.buttonNumber = 2;
        serverReceive.handleTradeOffer(packet2);
        Assert.AreEqual("Not enough resources to offer", MockServerRequest.notifyRejectionErrorMessage);


        // not current player can't offer
        Packet packet3 = new Packet();
        packet3.myPlayerID = 1;
        packet3.resourceType = (int) RESOURCETYPE.SHEEP;
        packet3.buttonNumber = 1;
        serverReceive.handleTradeOffer(packet3);
        Assert.AreEqual(1, MockServerRequest.notifyRejectionPlayerID);
        Assert.AreEqual("You are not allowed to offer a trade!", MockServerRequest.notifyRejectionErrorMessage);
    }

    [Test]
    public void G_handlePlayDevelopement()
    {
        Packet packet = new Packet();
        packet.myPlayerID = 0;

        // Give DevCard
        serverReceive.handleBuyDevelopement(packet);
        // Get VictoryPoints
        int previousVP = MockServerRequest.updateRepPlayersUpdateNumbers[0][0];
        // Play DevCard
        serverReceive.handlePlayDevelopement(packet);
        // Get VictoryPoints
        int newVP = MockServerRequest.updateRepPlayersUpdateNumbers[0][0];
        // Testing if victory points have increased
        Assert.IsTrue(newVP > previousVP);
    }
    
    [Test]
    public void H_handleEndTurnTest()
    {
        // end turn with current player
        Packet packet = new Packet();
        packet.myPlayerID = (int) PLAYERCOLOR.RED;
        serverReceive.handleEndTurn(packet);

        Assert.AreEqual((int) PLAYERCOLOR.RED, MockServerRequest.notifyNextPlayerPreviousPlayerIndex);
        Assert.AreEqual((int) PLAYERCOLOR.BLUE, MockServerRequest.notifyNextPlayerPlayerIndex);

        // end turn with wrong player
        packet.myPlayerID = (int) PLAYERCOLOR.RED;
        serverReceive.handleEndTurn(packet);
        
        Assert.AreEqual("You are not allowed to end someone elses turn", MockServerRequest.notifyRejectionErrorMessage);
        
        // end turn with current player
        packet.myPlayerID = (int) PLAYERCOLOR.BLUE;
        serverReceive.handleEndTurn(packet);
        
        Assert.AreEqual((int) PLAYERCOLOR.BLUE, MockServerRequest.notifyNextPlayerPreviousPlayerIndex);
        Assert.AreEqual((int) PLAYERCOLOR.RED, MockServerRequest.notifyNextPlayerPlayerIndex);
        
        // buy things to reach 10 victory points
        packet.myPlayerID = (int) PLAYERCOLOR.RED;
        for (int i = 0; i < 7; i++)
        {
            serverReceive.handleBuyDevelopement(packet);
            serverReceive.handlePlayDevelopement(packet);   
        }

        serverReceive.handleBuyDevelopement(packet);
        Assert.AreEqual("There are no development cards left to buy", MockServerRequest.notifyRejectionErrorMessage);

        Debug.Log(MockServerRequest.updateRepPlayersUpdateNumbers[0][0]);
        Assert.AreEqual(PLAYERCOLOR.RED, MockServerRequest.notifyVictoryPlayerColor);
    }
    
    /// <summary>
    /// Closes Server at the end of test session
    /// </summary>
    [OneTimeTearDown]
    public void tearDown()
    {
        Server.shutDownServer();
    }
}

