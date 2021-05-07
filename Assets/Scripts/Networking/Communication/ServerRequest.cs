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
            
            // send to all
            Server.sendDataToAll(PacketSerializer.objectToJsonString(packet));
        }


        public void gamestartInitialize(int[][] gameBoard)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleGameStartInitialize;
            packet.gameBoard = gameBoard;
            
            // send to all
            Server.sendDataToAll(PacketSerializer.objectToJsonString(packet));
        }

        public void distributeResources(int playerID, int[] resources, int victoryPoints)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleGetResources;
            packet.playerNumber = playerID;
            packet.resourcesObtained = resources;
            packet.victoryPoint = victoryPoints;
            
            // send to one
            Server.sendDataToOne(playerID, PacketSerializer.objectToJsonString(packet));
        }


        public void notifyObjectPlacement(BUYABLES buildType, int buildID, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleObjectPlacement;
            packet.buildID = buildID;
            packet.buildType = (int) buildType;
            packet.buildColor = color;
            
            // send to all
            Server.sendDataToAll(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyNextPlayer(string playerName)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleNextPlayer;
            packet.playerName = playerName;
            
            // send to all
            Server.sendDataToAll(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyVictory(string playerName, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleVictory;
            packet.playerName = playerName;
            packet.playerColor = color;
            
            // send to all
            Server.sendDataToAll(PacketSerializer.objectToJsonString(packet));
        }


        public void notifyClientDisconnect(string playerName, string color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleClientDisconnect;
            packet.playerName = playerName;
            packet.playerColor = color;
            
            // send to all
            Server.sendDataToAll(PacketSerializer.objectToJsonString(packet));
        }


        // return requested information/resources ------------------------

        public void notifyRejection(int playerID, string errorMessage)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleRejection;
            packet.errorMessage = errorMessage;
            
            // send to active
            Server.sendDataToOne(playerID, PacketSerializer.objectToJsonString(packet));
        }


        public void notifyRollDice(int[] diceResult)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleAccpetBeginRound;
            packet.diceResult = diceResult;
            
            // send to all
            Server.sendDataToAll(PacketSerializer.objectToJsonString(packet));
        }

        public void acceptBuyDevelopement(int playerID, DEVELOPMENT_TYPE developmentCard)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleAcceptBuyDevelopement;
            packet.playerNumber = playerID;
            packet.developmentCard = (int) developmentCard;
            
            // send to active
            Server.sendDataToOne(playerID, PacketSerializer.objectToJsonString(packet));
        }


        public void notifyAcceptPlayDevelopement(DEVELOPMENT_TYPE developmentCard, string playerName)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.handleAcceptPlayDevelopement;
            packet.developmentCard = (int) developmentCard;
            packet.playerName = playerName;
            
            // send to all
            Server.sendDataToAll(PacketSerializer.objectToJsonString(packet));
        }
    }
}