using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Enums;
using Networking.ClientSide;
using Networking.Communication;
using Networking.ServerSide;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinHostKlickListener : MonoBehaviour
{
    //TODO: mover methods private and in the start method use gameObject.transform / gameobject.name .... to get the current button and add the listener

    /// <summary>
    /// Method is called from a onClick event. Join the game with the IP address entered by the user.
    /// </summary>
    public void joinListener()
    {
        string playerName = GameObject.Find("Canvas/joinPanel/join_PlayerName").GetComponent<InputField>().text;
        string hostIp = GameObject.Find("Canvas/joinPanel/join_IP_Address").GetComponent<InputField>().text == ""
            ? "127.0.0.1"
            : GameObject.Find("Canvas/joinPanel/join_IP_Address").GetComponent<InputField>().text;

        Debug.Log("SERVER: joining game...");
        Debug.Log("SERVER: playerName: " + playerName);
        Debug.Log("SERVER: hostIp: " + hostIp);

        
        var clientReceive = new GameObject();
        clientReceive.name = "clientReceive";
        clientReceive = Instantiate(clientReceive);
        clientReceive.AddComponent<ClientReceive>();
        clientReceive.AddComponent<BoardGenerator>();
            
        bool initComplete = Client.initClient(hostIp, clientReceive.GetComponent<ClientReceive>());
        // bool initComplete = Client.initClient(hostIp);

        if (initComplete)
        {
            ClientRequest clientRequest = new ClientRequest();
            clientRequest.requestJoinLobby(playerName);

            SceneManager.LoadScene("Scenes/1_LobbyScene");
        }
        else
        {
            Debug.Log("SERVER: Client init failed!");
        }
    }


    /// <summary>
    /// Method is called from a onClick event. Starts the game server as well as a client.
    /// </summary>
    /// <exception cref="Exception">
    /// If the servers IPEndpoint is null the client fails to connect.
    /// To prevent this an Exception is thrown.</exception>
    public void hostListener()
    {
        bool isRunning = Server.setupServer(new ServerReceive(new ServerRequest())); //host server
        string playerName = GameObject.Find("Canvas/hostPanel/host_PlayerName").GetComponent<InputField>().text;
        
        // Packet gameInformation = new Packet();
        // gameInformation.playerName = "Simon";
        
        Debug.Log("SERVER: hosting game...");

        if (isRunning)
        {
            IPEndPoint serverIPEndpoint = Server.getServerEndpoint();

            if (serverIPEndpoint == null)
            {
                throw new Exception("serverIPEndpoint is null!");
            }

            var clientReceive = new GameObject();
            clientReceive.name = "clientReceive";
            clientReceive = Instantiate(clientReceive);
            clientReceive.AddComponent<ClientReceive>();
            clientReceive.AddComponent<BoardGenerator>();

            bool initComplete = Client.initClient(serverIPEndpoint.Address.ToString(), clientReceive.GetComponent<ClientReceive>()); //join hosted game as client

            if (initComplete)
            {
                ClientRequest clientRequest = new ClientRequest();
                clientRequest.requestJoinLobby(playerName);
                // Client.sendRequest(PacketSerializer.objectToJsonString(gameInformation)); //send playerName to host
            }

            Debug.Log("SERVER: Client: hostIp: " + serverIPEndpoint.Address);
            SceneManager.LoadScene("Scenes/1_LobbyScene");
        }
    }
}