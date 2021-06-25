using Enums;
using Networking.Interfaces;
using Networking.Package;
using Networking.ClientSide;

namespace Networking.Communication
{
    public class ClientRequest : ClientToServerCommunication
    {
        public void requestJoinLobby(string playerName)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_REQUEST_JOIN_LOBBY;
            packet.playerName = playerName;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestPlayerReady(bool isReady)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY;
            packet.isReady = isReady;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestTradeBank(int[] offer, int[] expect)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_TRADE_BANK;
            packet.tradeResourcesOffer = offer;
            packet.tradeResourcesExpect = expect;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestTradeOffer(RESOURCETYPE offerResource, int buttonNumber)
        {
            Packet packet = new Packet();               
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_TRADE_OFFER;
            packet.resourceType = (int) offerResource;
            packet.buttonNumber = buttonNumber;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestBuild(BUYABLES type, int buildID)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_BUILD;
            packet.buildID = buildID;
            packet.buildType = (int) type;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestBuyDevelopement()
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_BUY_DEVELOPMENT;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestPlayDevelopement(DEVELOPMENT_TYPE developmentType)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_PLAY_DEVELOPMENT;
            packet.developmentCard = developmentType;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void requestEndTurn()
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_END_TURN;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }
    }
}