using Enums;

namespace Networking.Communication
{
    public class ServerRequest : ServerToClientCommunication
    {
        public void notifyClientJoined(int playerID, string playerName, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleClientJoined;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void gamestartInitialize(int[][] gameBoard)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleGameStartInitialize;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyObjectPlacement(BUYABLES buildType, int buildID, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleObjectPlacement;
            packet.buildID = buildID;
            packet.buildType = (int) buildType;
            packet.buildColor = color;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyNextPlayer(string playerName)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleNextPlayer;
            packet.PlayerName = playerName;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyVictory(int playerID, string playerName, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleVictory;
            packet.PlayerNumber = playerID;
            packet.PlayerName = playerName;
            packet.PlayerColor = color;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyClientDisconnect(string playerName, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleClientDisconnect;
            packet.PlayerName = playerName;
            packet.PlayerColor = color;
            
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }
    }
}