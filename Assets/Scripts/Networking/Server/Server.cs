using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using Networking.Package;
using UnityEngine;
using Random = System.Random;
using Enums;
using Debug = UnityEngine.Debug;
using System.Threading;
using Networking.Interfaces;

namespace Networking.ServerSide
{
    public class Server
    {
        private static bool isRunning = false;
        private static Socket serverSocket;
        private static Dictionary<int, Socket> socketPlayerData;  //serves to store all sockets with playerID
        // private static readonly List<Socket> clientSockets = new List<Socket>(); //serves to store all sockets
        private const int BUFFER_SIZE = 64000;
        private const int PORT = 50042; //freely selectable
        private static byte[] buffer;
        public static IPAddress serverIP { get; private set; }

        private static System.Timers.Timer keepAliveTimer;
        private const int KEEP_ALIVE_DURATION = 4000;
        private static Dictionary<int, long> timeOfPing;

        private static Stack<Color> playerColors = new Stack<Color>();
        private static INetworkableServer _serverReceive;


        /// <summary>
        /// Starts the server to host a game.
        /// </summary>
        public static bool setupServer(INetworkableServer serverReceive)
        {
            if (isRunning)
            {
                Debug.LogWarning("Server has already been started. Aborting setup...");
                return true;
            }

            _serverReceive = serverReceive;

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketPlayerData = new Dictionary<int, Socket>();
            buffer = new byte[BUFFER_SIZE];

            playerColors.Push(Color.red);
            playerColors.Push(Color.green);
            playerColors.Push(Color.blue);
            playerColors.Push(Color.yellow);

            //Console.Title = "Game Server";
            Debug.Log("SERVER: Setting up Server...");
            try
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT)); //Bind endpoint with ip address and port to socket
                serverIP = getServerEndpoint().Address;
                serverSocket.Listen(4); //maximum pending connection attempts at one time
                serverSocket.BeginAccept(AcceptCallback, null); //begins waiting for client connection attempts
                Debug.Log("SERVER: Server setup complete, let's go!");
                isRunning = true;
            }
            catch (Exception e)
            {
                Debug.LogError("SERVER: " + e);
                Debug.LogWarning("SERVER: Closing all Sockets");

                closeAllSockets();
            }

            //Start keepAliveTimer
            timeOfPing = new Dictionary<int, long>();
            keepAliveTimer = new System.Timers.Timer(KEEP_ALIVE_DURATION);
            keepAliveTimer.Elapsed += sendKeepAlive;
            keepAliveTimer.AutoReset = true;
            keepAliveTimer.Start();

            isRunning = true;
            return isRunning;
        }


        /// <summary>
        /// Callback method is called in case of an incoming connection attenpt.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void AcceptCallback(IAsyncResult AR)
        {

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
                    Debug.Log("SERVER: ObjectDisposedException. Happens always if the server socket is closed.\n" + e.Message);
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

                timeOfPing.Add(newClientID, DateTime.Now.Ticks);
                _serverReceive.generatePlayer(newClientID); //tell server logic the clients ID
                socketPlayerData.Add(newClientID, clientSocket); //save client to socket list
                Debug.Log($"SERVER: client (id: {newClientID}) stored in dictionary");


                clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, new object[] { clientSocket, newClientID }); // open "chanel" to recieve data from the connected socket
                Debug.Log($"SERVER: Client {clientSocket.RemoteEndPoint} connected, waiting for request...");

                //RepresentJoinigClients.representNewPlayer();
                serverSocket.BeginAccept(AcceptCallback, null); //begins waiting for client connection attempts
            }
            catch (Exception e)
            {
                Debug.Log("SERVER: " + e);
                throw;
            }
        }


        /// <summary>
        /// Callback method is called when the server has finished sending data.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void sendCallback(IAsyncResult AR)
        {
            Debug.Log("SERVER: IAsyncResult: " + AR);
            serverSocket.EndSend(AR);
        }


        /// <summary>
        /// Callback method is called in case of data being sent to the server.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void ReceiveCallback(IAsyncResult AR)
        {
            object[] socketIDArray = (object[])AR.AsyncState;
            Socket currentClientSocket = (Socket)socketIDArray[0];
            int currentClientID = (int)socketIDArray[1];

            int recievedByteLengh;

            try
            {
                recievedByteLengh = currentClientSocket.EndReceive(AR);
            }
            catch (SocketException e)
            {
                Debug.LogWarning("SERVER: Client forcefully disconnected\n" + e.Message);
                currentClientSocket.Close();

                // ominous solution to get to the key via the value
                socketPlayerData.Remove(currentClientID);
                //socketPlayerData.Remove(socketPlayerData.FirstOrDefault(x => x.Value == currentClientSocket).Key);
                //todo: reestablish connection
                return;
            }

            // Client disconnects
            if (recievedByteLengh <= 0)
            {
                try
                {
                    currentClientSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Could not shut down client socket {currentClientID}\n" + e.Message);
                }
                finally
                {
                    currentClientSocket.Close();
                    socketPlayerData.Remove(currentClientID);
                }
                return;
            }

            byte[] currentBuffer = new byte[recievedByteLengh];
            Array.Copy(buffer, currentBuffer, recievedByteLengh); //to remove the protruding zeros from buffer
            string incomingDataString = Encoding.ASCII.GetString(currentBuffer);


            if (incomingDataString.Contains("pong") && !incomingDataString.Contains("{")) // Keep alive ping
            {
                timeOfPing[currentClientID] = DateTime.Now.Ticks;
                Debug.Log("SERVER: recieved pong");
            }
            else // Normal packet
            {
                Packet incomingPacket = PacketSerializer.jsonToObject(incomingDataString);
                incomingPacket.myPlayerID = currentClientID;
                Debug.Log($"SERVER: Received Text (from {currentClientSocket.LocalEndPoint}, clientID: {incomingPacket.myPlayerID}): " + incomingDataString);

                // map socket to id and send id to method call

                delegateIncomingDataToMethods(incomingPacket, currentClientID);
            }

            currentClientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback,
                socketIDArray); // Begins waiting for incoming traffic again. Overwrites buffer.
        }

        /// <summary>
        /// Call this method from a Timer regularly
        /// Sends a message "ping" to all connected clients
        /// and tests if an answer was received between the method calls.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="elapsedEventArgs"></param>
        private static void sendKeepAlive(object source, ElapsedEventArgs elapsedEventArgs)
        {
            byte[] message = Encoding.ASCII.GetBytes("ping");
            foreach (int clientID in socketPlayerData.Keys)
            {
                // Test if an answer was received in the meantime
                //todo: this Method is probably not needet due to TCPs own timers.
                if (DateTime.Now.Ticks - timeOfPing[clientID] > (KEEP_ALIVE_DURATION + 1000) * 10000)
                {
                    //todo Reconnect
                    Debug.LogError($"SERVER: Client with ID {clientID} has disconnected!");
                    _serverReceive.handleClientDisconnectServerCall(clientID);
                }

                var socket = socketPlayerData[clientID];
                timeOfPing[clientID] = DateTime.Now.Ticks;
                socket.BeginSend(message, 0, message.Length, SocketFlags.None, sendCallback, socket);
            }
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
            Thread.Sleep(50);
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
                if (id != playerID)
                {
                    data.myPlayerID = playerID;
                    string dataString = PacketSerializer.objectToJsonString(data);
                    byte[] dataToSend = Encoding.ASCII.GetBytes(dataString);
                    socketPlayerData[id].BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, sendCallback, serverSocket);
                }
            }
            Thread.Sleep(50);
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
            Thread.Sleep(50);
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
                Debug.Log("SERVER: " + address);
                endpoint = new IPEndPoint(address, PORT);
            }

            return endpoint;
        }


        /// <summary>
        /// If the server is running all timers are stopped and disposed.
        /// In addition all sockets are closed.
        /// </summary>
        public static void shutDownServer()
        {
            if (isRunning)
            {
                keepAliveTimer.Stop();
                keepAliveTimer.Dispose();
                closeAllSockets();
                isRunning = false;
            }
        }


        /// <summary>
        /// Needs to be called at the end of the session to close all connected Sockets and the serverSocket
        /// </summary>
        private static void closeAllSockets()
        {
            // Close connected client sockets
            foreach (int key in socketPlayerData.Keys)
            {
                Socket socket = socketPlayerData[key];
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Debug.LogError($"SERVER: Socket with number {key} could not be shut down. Closing... \n" + e);
                }
                finally
                {
                    socket.Close();
                }
            }

            // Close server listening socket
            try
            {
                //serverSocket.Shutdown(SocketShutdown.Both);
                serverSocket.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("SERVER: Socket could not be shut down. Closing..." + e);
            }
            Debug.Log("SERVER: Server was shut down.");
        }


        /// <summary>
        /// map the incoming data by its type to a handle method
        /// </summary>
        /// <param name="incomingData">received data from client</param>
        private static void delegateIncomingDataToMethods(Packet incomingData, int currentClientID)
        {
            switch (incomingData.type)
            {
                case (int)COMMUNICATION_METHODS.HANDLE_REQUEST_JOIN_LOBBY:
                    _serverReceive.handleRequestJoinLobby(incomingData, currentClientID);
                    break;

                case (int)COMMUNICATION_METHODS.HANDLE_PLAYER_READY:
                    _serverReceive.handleRequestPlayerReady(incomingData, currentClientID);
                    break;

                case (int)COMMUNICATION_METHODS.HANDLE_BEGIN_ROUND:
                    _serverReceive.handleBeginRound(incomingData);
                    break;

                case (int)COMMUNICATION_METHODS.HANDLE_TRADE_BANK:
                    _serverReceive.handleTradeBank(incomingData);
                    break;

                case (int)COMMUNICATION_METHODS.HANDLE_TRADE_OFFER:
                    _serverReceive.handleTradeOffer(incomingData);
                    break;

                case (int)COMMUNICATION_METHODS.HANDLE_BUILD:
                    _serverReceive.handleBuild(incomingData);
                    break;

                case (int)COMMUNICATION_METHODS.HANDLE_BUY_DEVELOPMENT:
                    _serverReceive.handleBuyDevelopement(incomingData);
                    break;

                case (int)COMMUNICATION_METHODS.HANDLE_PLAY_DEVELOPMENT:
                    _serverReceive.handlePlayDevelopement(incomingData);
                    break;

                case (int)COMMUNICATION_METHODS.HANDLE_END_TURN:
                    _serverReceive.handleEndTurn(incomingData);
                    break;

                default:
                    Debug.LogWarning($"SERVER: there was no target method send, invalid data packet. Packet Type: {incomingData.type}");
                    // TODO: trow exception!!!
                    break;
            }
        }
    }
}