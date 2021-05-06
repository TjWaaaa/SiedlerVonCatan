using Enums;

namespace Networking
{
    public class ClientRequest : ClientToServerCommunication
    {
        public void requestBeginRound()
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleBeginRound;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestTradeBank(int[] offer, int[] expect)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleTradeBank;
            packet.tradeResourcesOffer = offer;
            packet.tradeResourcesExpect = expect;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestBuild(BUYABLES type, int buildID)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleBuild;
            packet.buildID = buildID;
            packet.buildType = (int) type;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestBuyDevelopement()
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleBuyDevelopement;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestPlayDevelopement(DEVELOPMENT_TYPE developmentType)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handlePlayDevelopement;
            packet.developementCard = (int) developmentType;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestEndTurn()
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleEndTurn;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }
    }
}