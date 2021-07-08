using System.Collections;
using System.Collections.Generic;
using Networking.ClientSide;
using Networking.ServerSide;
using NUnit.Framework;
using Tests.Networking;
using UnityEngine;
using UnityEngine.TestTools;

public class ServerClientTest
{
    private bool setUpClient;
    private bool setUpServer;

    [OneTimeSetUp]
    public void SetUp()
    {
        setUpServer = Server.setupServer(new MockServerReceive());
        setUpClient = Client.initClient("127.0.0.1", new MockClientReceive());
    }


    /// <summary>
    /// Test if setting up Server and Client is possible
    /// </summary>
    [Test]
    public void TestServerClientSetup()
    {
        Assert.True(setUpClient);
        Assert.True(setUpServer);
    }


    [TearDown]
    public void TearDown()
    {
        Client.shutDownClient();
        Server.shutDownServer();
    }
}
