using Networking.Interfaces;
using Networking.Package;
using NUnit.Framework;

namespace Tests.Networking
{
    public class MockServerReceive : INetworkableServer
    {
        public void handleRequestJoinLobby(Packet clientPacket, int currentClientID)
        {
            throw new System.NotImplementedException();
        }

        public void handleRequestPlayerReady(Packet clientPacket, int currentClientID)
        {
            throw new System.NotImplementedException();
        }

        public void handleBeginRound(Packet clientPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleTradeBank(Packet clientPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleBuild(Packet clientPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleBuyDevelopement(Packet clientPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handlePlayDevelopement(Packet clientPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleEndTurn(Packet clientPacket)
        {
            throw new System.NotImplementedException();
        }

        public void handleClientDisconnectServerCall(int disconnectedClientID)
        {
            throw new System.NotImplementedException();
        }

        public void handleTradeOffer(Packet clientPacket)
        {
            throw new System.NotImplementedException();
        }

        public void generatePlayer(int playerId)
        {
            //todo: think of something intelligent to test here.
        }
    }
}