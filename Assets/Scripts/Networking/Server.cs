using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Enums;
using UnityEngine;
using Random = System.Random;

namespace Networking
{
    public class Server
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static Dictionary<int, Socket> socketPlayerData = new Dictionary<int, Socket>();  //serves to store all sockets with playerID
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

            // generate a new random client ID
            int newClientID;
            Random random = new Random();
            bool validClientID = false;
            do
            {
                newClientID = random.Next(100);
                validClientID = socketPlayerData.ContainsKey(newClientID);  
            } while (!validClientID);

            //todo: tell server logic the clients ID
            socketPlayerData.Add(newClientID, clientSocket); //save client to socket list
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
                
                // ominous solution to get to the key via the value
                socketPlayerData.Remove(socketPlayerData.FirstOrDefault(x => x.Value == currentClientSocket).Key);
                //todo: reestablish connection
                return;
            }

            byte[] currentBuffer = new byte[recievedByteLengh];
            Array.Copy(buffer, currentBuffer, recievedByteLengh); //to remove the protruding zeros from buffer
            string incomingDataString = Encoding.ASCII.GetString(currentBuffer);
            Packet incomingPacket = PacketSerializer.jsonToObject(incomingDataString);
            Debug.Log($"Server: Received Text (from {currentClientSocket.LocalEndPoint}): " + incomingDataString);
            
            delegateIncomingDataToMethods(incomingPacket);
            //TODO: trigger gui to display new player and add PlayerData to Dictionary

            currentClientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback,
                currentClientSocket); // Begins waiting for incoming traffic again. Overwrites buffer.
        }


        /// <summary>
        /// Send data to one player with the specified ID.
        /// </summary>
        /// <param name="playerID">Player to send data to</param>
        /// <param name="dataString">Data to send</param>
        public static void sendDataToOne(int playerID, string dataString)
        {
            byte[] dataToSend = Encoding.ASCII.GetBytes(dataString);
            socketPlayerData[playerID].BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, sendCallback, serverSocket);
            
        }
        
        
        /// <summary>
        /// Send data to all players but the one with the specified ID.
        /// </summary>
        /// <param name="playerID">Player who is excluded from sending data</param>
        /// <param name="dataString">Data to send</param>
        public static void sendDataToAllButOne(int playerID, string dataString)
        {
            byte[] dataToSend = Encoding.ASCII.GetBytes(dataString);
            
            foreach (int id in socketPlayerData.Keys)
            {
                if(id != playerID)
                {
                    socketPlayerData[id].BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, sendCallback, serverSocket);
                }
            }
        }
        
        
        /// <summary>
        /// Send data to all players.
        /// </summary>
        /// <param name="dataString">Data to send</param>
        public static void sendDataToAll(string dataString)
        {
            byte[] dataToSend = Encoding.ASCII.GetBytes(dataString);
            foreach (int id in socketPlayerData.Keys)
            {
                socketPlayerData[id].BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, sendCallback, serverSocket);
            }
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
            foreach (Socket socket in socketPlayerData.Values) {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            //todo: maybe serversocket.BeginDisconnect() ?
            serverSocket.Close();
        }

        
        /// <summary>
        /// map the incoming data by its type to a handle method
        /// </summary>
        /// <param name="incomingData">received data from client</param>
        private static void delegateIncomingDataToMethods(Packet incomingData)
        {
            switch (incomingData.type)
            {
                case (int) COMMUNICATION_METHODS.handleRequestJoinLobby:
                    //handleRequestJoinLobby(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleBeginRound:
                    //handleBeginRound(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleTradeBank:
                    //handleTradeBank(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleBuild:
                    //handleBuild(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleBuyDevelopement:
                    //handleBuyDevelopement(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handlePlayDevelopement:
                    //handlePlayDevelopement(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleEndTurn:
                    //handleEndTurn(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleClientDisconnectServerCall:
                    //handleClientDisconnectServerCall(incomingData);
                    break;
                
                default:
                    Debug.LogWarning("there was no target method send, invalid data packet");
                    // TODO: trow exception!!!
                    break;
            }
            
            //todo: remove this when communication works!!!
            string data = PacketSerializer.objectToJsonString(incomingData);
            Debug.Log($"Server: Echoing text: {data}");
            byte[] dataToSend = Encoding.ASCII.GetBytes(data);
                
            foreach (Socket socket in socketPlayerData.Values)
            {
                socket.BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, sendCallback, serverSocket);
            }
        }
    }
}