using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class Server
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>(); //serves to store all sockets
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 50042; //freely selectable
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];
        
        
        /// <summary>
        /// Starts the server to host a game.
        /// </summary>
        public static bool setupServer()
        {
            Boolean isRunning = false;
            //Console.Title = "Game Server";
            Debug.Log("Server: Setting up Server...");
            try
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT)); //Bind endpoint with ip address and port to socket
                serverSocket.Listen(4); //maximum pending connection attempts at one time
                serverSocket.BeginAccept(AcceptCallback, null); //begins waiting for client connection attempts
                Debug.Log("Server: Server setup complete, let's go!");
                isRunning = true;
            }
            catch (Exception e)
            {
                Debug.Log("failed to start server!!!");
                Debug.Log(e);
                throw;
            }
            
            //todo: Close server after the end of the game.
            //Debug.Log("Press any key to quit.");
            //Console.ReadLine(); // to keep console open
            //while (true)
            //{
                
            //}
            //closeAllSockets();
            return isRunning;
        }
        
        
        /// <summary>
        /// Callback method is called in case of an incoming connection attenpt.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void AcceptCallback(IAsyncResult AR) {
            Socket clientSocket;
            
            try {
                clientSocket = serverSocket.EndAccept(AR); //accepts clients connection attempt, returns client socket
            } catch (ObjectDisposedException) 
            {
                Debug.Log("Server: ObjectDisposedException. Client disconnected?");
                return;
            }
            
            clientSockets.Add(clientSocket); //save client to socket list
            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, clientSocket); // open "chanel" to recieve data from the connected socket
            Debug.Log($"Server: Client {clientSocket.RemoteEndPoint} connected, waiting for request...");
            serverSocket.BeginAccept(AcceptCallback, null); //begins waiting for client connection attempts
        }

        
        /// <summary>
        /// Callback method is called in case of data being sent to the server.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket currentClientSocket = (Socket) AR.AsyncState;
            int recievedByteLengh;

            try
            {
                recievedByteLengh = currentClientSocket.EndReceive(AR);
            }
            catch (SocketException)
            {
                Debug.Log("Server: Client forcefully disconnected");
                currentClientSocket.Close();
                clientSockets.Remove(currentClientSocket);
                //todo: reestablish connection
                return;
            }

            byte[] currentBuffer = new byte[recievedByteLengh];
            Array.Copy(buffer, currentBuffer, recievedByteLengh); //to remove the protruding zeros from buffer
            string incomingDataString = Encoding.ASCII.GetString(currentBuffer);
            Debug.Log("Server: Received Text: " + incomingDataString);
            //todo: handle incomingDataString
            
            if (incomingDataString.ToLower().Equals("get status"))
            {
                string dataString = $"Current status: \n connected clients: {clientSockets.Count} \n current buffer size: {BUFFER_SIZE}";
                byte[] dataToSend = Encoding.ASCII.GetBytes(dataString);
                currentClientSocket.Send(dataToSend);
                Debug.Log("Server: Current status was requested and sent.");
            } else if (incomingDataString.ToLower().Equals("exit")) // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                currentClientSocket.Shutdown(SocketShutdown.Both);
                currentClientSocket.Close();
                clientSockets.Remove(currentClientSocket);
                Debug.Log("Server: Client disconnected");
                return;
            }
            else
            {
                Debug.Log($"Server: Echoing text: {incomingDataString}");
                byte[] dataToSend = Encoding.ASCII.GetBytes(incomingDataString);
                
                foreach (Socket s in clientSockets)
                {
                    s.Send(dataToSend);
                }
            }

            currentClientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback,
                currentClientSocket); // Begins waiting for incoming traffic again. Overwrites buffer.
        }


        public static IPEndPoint getServerEndpoint()
        {
            return (IPEndPoint)serverSocket.LocalEndPoint;
        }



        /// <summary>
        /// needs to be called at the end of the session to close all connected Sockets and the serverSocket
        /// </summary>
        private static void closeAllSockets() {
            foreach (Socket socket in clientSockets) {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            //todo: maybe serversocket.BeginDisconnect() ?
            serverSocket.Close();
        }
    }
}