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
    private readonly int playerID = 1;
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
    public void handleRequestJoinLobbyTest()
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
        Assert.AreEqual(PLAYERCOLOR.RED, ((object[]) playerInformation[0])[2]);
        Assert.AreEqual("Horst", ((object[]) playerInformation[0])[1]);
        Assert.AreEqual(1, ((object[]) playerInformation[0])[0]);

        firstPlayerJoined = true;
    }
    
    /// <summary>
    /// Lets a second Player join.
    /// Sets both players ready status to true.
    /// </summary>
    [Test]
    public void handleRequestPlayerReadyTest()
    {
        if (!firstPlayerJoined) // join first player, if it hasn't happened already.
        {
            Packet _packet = new Packet();
            _packet.playerName = playerName;
            serverReceive.handleRequestJoinLobby(_packet, playerID);
            firstPlayerJoined = true;
        }
        
        // join second player
        serverReceive.generatePlayer(2);
        Packet _joinPacket = new Packet();
        _joinPacket.playerName = "Guenter";
        serverReceive.handleRequestJoinLobby(_joinPacket, 2);
        
        // Test new players information
        ArrayList playerInformation = MockServerRequest.notifyClientJoinedPlayerInformation;
        Assert.AreEqual(PLAYERCOLOR.BLUE, ((object[]) playerInformation[1])[2]);
        Assert.AreEqual("Guenter", ((object[]) playerInformation[1])[1]);
        Assert.AreEqual(2, ((object[]) playerInformation[1])[0]);

        
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
        serverReceive.handleRequestPlayerReady(packet, 2);
        Assert.AreEqual(2, MockServerRequest.notifyPlayerReadyCurrentClientID);
        Assert.AreEqual("Guenter", MockServerRequest.notifyPlayerReadyPlayerName);
        Assert.AreEqual(true, MockServerRequest.notifyPlayerReadyReadyStatus);

        
        // check of Board is not empty, a random content cant be checked
        Assert.IsNotEmpty(MockServerRequest.gamestartInitializeGameBoard);
        
        
        Assert.AreEqual(0, MockServerRequest.notifyNextPlayerPreviousPlayerIndex);
        Assert.AreEqual(1, MockServerRequest.notifyNextPlayerPlayerIndex);
    }


    [Test]
    public void handleBeginRoundTest()
    {
        serverReceive.handleBeginRound(new Packet());
        
        // Todo: more dice tests and update Players tests
        Assert.IsNotEmpty(MockServerRequest.notifyRollDiceDiceResult);
        
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
    public void handleTradeBankTest()
    {
        Packet packet = new Packet();
        packet.tradeResourcesOffer = new[] {1, 0, 0, 0, 0};
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

