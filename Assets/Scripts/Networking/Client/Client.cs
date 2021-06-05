using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using Enums;
using Networking.Interfaces;
using UnityEngine;
using Networking.Package;

namespace Networking.ClientSide
{
    public class Client
    {
        private static bool isRunning = false;
        private const int BUFFER_SIZE = 64000;
        private const int PORT = 50042;
        private static byte[] buffer;

        private static Socket clientSocket;

        private static INetworkableClient _clientReceive;
        
        private static long timeOfLastPing;
        private static Timer keepAliveTimer;
        private const int disconnetThreshold = 50000000; // in .net Ticks


        /// <summary>
        /// Initializes a client instance and tries to connect to the game server using the given IP address.
        /// After 5 attempts the connecting attempts are aborted.
        /// </summary>
        /// <param name="ipAddress">IP address of the server</param>
        /// <returns>true if connection was established successfully. Otherwise false.</returns>
        /// <exception cref="Exception"></exception>
        public static bool initClient(string ipAddress, INetworkableClient clientReceive)
        {
            if (isRunning)
            {
                Debug.LogWarning("Client has already been started. Aborting initialisation...");
                return true;
            }
            
            timeOfLastPing = DateTime.Now.Ticks;
            keepAliveTimer = new Timer(1000); // Check every second for disconnect
            keepAliveTimer.AutoReset = true;
            keepAliveTimer.Elapsed += checkReceivedPing;
            keepAliveTimer.Start();
            
            // instantiate a ClientGameLogic object
            // var gameLogicObject = new GameObject();
            // gameLogicObject.AddComponent<ClientReceive>();
            // gameLogicObject.AddComponent<BoardGenerator>();
            // _clientReceive = gameLogicObject.GetComponent<ClientReceive>();
            _clientReceive = clientReceive;

            buffer = new byte[BUFFER_SIZE];
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                bool connectionSuccess = connectToServer(ipAddress);
                if (!connectionSuccess)
                {
                    return isRunning;
                }

                clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallback, clientSocket);
            }
            catch (Exception e)
            {
                Debug.Log("SERVER: Client could not start");
                throw e;
            }

            isRunning = true;
            return isRunning;
        }


        /// <summary>
        /// Try to connect to the server socket via user input ip address.
        /// Blocks for that time.
        /// </summary>
        /// <param name="ipAddress">IP address of the server</param>
        /// <returns>true if connection was established successfully. Otherwise false.</returns>
        private static bool connectToServer(string ipAddress)
        {
            int attempts = 0;

            while (!clientSocket.Connected && attempts < 5)
            {
                try
                {
                    attempts++;
                    Debug.Log("CLIENT: Client: Connection attempts: " + attempts);
                    clientSocket.Connect(IPAddress.Parse(ipAddress), PORT);
                }
                catch (SocketException)
                {
                    Debug.Log("CLIENT: Client: Connection Error");
                }
            }

            if (attempts >= 5)
            {
                Debug.Log("CLIENT: Failed connecting to Server!");
                return false;
            }

            Debug.Log("CLIENT: Connected");
            return true;
        }


        /// <summary>
        /// Sends a request to the server.
        /// </summary>
        /// <param name="request">Data to send</param>
        public static void sendRequest(string request)
        {
            Debug.Log("CLIENT: Sending a request: " + request);

            byte[] buffer = Encoding.ASCII.GetBytes(request);
            clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, sendCallback, clientSocket);
        }

        /// <summary>
        /// Call this method regularly
        /// Compares time between receiving of last keepAlivePing and current time.
        /// If time is longer than disconnectThreshold try to reconnect.
        /// todo: this Method is probably not needet due to TCPs own timers.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="elapsedEventArgs"></param>
        private static void checkReceivedPing(object source, ElapsedEventArgs elapsedEventArgs)
        {
            if (elapsedEventArgs.SignalTime.Ticks - timeOfLastPing > disconnetThreshold)
            {
                //todo: Reconnect and/or display in lobby
                Debug.LogError("CLIENT: Lost connection to server");
            }
        }


        /// <summary>
        /// Callback method is called when the client has finished sending data.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void sendCallback(IAsyncResult AR)
        {
            clientSocket.EndSend(AR);
        }


        /// <summary>
        /// Callback method is called in case of data being sent to the client.
        /// </summary>
        /// <param name="AR">IAsyncResult</param>
        private static void receiveCallback(IAsyncResult AR)
        {
            // necessary to log errors that occur in the side thread
            try
            {
                Socket currentServerSocket = (Socket) AR.AsyncState;
                int receivedBufferSize;

                try
                {
                    receivedBufferSize = currentServerSocket.EndReceive(AR);
                    Debug.Log("CLIENT: receivedBufferSize: " + receivedBufferSize);
                }
                catch (ObjectDisposedException e)
                {
                    Debug.Log("CLIENT: Object disposed exception. Happens always.\n" + e.Message);
                    //TODO handle connection loss
                    return;
                }
                
                // Server Socket was shut down
                if (receivedBufferSize <= 0)
                {
                    Debug.LogWarning("CLIENT: Received null from server.");
                    return;
                }
                
                byte[] receievedBuffer = new byte[receivedBufferSize];
                Array.Copy(buffer, receievedBuffer, receivedBufferSize);
                var jsonString = Encoding.ASCII.GetString(receievedBuffer);
                
                //Keep alive Ping
                if (jsonString == "ping")
                {
                    timeOfLastPing = DateTime.Now.Ticks;
                    sendRequest("pong");
                } 
                else
                {
                    Debug.Log("CLIENT: received Data: " + jsonString);
                    Packet serverData = PacketSerializer.jsonToObject(jsonString);
                
                    delegateIncomingDataToMethods(serverData);
                }

                clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallback, clientSocket); // start listening again
            }
            catch (Exception e)
            {
                Debug.LogError("CLIENT: " + e.HelpLink + e);
                throw e;
            }
        }


        /// <summary>
        /// If the the client is running all sockets are closed and timers are stopped and disposed.
        /// </summary>
        public static void shutDownClient()
        {
            lock (keepAliveTimer)
            {
                if (isRunning)
                {
                    isRunning = false;
                    
                    keepAliveTimer.Stop();
                    keepAliveTimer.Dispose();
            
                    // Close client Socket
                    try
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"CLIENT: Client socket could not be closed " + e.Message);
                    }
                    finally
                    {
                        clientSocket.Close();
                    }
                    Debug.Log("CLIENT: Client was shut down.");
                }
            }
        }


        /// <summary>
        /// map the incoming data by its type to a handle method
        /// </summary>
        /// <param name="incomingData">received data from server</param>
        private static void delegateIncomingDataToMethods(Packet incomingData)
        {
            try
            {
                switch (incomingData.type)
                {
                    case (int) COMMUNICATION_METHODS.HANDLE_CLIENT_JOINED:
                        // todo: eliminate race condition!!!
                        // while (SceneManager.GetActiveScene().name != "Lobby") ; //Waiting for Unity to load lobby
                        
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleClientJoined(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_GAMESTART_INITIALIZE:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleGameStartInitialize(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY_NOTIFICATION:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handlePlayerReadyNotification(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_OBJECT_PLACEMENT:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleObjectPlacement(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_NEXT_PLAYER:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleNextPlayer(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_VICTORY:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleVictory(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_CLIENT_DISCONNECT:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleClientDisconnect(incomingData);
                        });
                        break;


                    case (int) COMMUNICATION_METHODS.HANDLE_REJECTION:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleRejection(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BEGIN_ROUND:
                        Debug.Log("CLIENT: calling handleAcceptBeginRound in ClientReceive");
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleAccpetBeginRound(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_TRADE_BANK:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleAcceptTradeBank(incomingData);
                        });
                        break;
                    
                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_TRADE_OFFER:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleAcceptTradeOffer(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BUILD:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleAcceptBuild(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_GET_RESOURCES:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleGetResources(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BUY_DEVELOPMENT_CARD:
                        ThreadManager.executeOnMainThread(() =>
                        {
                            _clientReceive.handleAcceptBuyDevelopement(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_PLAY_DEVELOPMENT_CARD:
                        ThreadManager.executeOnMainThread(() =>
                        {  
                            _clientReceive.handleAcceptPlayDevelopement(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_UPDATE_RP:
                        ThreadManager.executeOnMainThread(() =>
                        {  
                            _clientReceive.handleUpdateRP(incomingData);
                        });
                        break;

                    case (int) COMMUNICATION_METHODS.HANDLE_UPDATE_OP:
                        ThreadManager.executeOnMainThread(() =>
                        {  
                            _clientReceive.handleUpdateOP(incomingData);
                        });
                        break;

                    default:
                        Debug.LogWarning($"CLIENT: there was no target method send, invalid data packet. Packet Type: {incomingData.type}");
                        // TODO: throw exception!!!
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
            string receievedText = PacketSerializer.objectToJsonString(incomingData);
            Debug.Log("CLIENT: Incoming Data: " + receievedText);
        }
    }
}