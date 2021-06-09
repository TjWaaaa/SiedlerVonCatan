using Networking.Interfaces;
using Networking.Package;

namespace Tests.Networking
{
    public class MockClientReceive : INetworkableClient
    {
        public void handleClientJoined(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handlePlayerReadyNotification(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleGameStartInitialize(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleObjectPlacement(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleNextPlayer(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleVictory(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleClientDisconnect(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleRejection(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAccpetBeginRound(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAcceptTradeBank(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAcceptTradeOffer(Packet clientPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAcceptBuild(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleGetResources(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAcceptBuyDevelopement(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleAcceptPlayDevelopement(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleUpdateRP(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleUpdateOP(Packet serverPacket)
        {
            throw new System.NotImplementedException();
        }
    }
}