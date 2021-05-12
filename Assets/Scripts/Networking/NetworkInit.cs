// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Threading;
// using UnityEngine;
// using Networking;
//
// public class NetworkInit : MonoBehaviour
// {
//     public string ipAddressServer = "127.0.0.1";
//     // Start is called before the first frame update
//     void Start()
//     {
//         Thread serverThread = new Thread(() =>
//         {
//             Server.setupServer();
//             Debug.Log("Server setup complete");
//         });
//         serverThread.Start();
//         Thread.Sleep(1000);
//
//         Thread clientThread = new Thread(() =>
//         {
//             Client.initClient(ipAddressServer);
//             Debug.Log("Client setup completed");
//         });
//         clientThread.Start();
//     }
// }
