using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Networking
{
    public class Client
    {
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 50042;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        private static readonly Socket clientSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public static void initClient(string ipAddress)
        {
            Console.Title = "Client 1";
            connectToServer(ipAddress);
            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallback, clientSocket);
            //while (true)
            //{
                sendRequest();
            //}
        }
        

        /// <summary>
        /// Try to connect to the server socket via user input ip address.
        /// Blocks for that time.
        /// </summary>
        private static bool connectToServer(string ipAddress)
        {
            int attempts = 0;
            
            Console.WriteLine("Please enter ip address of host: ");
            //string ipAddress = Console.ReadLine();
            //string ipAddress = "127.0.0.1";
            
            while(!clientSocket.Connected) {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempts: " + attempts);
                    clientSocket.Connect(IPAddress.Parse(ipAddress), PORT);
                }
                catch (SocketException)
                {
                    Console.Clear();
                }
            } 
            
            Console.Clear();
            Console.WriteLine("Connected");
            return true;
        }
        
        
        /// <summary>
        /// Sends a request to the server
        /// </summary>
        private static void sendRequest()
        {
            Console.Write("Send a request: (get status)");
            //string request = Console.ReadLine();
            string request = "Hallo Welt";
            
            byte[] buffer = Encoding.ASCII.GetBytes(request);
            clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            
            //TODO: send game data and messages -> may send JSON
        }
        
        private static void receiveCallback(IAsyncResult AR) {
            Socket currentServerSocket = (Socket) AR.AsyncState;
            int receivedBufferSize;

            try
            {
                receivedBufferSize = currentServerSocket.EndReceive(AR);
            }
            catch (SocketException)
            {
                Console.WriteLine("Server forcefully disconnected");
                //todo handle connection loss
                return;
            }
            
            byte[] receievedBuffer = new byte[receivedBufferSize];
            Array.Copy(buffer, receievedBuffer, receivedBufferSize);
            //may recieve JSON
            string receievedText = Encoding.ASCII.GetString(receievedBuffer);
            Console.WriteLine("Incoming Data: " + receievedText);
            
            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallback, clientSocket); // start listening again
        }
    }
}