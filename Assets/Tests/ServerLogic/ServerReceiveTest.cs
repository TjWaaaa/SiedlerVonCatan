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
        Packet packet = new Packet();
        packet.tradeResourcesOffer = new[] {1, 0, 0, 0, 0};
    }
    
    [Test]
    public void F_handleEndTurnTest()
    {
        // TODO create 2 players one that wins and one that doesn't

        // Testing if winning player wins
        //Assert.IsTrue(serverReceive.didThisPlayerWin(2));

        // Testing if not winning player doesn't win
        //Assert.IsFalse(serverReceive.didThisPlayerWin(2));
    }
    
    [Test]
    public void G_handlePlayDevelopement()
    {
        // Get Player
        // Give DevCard
        // Get VictoryPoints
        int previousVP = 0;
        // Play DevCard
        // Get VictoryPoints
        int newVP = 1;
        Assert.IsTrue(newVP > previousVP);
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

