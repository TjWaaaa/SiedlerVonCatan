using Enums;

namespace Networking.Communication
{
    public class ServerRequest : ServerToClientCommunication
    {
        public void notifyClientJoined(int playerID, string playerName, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleClientJoined;
            packet.playerNumber = playerID;
            packet.playerName = playerName;
            packet.playerColor = color;
            
            // send to activ
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void gamestartInitialize(int[][] gameBoard)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleGameStartInitialize;
            packet.gameBoard = gameBoard;
            
            // send to all
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void distributeResources(int playerID, int[] resources, int victoryPoints)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleGetResources;
            packet.playerNumber = playerID;
            packet.resourcesObtained = resources;
            packet.victoryPoint = victoryPoints;
            
            // send to active|send to inactive
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyObjectPlacement(BUYABLES buildType, int buildID, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleObjectPlacement;
            packet.buildID = buildID;
            packet.buildType = (int) buildType;
            packet.buildColor = color;
            
            // send to active|send to inactive
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyNextPlayer(string playerName)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleNextPlayer;
            packet.playerName = playerName;
            
            // send to active|send to inactive
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyVictory(int playerID, string playerName, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleVictory;
            packet.playerNumber = playerID;
            packet.playerName = playerName;
            packet.playerColor = color;
            
            // send to all
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyClientDisconnect(string playerName, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleClientDisconnect;
            packet.playerName = playerName;
            packet.playerColor = color;
            
            // send to all
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        // return requested information/resources ------------------------

        public void notifyRejection(string errorMessage)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleRejection;
            packet.errorMessage = errorMessage;
            
            // send to active
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyAccpetBeginRound(int[] diceResult)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleAccpetBeginRound;
            packet.diceResult = diceResult;
            
            // send to active
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }

        public void acceptBuyDevelopement(int playerID, DEVELOPMENT_TYPE developmentCard)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleAcceptBuyDevelopement;
            packet.playerNumber = playerID;
            packet.developmentCard = (int) developmentCard;
            
            // send to active
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyAcceptPlayDevelopement(DEVELOPMENT_TYPE developmentCard, string playerName)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleAcceptPlayDevelopement;
            packet.developmentCard = (int) developmentCard;
            packet.playerName = playerName;
            
            // send to active|send to inactive
            Client.sendRequest(PacketSerializer.objectToJsonString(packet));
        }
    }
}