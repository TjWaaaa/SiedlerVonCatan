using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Networking.Package;
using UnityEngine;
using Random = System.Random;
using Enums;

namespace Networking.ServerSide
{
    public class Server
    {
        private static Socket serverSocket;
        private static Dictionary<int, Socket> socketPlayerData;  //serves to store all sockets with playerID
        // private static readonly List<Socket> clientSockets = new List<Socket>(); //serves to store all sockets
        private const int BUFFER_SIZE = 64000;
        private const int PORT = 50042; //freely selectable
        private static byte[] buffer;
        public static IPAddress serverIP { get; private set; }
        
        private static Stack<Color> playerColors = new Stack<Color>();
        private static ServerGameLogic serverGameLogic = new ServerGameLogic();


        /// <summary>
        /// Starts the server to host a game.
        /// </summary>
        public static bool setupServer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketPlayerData = new Dictionary<int, Socket>();
            buffer = new byte[BUFFER_SIZE];
            
            bool isRunning = false;
            
            playerColors.Push(Color.red);
            playerColors.Push(Color.green);
            playerColors.Push(Color.blue);
            playerColors.Push(Color.yellow);
            
            //Console.Title = "Game Server";
            Debug.Log("Server: Setting up Server...");
            try
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT)); //Bind endpoint with ip address and port to socket
                serverIP = getServerEndpoint().Address;
                serverSocket.Listen(4); //maximum pending connection attempts at one time
                serverSocket.BeginAccept(AcceptCallback, null); //begins waiting for client connection attempts
                Debug.Log("Server: Server setup complete, let's go!");
                isRunning = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogWarning("Closing all Sockets");
                
                closeAllSockets();
            }
            
            //todo: Close server after the end of the game.
            return isRunning;
        }
        
        
        /// <summary>
        /// Callback method is called in case of an incoming connection attenpt.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void AcceptCallback(IAsyncResult AR) {
            
            // necessary to log errors that occur in the side thread
            try
            {
                Socket clientSocket;
            
                try
                {
                    clientSocket = serverSocket.EndAccept(AR); //accepts clients connection attempt, returns client socket
                }
                catch (ObjectDisposedException e) 
                {
                    Debug.LogError("Server: ObjectDisposedException. Client disconnected?" + e.Message);
                    return;
                }
                
                // generate a new random client ID
                int newClientID;
                Random random = new Random();
                bool validClientID = false;
                do
                {
                    newClientID = random.Next(100);
                    validClientID = !socketPlayerData.ContainsKey(newClientID);  
                } while (!validClientID);

                //todo: tell server logic the clients ID
                serverGameLogic.generatePlayer(newClientID);
                socketPlayerData.Add(newClientID, clientSocket); //save client to socket list
                Debug.Log($"Server: client (id: {newClientID}) stored in dictionary");
                
                
                clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, new object[] {clientSocket, newClientID}); // open "chanel" to recieve data from the connected socket
                Debug.Log($"Server: Client {clientSocket.RemoteEndPoint} connected, waiting for request...");

                //RepresentJoinigClients.representNewPlayer();
                serverSocket.BeginAccept(AcceptCallback, null); //begins waiting for client connection attempts
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        
        /// <summary>
        /// Callback method is called when the server has finished sending data.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void sendCallback(IAsyncResult AR)
        {
            Debug.Log("IAsyncResult: " + AR);
            serverSocket.EndSend(AR);
        }

        
        /// <summary>
        /// Callback method is called in case of data being sent to the server.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void ReceiveCallback(IAsyncResult AR)
        {
            object[] socketIDArray = (object[]) AR.AsyncState;
            Socket currentClientSocket = (Socket) socketIDArray[0];
            int currentClientID = (int) socketIDArray[1];

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
            
            // map socket to id and send id to method call
            
            
            delegateIncomingDataToMethods(incomingPacket, currentClientID);

            currentClientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback,
                socketIDArray); // Begins waiting for incoming traffic again. Overwrites buffer.
        }


        /// <summary>
        /// Send data to one player with the specified ID.
        /// </summary>
        /// <param name="playerID">Player to send data to</param>
        /// <param name="dataString">Data to send</param>
        public static void sendDataToOne(int playerID, Packet data)
        {
            data.myPlayerID = playerID;
            string dataString = PacketSerializer.objectToJsonString(data);
            byte[] dataToSend = Encoding.ASCII.GetBytes(dataString);
            socketPlayerData[playerID].BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, sendCallback, serverSocket);
            
        }
        
        
        /// <summary>
        /// Send data to all players but the one with the specified ID.
        /// </summary>
        /// <param name="playerID">Player who is excluded from sending data</param>
        /// <param name="dataString">Data to send</param>
        public static void sendDataToAllButOne(int playerID, Packet data)
        {
            foreach (int id in socketPlayerData.Keys)
            {
                if(id != playerID)
                {
                    data.myPlayerID = playerID;
                    string dataString = PacketSerializer.objectToJsonString(data);
                    byte[] dataToSend = Encoding.ASCII.GetBytes(dataString);
                    socketPlayerData[id].BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, sendCallback, serverSocket);
                }
            }
        }
        
        
        /// <summary>
        /// Send data to all players.
        /// </summary>
        /// <param name="dataString">Data to send</param>
        public static void sendDataToAll(Packet data)
        {
            foreach (int id in socketPlayerData.Keys)
            {
                data.myPlayerID = id;
                string dataString = PacketSerializer.objectToJsonString(data);
                byte[] dataToSend = Encoding.ASCII.GetBytes(dataString);
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
        private static void closeAllSockets()
        {
            foreach (Socket socket in socketPlayerData.Values)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Server: Socket could not be shut down. Closing..." + e);
                }
                finally
                {
                    socket.Close();
                }
            }

            //todo: maybe serversocket.BeginDisconnect() ?
            serverSocket.Close();
        }

        
        /// <summary>
        /// map the incoming data by its type to a handle method
        /// </summary>
        /// <param name="incomingData">received data from client</param>
        private static void delegateIncomingDataToMethods(Packet incomingData, int currentClientID)
        {
            switch (incomingData.type)
            {
                case (int) COMMUNICATION_METHODS.HANDLE_REQUEST_JOIN_LOBBY:
                    serverGameLogic.handleRequestJoinLobby(incomingData, currentClientID);
                    break;
                
                case (int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY:
                    serverGameLogic.handleRequestPlayerReady(incomingData, currentClientID);
                    break;
                
                case (int) COMMUNICATION_METHODS.HANDLE_BEGIN_ROUND:
                    serverGameLogic.handleBeginRound(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.HANDLE_TRADE_BANK:
                    serverGameLogic.handleTradeBank(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.HANDLE_BUILD:
                    serverGameLogic.handleBuild(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.HANDLE_BUY_DEVELOPMENT:
                    serverGameLogic.handleBuyDevelopement(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.HANDLE_PLAY_DEVELOPMENT:
                    serverGameLogic.handlePlayDevelopement(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.HANDLE_END_TURN:
                    serverGameLogic.handleEndTurn(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.HANDLE_CLIENT_DISCONNECT_SERVER_CALL:
                    //handleClientDisconnectServerCall(incomingData);
                    break;
                
                default:
                    Debug.LogWarning($"there was no target method send, invalid data packet. Packet Type: {incomingData.type}");
                    // TODO: trow exception!!!
                    break;
            }
        }
    }
}