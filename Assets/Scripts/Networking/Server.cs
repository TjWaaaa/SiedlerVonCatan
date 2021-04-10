using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        public static void setupServer()
        {
            Console.Title = "Game Server";
            Console.WriteLine("Setting up Server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT)); //Bind endpoint with ip address and port to socket
            serverSocket.Listen(4); //maximum pending connections at one time
            serverSocket.BeginAccept(AcceptCallback, null); //begins waiting for client connection attempts
            
            Console.WriteLine("Server setup complete, let's go!");
            //todo: Close server after the end of the game.
            Console.WriteLine("Press any key to quit.");
            Console.ReadLine(); // to keep console open
            closeAllSockets();
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
                Console.WriteLine("ObjectDisposedException. Client disconnected?");
                return;
            }
            
            clientSockets.Add(clientSocket); //save client to socket list
            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, clientSocket); // open "chanel" to recieve data from the connected socket
            Console.WriteLine($"Client {clientSocket.RemoteEndPoint} connected, waiting for request...");
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
                Console.WriteLine("Client forcefully disconnected");
                currentClientSocket.Close();
                clientSockets.Remove(currentClientSocket);
                return;
            }

            byte[] currentBuffer = new byte[recievedByteLengh];
            Array.Copy(buffer, currentBuffer, recievedByteLengh); //to remove the protruding zeros from buffer
            string incomingDataString = Encoding.ASCII.GetString(currentBuffer);
            Console.WriteLine("Received Text: " + incomingDataString);
            //todo: handle incomingDataString
            
            if (incomingDataString.ToLower().Equals("get status"))
            {
                string dataString = $"Current status: \n connected clients: {clientSockets.Count} \n current buffer size: {BUFFER_SIZE}";
                byte[] dataToSend = Encoding.ASCII.GetBytes(dataString);
                currentClientSocket.Send(dataToSend);
                Console.WriteLine("Current status was requested and sent.");
            } else if (incomingDataString.ToLower().Equals("exit")) // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                currentClientSocket.Shutdown(SocketShutdown.Both);
                currentClientSocket.Close();
                clientSockets.Remove(currentClientSocket);
                Console.WriteLine("Client disconnected");
                return;
            }
            else
            {
                Console.WriteLine("Text is an invalid request");
                byte[] dataToSend = Encoding.ASCII.GetBytes("Invalid request");
                currentClientSocket.Send(dataToSend);
                Console.WriteLine("Warning Sent");
            }

            currentClientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback,
                currentClientSocket); // Begins waiting for incoming traffic again. Overwrites buffer.
        }
        
        
        /// <summary>
        /// needs to be called at the end of the session to close all connected Sockets and the serverSocket
        /// </summary>
        private static void closeAllSockets() {
            foreach (Socket socket in clientSockets) {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            serverSocket.Close();
        }
    }
}