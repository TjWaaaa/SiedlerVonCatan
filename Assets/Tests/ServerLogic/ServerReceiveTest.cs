using System.Collections;
using System.Collections.Generic;
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
/// All Assert statements are in MockServerRequest.
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!IMPORTANT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// All test need to be run in one go! Otherwise some tests fail.
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// </summary>
public class ServerReceiveTest
{
    private ServerReceive serverReceive;
    private readonly int playerID = 1;
    private readonly string playerName = "Horst";

    /// <summary>
    /// Prepare serverReceive and Server for testing
    /// </summary>
    [OneTimeSetUp]
    public void setUp()
    {
        serverReceive = new ServerReceive(new MockServerRequest());
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
    }
    
    /// <summary>
    /// Lets a second Player join.
    /// Sets both players ready status to true.
    /// </summary>
    [Test]
    public void handleRequestPlayerReadyTest()
    {
        // join second player
        serverReceive.generatePlayer(2);
        Packet _joinPacket = new Packet();
        _joinPacket.playerName = "Guenter";
        serverReceive.handleRequestJoinLobby(_joinPacket, 2);

        // set first player to isReady == true
        Packet packet = new Packet();
        packet.isReady = true;
        serverReceive.handleRequestPlayerReady(packet, playerID);
        
        // set second player to isReady == true
        serverReceive.handleRequestPlayerReady(packet, 2);
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

