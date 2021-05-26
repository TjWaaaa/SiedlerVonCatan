using System.Collections;
using Networking.Interfaces;
using UnityEngine;
using Networking.Package;
using Networking.ServerSide;
using Enums;
using System;
using System.Collections.Generic;
using Player;
using PlayerColor;

namespace Networking.Communication
{
    public class ServerRequest : ServerToClientCommunication
    {
        public void notifyClientJoined(ArrayList playerInformation, string lobbyIP)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_CLIENT_JOINED;
            packet.lobbyContent = playerInformation;
            packet.lobbyIP = lobbyIP;
            
            // send to all
            Server.sendDataToAll(packet);
        }

        public void gamestartInitialize(Hexagon[][] gameBoard)
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
            packet.myPlayerID = playerID;
            packet.resourcesObtained = resources;
            packet.victoryPoint = victoryPoints;
            
            // send to one
            Server.sendDataToOne(playerID, packet);
        }


        public void notifyObjectPlacement(BUYABLES buildType, int buildID, PLAYERCOLOR playerColor)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_OBJECT_PLACEMENT;
            packet.buildID = buildID;
            packet.buildType = (int) buildType;
            packet.buildColor = playerColor;

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


        public void notifyVictory(string playerName, PLAYERCOLOR playerColor)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_VICTORY;
            packet.playerName = playerName;
            packet.playerColor = playerColor;
            Debug.Log("SERVER: " + playerColor);
            
            // send to all
            Server.sendDataToAll(packet);
        }


        public void notifyClientDisconnect(string playerName, PLAYERCOLOR playerColor)
        {
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_CLIENT_DISCONNECT;
            packet.playerName = playerName;
            packet.playerColor = playerColor;
            
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
            packet.currentPlayerID = currentClientID;
            
            // send to all but the player that changed its status
            Server.sendDataToAllButOne(currentClientID, packet);
        }


        public void notifyRollDice(int[] diceResult)
        {
            Debug.Log("SERVER: notifyRollDice has been called");
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
            packet.myPlayerID = playerID;
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

        public void updateRepPlayers(int[][] updateNumbers)
        {
            Debug.Log("SERVER: ServerRequest updateRPPacket");
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_UPDATE_RP;
            packet.updateRP = updateNumbers;
            

            // send to all
            Server.sendDataToAll(packet);
        }

        public void updateOwnPlayer(int[] updateLeftBuildings,Dictionary<RESOURCETYPE, int> updateResources, int playerID)
        {
            Debug.Log("SERVER: ServerRequest updateOwnPlayer");
            Packet packet = new Packet();
            packet.type = (int) COMMUNICATION_METHODS.HANDLE_UPDATE_OP;
            packet.updateOP = updateLeftBuildings;
            packet.updateResourcesOnOP = updateResources;

            // send to the current player
            Server.sendDataToOne(playerID,packet);
        }
    }
}