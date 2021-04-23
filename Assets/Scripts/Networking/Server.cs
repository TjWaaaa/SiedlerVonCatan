using System;
using System.Collections.Generic;
using System.Linq;
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
        private static Dictionary<Socket, PlayerData> socketPlayerData = new Dictionary<Socket, PlayerData>();  //serves to store all sockets
        // private static readonly List<Socket> clientSockets = new List<Socket>(); //serves to store all sockets
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 50042; //freely selectable
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];
        
        private static Stack<Color> playerColors = new Stack<Color>();
        
        
        /// <summary>
        /// Starts the server to host a game.
        /// </summary>
        public static bool setupServer()
        {
            Boolean isRunning = false;
            playerColors.Push(Color.red);
            playerColors.Push(Color.green);
            playerColors.Push(Color.blue);
            playerColors.Push(Color.yellow);
            
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
            
            socketPlayerData.Add(clientSocket, null); //save client to socket list
            // clientSockets.Add(clientSocket); 
            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, clientSocket); // open "chanel" to recieve data from the connected socket
            Debug.Log($"Server: Client {clientSocket.RemoteEndPoint} connected, waiting for request...");
            
            RepresentJoinigClients.representNewPlayer();
            
            serverSocket.BeginAccept(AcceptCallback, null); //begins waiting for client connection attempts
        }

        
        /// <summary>
        /// Callback method is called when the server has finished sending data.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void sendCallback(IAsyncResult AR)
        {
            serverSocket.EndSend(AR);
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
                socketPlayerData.Remove(currentClientSocket);
                //todo: reestablish connection
                return;
            }

            byte[] currentBuffer = new byte[recievedByteLengh];
            Array.Copy(buffer, currentBuffer, recievedByteLengh); //to remove the protruding zeros from buffer
            string incomingDataString = Encoding.ASCII.GetString(currentBuffer);
            Debug.Log($"Server: Received Text (from {currentClientSocket.LocalEndPoint}): " + incomingDataString);
            
            //todo: handle incomingDataString
            //TODO: trigger gui to display new player and add PlayerData to Dictionary
            
            if (incomingDataString.ToLower().Equals("get status"))
            {
                string dataString = $"Current status: \n connected clients: {socketPlayerData.Count} \n current buffer size: {BUFFER_SIZE}";
                byte[] dataToSend = Encoding.ASCII.GetBytes(dataString);
                currentClientSocket.BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, sendCallback, serverSocket);
                Debug.Log("Server: Current status was requested and sent.");
            } else if (incomingDataString.ToLower().Equals("exit")) // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                currentClientSocket.Shutdown(SocketShutdown.Both);
                currentClientSocket.Close();
                socketPlayerData.Remove(currentClientSocket);
                Debug.Log("Server: Client disconnected");
                return;
            }
            else
            {
                Debug.Log($"Server: Echoing text: {incomingDataString}");
                byte[] dataToSend = Encoding.ASCII.GetBytes(incomingDataString);
                
                foreach (Socket s in socketPlayerData.Keys)
                {
                    s.BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, sendCallback, serverSocket);
                }
            }

            currentClientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback,
                currentClientSocket); // Begins waiting for incoming traffic again. Overwrites buffer.
        }


        /// <summary>
        /// Gets the current IPEndpoint of the machine.
        /// <returns>IPEndpoint with a IPv4 Address and Port.</returns>
        /// </summary>
        public static IPEndPoint getServerEndpoint()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            IPAddress[] hostIPs = Dns.GetHostAddresses(hostName);
            IPEndPoint endpoint = null;

            // get ipv4 address of host computer. hostIPs contains the IPv4 and IPv6 therefore it needs to be filtered.
            foreach (IPAddress address in hostIPs.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
            {
                Debug.Log(address);
                endpoint = new IPEndPoint(address, PORT);
            }
            
            return endpoint;
        }


        /// <summary>
        /// needs to be called at the end of the session to close all connected Sockets and the serverSocket
        /// </summary>
        private static void closeAllSockets() {
            foreach (Socket socket in socketPlayerData.Keys) {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            //todo: maybe serversocket.BeginDisconnect() ?
            serverSocket.Close();
        }
        
        
        private class PlayerData
        {
            private Socket playerSocket {get;}
            private string playerName {get;}
            private Color color {get;}
            private int id {get;}

            public PlayerData(Socket playerSocket, string playerName, int id)
            {
                this.playerSocket = playerSocket;
                this.playerName = playerName;
                this.color = playerColors.Pop();
                this.id = id;
            }
        }
    }
}