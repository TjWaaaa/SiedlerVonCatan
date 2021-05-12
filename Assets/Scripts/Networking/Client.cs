using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Enums;
using UnityEngine;

namespace Networking
{
    public class Client
    {
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 50042;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        private static readonly Socket clientSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private static ClientGameLogic clientGameLogic = new ClientGameLogic();


        /// <summary>
        /// Initializes a client instance and tries to connect to the game server using the given IP address.
        /// After 5 attempts the connecting attempts are aborted.
        /// </summary>
        /// <param name="ipAddress">IP address of the server</param>
        /// <returns>true if connection was established successfully. Otherwise false.</returns>
        /// <exception cref="Exception"></exception>
        public static bool initClient(string ipAddress)
        {
            try
            {
                bool connectionSuccess = connectToServer(ipAddress);
                if (!connectionSuccess)
                {
                    return false;
                }
                clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallback, clientSocket);
            } 
            catch (Exception e)
            {
                Debug.Log("Client could not start");
                throw e;
            }
            
            return true;
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
            
            while(!clientSocket.Connected && attempts < 5) {
                try
                {
                    attempts++;
                    Debug.Log("Client: Connection attempts: " + attempts);
                    clientSocket.Connect(IPAddress.Parse(ipAddress), PORT);
                }
                catch (SocketException)
                {
                    Debug.Log("Client: Connection Error");
                }
            }

            if (attempts >= 5)
            {
                Debug.Log("Client: Failed connecting to Server!");
                return false;
            }
            
            Debug.Log("Client: Connected");
            return true;
        }
        
        
        /// <summary>
        /// Sends a request to the server.
        /// </summary>
        /// <param name="request">Data to send</param>
        public static void sendRequest(string request)
        {
            Debug.Log("Client: Sending a request" + request);

            byte[] buffer = Encoding.ASCII.GetBytes(request);
            clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, sendCallback, clientSocket);
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
        private static void receiveCallback(IAsyncResult AR) {
            Socket currentServerSocket = (Socket) AR.AsyncState;
            int receivedBufferSize;

            try
            {
                receivedBufferSize = currentServerSocket.EndReceive(AR);
            }
            catch (SocketException)
            {
                Debug.Log("Client: Server forcefully disconnected");
                //todo handle connection loss
                return;
            }
            
            byte[] receievedBuffer = new byte[receivedBufferSize];
            Array.Copy(buffer, receievedBuffer, receivedBufferSize);
            Packet serverData = PacketSerializer.jsonToObject(Encoding.ASCII.GetString(receievedBuffer));
            
            delegateIncomingDataToMethods(serverData);
            
            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallback, clientSocket); // start listening again
        }
        
        
        /// <summary>
        /// map the incoming data by its type to a handle method
        /// </summary>
        /// <param name="incomingData">received data from server</param>
        private static void delegateIncomingDataToMethods(Packet incomingData)
        {
            switch (incomingData.type)
            {
                case (int) COMMUNICATION_METHODS.handleClientJoined:
                    clientGameLogic.handleClientJoined(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleGameStartInitialize:
                    //handleGameStartInitialize(incomingData);
                    break;

                case (int) COMMUNICATION_METHODS.handleObjectPlacement:
                    //handleObjectPlacement(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleNextPlayer:
                    //handleNextPlayer(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleVictory:
                    //handleVictory(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleClientDisconnect:
                    //handleClientDisconnect(incomingData);
                    break;
                
                
                case (int) COMMUNICATION_METHODS.handleRejection:
                    //handleRejection(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleAccpetBeginRound:
                    //handleAccpetBeginRound(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleAcceptTradeBank:
                    //handleAcceptTradeBank(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleAcceptBuild:
                    //handleAcceptBuild(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleGetResources:
                    //handleGetResources(incomingData);
                    break;
                
                case (int) COMMUNICATION_METHODS.handleAcceptBuyDevelopement:
                    //handleAcceptBuyDevelopement(incomingData);
                    break;

                case (int) COMMUNICATION_METHODS.handleAcceptPlayDevelopement:
                    //handleAcceptPlayDevelopement(incomingData);
                    break;
                
                default:
                    Debug.LogWarning("there was no target method send, invalid data packet");
                    // TODO: trow exception!!!
                    break;
            }
            
            string receievedText = PacketSerializer.objectToJsonString(incomingData);
            Debug.Log("Client: Incoming Data: " + receievedText);
        }
    }
}