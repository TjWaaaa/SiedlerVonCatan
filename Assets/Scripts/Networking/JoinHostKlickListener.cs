using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Networking;
using UnityEngine;
using UnityEngine.UI;

public class JoinHostKlickListener : MonoBehaviour
{
    
    //TODO: mover methods private and in the start method use gameObject.transform / gameobject.name .... to get the current button and add the listener

    public void joinListener()
    {
        string playerName = GameObject.Find("Canvas/InputField_playerName").GetComponent<InputField>().text;
        string hostIp = GameObject.Find("Canvas/InputField_hostIP").GetComponent<InputField>().text == "" ? "127.0.0.1" : GameObject.Find("Canvas/InputField_hostIP").GetComponent<InputField>().text;

        Debug.Log("joining game...");
        Debug.Log("playerName: " + playerName);
        Debug.Log("hostIp: " + hostIp);

        
        Client.initClient(hostIp);
    }
    
    
    public void hostListener()
    {
        //TODO: something causes unity to break after server is setup!!!
        // bool isRunning = 
        Server.setupServer();
        Debug.Log("hosting game...");
        
        //TODO: pass serverIP to joinListener
        // if (isRunning)
        // {
        IPEndPoint serverIPEndpoint = Server.getServerEndpoint();
        // Client.initClient(serverIPEndpoint.Address.ToString()); something causes unity to break after server is setup!!!
        Debug.Log("Host: hostIp: " + serverIPEndpoint.Address);
        // }
    }
}
