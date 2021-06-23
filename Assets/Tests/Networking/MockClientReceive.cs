using Networking.Interfaces;
using Networking.Package;

namespace Tests.Networking
{
    public class MockClientReceive : INetworkableClient
    {
        public static Packet packethandleClientJoined;
        public static Packet packethandlePlayerReadyNotification;
        public static Packet packethandleGameStartInitialize;
        public static Packet packethandleObjectPlacement;
        public static Packet packethandleNextPlayer;
        public static Packet packethandleVictory;
        public static Packet packethandleClientDisconnect;
        public static Packet packethandleRejection;
        public static Packet packethandleAccpetBeginRound;
        public static Packet packethandleAcceptTradeOffer;
        public static Packet packethandleAcceptBuyDevelopement;
        public static Packet packethandleAcceptPlayDevelopement;
        public static Packet packethandleUpdateRP;
        public static Packet packethandleUpdateOP;


        public static int clientID;


        public void handleClientJoined(Packet serverPacket)
        {
            packethandleClientJoined = serverPacket;
            clientID = serverPacket.myPlayerID;
        }

        public void handlePlayerReadyNotification(Packet serverPacket)
        {
            packethandlePlayerReadyNotification = serverPacket;
            clientID = serverPacket.myPlayerID;

        }

        public void handleGameStartInitialize(Packet serverPacket)
        {
            packethandleGameStartInitialize = serverPacket;
            clientID = serverPacket.myPlayerID;

        }

        public void handleObjectPlacement(Packet serverPacket)
        {
            packethandleObjectPlacement = serverPacket;
            clientID = serverPacket.myPlayerID;
        }

        public void handleNextPlayer(Packet serverPacket)
        {
            packethandleNextPlayer = serverPacket;
            clientID = serverPacket.myPlayerID;
        }

        public void handleVictory(Packet serverPacket)
        {
            packethandleVictory = serverPacket;
            clientID = serverPacket.myPlayerID;
        }

        public void handleClientDisconnect(Packet serverPacket)
        {
            packethandleClientDisconnect = serverPacket;
            clientID = serverPacket.myPlayerID;
        }

        public void handleRejection(Packet serverPacket)
        {
            packethandleRejection = serverPacket;
            clientID = serverPacket.myPlayerID;
        }

        public void handleAccpetBeginRound(Packet serverPacket)
        {
            packethandleAccpetBeginRound = serverPacket;
            clientID = serverPacket.myPlayerID;
        }

        public void handleAcceptTradeOffer(Packet clientPacket)
        {
            packethandleAcceptTradeOffer = clientPacket;
            clientID = clientPacket.myPlayerID;
        }

        public void handleAcceptBuyDevelopement(Packet serverPacket)
        {
            packethandleAcceptBuyDevelopement = serverPacket;
            clientID = serverPacket.myPlayerID;
        }

        public void handleAcceptPlayDevelopement(Packet serverPacket)
        {
            packethandleAcceptPlayDevelopement = serverPacket;
            clientID = serverPacket.myPlayerID;
        }

        public void handleUpdateRP(Packet serverPacket)
        {
            packethandleUpdateRP = serverPacket;
            clientID = serverPacket.myPlayerID;
        }

        public void handleUpdateOP(Packet serverPacket)
        {
            packethandleUpdateOP = serverPacket;
            clientID = serverPacket.myPlayerID;
        }
    }
}