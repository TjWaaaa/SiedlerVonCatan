using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Networking;
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
        string hostIp = GameObject.Find("Canvas/InputField_hostIP").GetComponent<InputField>().text == "" ? "127.0.0.1" : GameObject.Find("Canvas/InputField_hostIP").GetComponent<InputField>().text;

        Debug.Log("joining game...");
        Debug.Log("playerName: " + playerName);
        Debug.Log("hostIp: " + hostIp);

        Thread clientThread = new Thread(() =>
        {
            Client.initClient(hostIp);
        });
        clientThread.Start();
        SceneManager.LoadScene("Scenes/Lobby");
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
        Debug.Log("hosting game...");

        if (isRunning)
        {
            IPEndPoint serverIPEndpoint = Server.getServerEndpoint();

            if (serverIPEndpoint == null)
            {
                throw new Exception("serverIPEndpoint is null!");
            }

            Client.initClient(serverIPEndpoint.Address.ToString()); //join hosted game as client
            Debug.Log("Host: hostIp: " + serverIPEndpoint.Address);
            SceneManager.LoadScene("Scenes/Lobby");
        }
    }
}
