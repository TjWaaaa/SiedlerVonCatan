using System.Collections;
using Networking.Interfaces;
using UnityEngine;
using Networking.Package;
using Networking.ServerSide;
using Enums;
using System.Collections.Generic;

namespace Networking.Communication
{
    public class ServerRequest : ServerToClientCommunication
    {
        public void notifyClientJoined(ArrayList playerInformation, string lobbyIP)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_CLIENT_JOINED;
            packet.lobbyContent = playerInformation;
            packet.lobbyIP = lobbyIP;

            // send to all
            Server.sendDataToAll(packet);
            Debug.Log("SERVER: Client should expect a NotifyClientJoined package Type 10");
        }

        public void gamestartInitialize(Hexagon[][] gameBoard)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_GAMESTART_INITIALIZE;
            packet.gameBoard = gameBoard;

            // send to all
            Server.sendDataToAll(packet);
            Debug.Log("SERVER: Client should expect a gameStartInitialize package Type 11");
        }


        public void notifyObjectPlacement(BUYABLES buildType, int buildID, PLAYERCOLOR buildColor)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_OBJECT_PLACEMENT;
            packet.buildID = buildID;
            packet.buildType = (int)buildType;
            packet.buildColor = buildColor;

            // send to all
            Server.sendDataToAll(packet);
            Debug.Log("SERVER: Client should expect a NotifyObjectPlacement package Type 13");

        }


        public void notifyNextPlayer(int playerIndex, int previousPlayerIndex)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_NEXT_PLAYER;
            packet.currentPlayerID = playerIndex;
            packet.previousPlayerID = previousPlayerIndex;

            // send to all
            Server.sendDataToAll(packet);
            Debug.Log("SERVER: Client should expect a NotifyNextPlayer package Type 14");
        }


        public void notifyVictory(string playerName, PLAYERCOLOR playerColor)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_VICTORY;
            packet.playerName = playerName;
            packet.playerColor = playerColor;
            Debug.Log("SERVER: " + playerColor);

            // send to all
            Server.sendDataToAll(packet);
            Debug.Log("SERVER: Client should expect a NotifyVictory package Type 15");
        }


        public void notifyClientDisconnect(string playerName, PLAYERCOLOR playerColor)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_CLIENT_DISCONNECT;
            packet.playerName = playerName;
            packet.playerColor = playerColor;

            // send to all
            Server.sendDataToAll(packet);
            Debug.Log("SERVER: Client should expect a NotifyClientDisconnect package Type 17");
        }


        // return requested information/resources ------------------------

        public void notifyRejection(int playerID, string errorMessage)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_REJECTION;
            packet.errorMessage = errorMessage;

            // send to active
            Server.sendDataToOne(playerID, packet);
            Debug.Log("SERVER: Client should expect a NotifyRejection package Type 18");
        }

        public void notifyPlayerReady(int currentClientID, string playerName, bool readyStatus)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_PLAYER_READY_NOTIFICATION;
            packet.playerName = playerName;
            packet.isReady = readyStatus;
            packet.currentPlayerID = currentClientID;

            // send to all but the player that changed its status
            Server.sendDataToAllButOne(currentClientID, packet);
            Debug.Log("SERVER: Client should expect a NotifyPlayerReady package Type 19");
        }


        public void notifyRollDice(int[] diceResult)
        {
            Debug.Log("SERVER: notifyRollDice has been called");
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_ACCEPT_BEGIN_ROUND;
            packet.diceResult = diceResult;

            // send to all
            Server.sendDataToAll(packet);
            Debug.Log("SERVER: Client should expect a Dice package Type 20");
        }

        public void acceptBuyDevelopement(int leftDevCards)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_ACCEPT_BUY_DEVELOPMENT_CARD;
            packet.leftDevCards = leftDevCards;

            // send to active
            Server.sendDataToAll(packet);
            Debug.Log("SERVER: Client should expect an AcceptBuyDevelopement package Type 25");
        }


        public void notifyAcceptPlayDevelopement(int playerID, DEVELOPMENT_TYPE developmentCard, string playerName)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_ACCEPT_PLAY_DEVELOPMENT_CARD;
            packet.developmentCard = developmentCard;
            packet.playerName = playerName;

            // send to all
            Server.sendDataToOne(playerID, packet);
            Debug.Log("SERVER: Client should expect an AcceptPlayDevelopement package Type 26");
        }

        public void notifyAcceptTradeOffer(int playerID, int buttonNumber)
        {
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_ACCEPT_TRADE_OFFER;
            packet.buttonNumber = buttonNumber;

            // send to active
            Server.sendDataToOne(playerID, packet);
            Debug.Log("SERVER: trade offer accepted");
        }

        public void updateRepPlayers(int[][] updateNumbers)
        {
            Debug.Log("SERVER: ServerRequest updateRPPacket");
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_UPDATE_RP;
            packet.updateRP = updateNumbers;


            // send to all
            Server.sendDataToAll(packet);
            Debug.Log("SERVER: Client should expect an UpdateRP package Type 27");
        }

        public void updateOwnPlayer(int[] updateLeftBuildings, Dictionary<RESOURCETYPE, int> updateResources, Dictionary<DEVELOPMENT_TYPE, int> updateDevCards, int playerID)
        {
            Debug.Log("SERVER: ServerRequest updateOwnPlayer");
            Packet packet = new Packet();
            packet.type = (int)COMMUNICATION_METHODS.HANDLE_UPDATE_OP;
            packet.updateOP = updateLeftBuildings;
            packet.updateResourcesOnOP = updateResources;
            packet.updateDevCardsOnOP = updateDevCards;

            // send to the current player
            Server.sendDataToOne(playerID, packet);
            Debug.Log("SERVER: Current Client should expect an UpdateOP package Type 28");
        }
    }
}