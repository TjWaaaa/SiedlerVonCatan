using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Networking
{
    public class NetworkableClientExample// : INetworkableClient
    {
        public static void Main()
        {
            var net = new NetworkableClientExample();
            net.requestRollDice(test, test);
        }

        public void requestRollDice(INetworkableClient.acceptCallback acceptCallback, INetworkableClient.rejectCallback rejectCallback)
        {
            //Client.sendRequest("gib mir Würfel");

            acceptCallback(new Task(()=> Console.WriteLine("Task")));

        }
        // public void requestRollDice(acceptCallback, rejectCallback)
        // {
        //     sendData(object(ich will würfeln))
        //
        //     if (sendData == true)
        //     {
        //         acceptCallback(IAsyncResult)
        //     }
        //     else
        //     {
        //         rejectCallback(IAsyncResult);
        //     }
        // }

        public static void test(object ar)
        {
            Debug.Log("Würfeln erfolgreich");
            
        }
        

    }
}