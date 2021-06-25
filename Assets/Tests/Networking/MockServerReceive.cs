using Networking.Interfaces;
using Networking.Package;
using NUnit.Framework;

namespace Tests.Networking
{
    public class MockServerReceive : INetworkableServer
    {

        public static Packet packetHandleRequestJoinLobby;
        public static Packet packethandleRequestPlayerReady;
        public static Packet packethandleBeginRound;
        public static Packet packethandleTradeBank;
        public static Packet packethandleTradeOffer;
        public static Packet packethandleBuild;
        public static Packet packethandleBuyDevelopement;
        public static Packet packethandlePlayDevelopement;
        public static Packet packethandleEndTurn;
        public static Packet packethandleClientDisconnectServerCall;

        public static int playerID;

        public void handleRequestJoinLobby(Packet clientPacket, int currentClientID)
        {
            packetHandleRequestJoinLobby = clientPacket;
        }

        public void handleRequestPlayerReady(Packet clientPacket, int currentClientID)
        {
            packethandleRequestPlayerReady = clientPacket;
        }

        public void handleBeginRound(Packet clientPacket)
        {
            packethandleBeginRound = clientPacket;
        }

        public void handleTradeBank(Packet clientPacket)
        {
            packethandleTradeBank = clientPacket;
        }
        
        public void handleTradeOffer(Packet clientPacket)
        {
            packethandleTradeOffer = clientPacket;
        }

        public void handleBuild(Packet clientPacket)
        {
            packethandleBuild = clientPacket;
        }

        public void handleBuyDevelopement(Packet clientPacket)
        {
            packethandleBuyDevelopement = clientPacket;
        }

        public void handlePlayDevelopement(Packet clientPacket)
        {
            packethandlePlayDevelopement = clientPacket;
        }

        public void handleEndTurn(Packet clientPacket)
        {
            packethandleEndTurn = clientPacket;
        }

        public void handleClientDisconnectServerCall(int disconnectedClientID)
        {
            // packethandleClientDisconnectServerCall = disconnectedClientID;
        }
        

        public void generatePlayer(int playerId)
        {
            playerID = playerId;
        }
    }
}