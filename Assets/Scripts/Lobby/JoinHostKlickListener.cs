using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Enums;
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
        string playerName = GameObject.Find("Canvas/InputField_playerName").GetComponent<InputField>().text;
        string hostIp = GameObject.Find("Canvas/InputField_hostIP").GetComponent<InputField>().text == ""
            ? "127.0.0.1"
            : GameObject.Find("Canvas/InputField_hostIP").GetComponent<InputField>().text;

        Debug.Log("joining game...");
        Debug.Log("playerName: " + playerName);
        Debug.Log("hostIp: " + hostIp);

        bool initComplete = Client.initClient(hostIp);

        if (initComplete)
        {
            ClientRequest clientRequest = new ClientRequest();
            clientRequest.requestJoinLobby(playerName);

            SceneManager.LoadScene("Scenes/Lobby");
        }
        else
        {
            Debug.Log("Client init failed!");
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
        bool isRunning = Server.setupServer(); //host server
        string playerName = GameObject.Find("Canvas/InputField_playerName").GetComponent<InputField>().text;
        
        // Packet gameInformation = new Packet();
        // gameInformation.playerName = "Simon";
        
        Debug.Log("hosting game...");

        if (isRunning)
        {
            IPEndPoint serverIPEndpoint = Server.getServerEndpoint();

            if (serverIPEndpoint == null)
            {
                throw new Exception("serverIPEndpoint is null!");
            }


            bool initComplete = Client.initClient(serverIPEndpoint.Address.ToString()); //join hosted game as client

            if (initComplete)
            {
                ClientRequest clientRequest = new ClientRequest();
                clientRequest.requestJoinLobby(playerName);
                // Client.sendRequest(PacketSerializer.objectToJsonString(gameInformation)); //send playerName to host
            }

            Debug.Log("Client: hostIp: " + serverIPEndpoint.Address);
            SceneManager.LoadScene("Scenes/Lobby");
        }
    }
}