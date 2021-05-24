using System.Collections;
using Networking.Interfaces;
using UnityEngine;
using Networking.Package;
using Networking.ServerSide;
using Enums;
using System;
using System.Collections.Generic;



namespace Networking.Communication
{
    public class ServerRequest : ServerToClientCommunication
    {
        public void notifyClientJoined(ArrayList playerInformation)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_CLIENT_JOINED;
            packet.lobbyContent = playerInformation;
            
            // send to all
            Server.sendDataToAll(packet);
        }

        public void gamestartInitialize(int[][] gameBoard)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_GAMESTART_INITIALIZE;
            packet.gameBoard = gameBoard;
            
            // send to all
            Server.sendDataToAll(packet);
        }

        public void distributeResources(int playerID, int[] resources, int victoryPoints)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_GET_RESOURCES;
            packet.playerID = playerID;
            packet.resourcesObtained = resources;
            packet.victoryPoint = victoryPoints;
            
            // send to one
            Server.sendDataToOne(playerID, packet);
        }


        public void notifyObjectPlacement(BUYABLES buildType, int buildID, Color color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_OBJECT_PLACEMENT;
            packet.buildID = buildID;
            packet.buildType = (int) buildType;
            packet.buildColor = color.ToString();
            
            // send to all
            Server.sendDataToAll(packet);
        }


        public void notifyNextPlayer(string playerName)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_NEXT_PLAYER;
            packet.playerName = playerName;
            
            // send to all
            Server.sendDataToAll(packet);
        }


        public void notifyVictory(string playerName, Color color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_VICTORY;
            packet.playerName = playerName;
            packet.playerColor = new float[] {color.r, color.g, color.b, color.a};
            
            // send to all
            Server.sendDataToAll(packet);
        }


        public void notifyClientDisconnect(string playerName, Color color)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_CLIENT_DISCONNECT;
            packet.playerName = playerName;
            packet.playerColor = new float[] {color.r, color.g, color.b, color.a};
            
            // send to all
            Server.sendDataToAll(packet);
        }


        // return requested information/resources ------------------------

        public void notifyRejection(int playerID, string errorMessage)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_REJECTION;
            packet.errorMessage = errorMessage;
            
            // send to active
            Server.sendDataToOne(playerID, packet);
        }

        public void notifyPlayerReady(int currentClientID, string playerName, bool readyStatus)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_PLAYER_READY_NOTIFICATION;
            packet.playerName = playerName;
            packet.isReady = readyStatus;
            packet.currentPlayerNumber = currentClientID;
            
            // send to all but the player that changed its status
            Server.sendDataToAllButOne(currentClientID, packet);
        }


        public void notifyRollDice(int[] diceResult)
        {
            Debug.Log("notifyRollDice has been called");
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BEGIN_ROUND;
            packet.diceResult = diceResult;
            
            // send to all
            Server.sendDataToAll(packet);
        }

        public void acceptBuyDevelopement(int playerID, DEVELOPMENT_TYPE developmentCard)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_BUY_DEVELOPMENT_CARD;
            packet.playerID = playerID;
            packet.developmentCard = (int) developmentCard;
            
            // send to active
            Server.sendDataToOne(playerID, packet);
        }


        public void notifyAcceptPlayDevelopement(DEVELOPMENT_TYPE developmentCard, string playerName)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_ACCEPT_PLAY_DEVELOPMENT_CARD;
            packet.developmentCard = (int) developmentCard;
            packet.playerName = playerName;
            
            // send to all
            Server.sendDataToAll(packet);
        }
    }
}