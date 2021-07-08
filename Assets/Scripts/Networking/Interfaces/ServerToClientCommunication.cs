using System.Collections;
using System.Collections.Generic;
using Enums;

namespace Networking.Interfaces
{
    public interface ServerToClientCommunication
    {
        /// <summary>
        /// Pre game: send a notification to all clients that a new client has joined the game.
        /// </summary>
        /// <param name="playerInformation">contains {{playerName, playerColor, isReady}, {...}, ...}</param>
        public void notifyClientJoined(ArrayList playerInformation, string lobbyIP);


        /// <summary>
        /// Game start: when the game starts send the board to all clients.
        /// </summary>
        /// <param name="gameBoard">Game board</param>
        public void gamestartInitialize(Hexagon[][] gameBoard);


        /// <summary>
        /// In game: when other player built something, send this information to clients.
        /// </summary>
        /// <param name="buildType">type of building</param>
        /// <param name="buildID">ID of position on the board</param>
        /// <param name="color">color of building owner</param>
        public void notifyObjectPlacement(BUYABLES buildType, int buildID, PLAYERCOLOR color);


        /// <summary>
        /// Notify all players who the next player is.
        /// </summary>
        public void notifyNextPlayer(int playerIndex, int previousPlayerIndex);


        /// <summary>
        /// Tell all clients the winner of the game.
        /// </summary>
        /// <param name="playerName">winner name</param>
        /// <param name="playerColor">winner color</param>
        public void notifyVictory(string playerName, PLAYERCOLOR playerColor);


        // /// <summary>
        // /// Pass trade request to all players.
        // /// </summary>
        // /// <param name="offer">offer of requesting player</param>
        // /// <param name="expectation">expectation of resources</param>
        // /// <param name="playerColor">requesting player color</param>
        // /// <param name="playerName">requesting player name</param>
        // public void playerToPlayerTradeRequest(int[] offer, int[] expectation, string playerColor, string playerName);


        /// <summary>
        /// notify all clients that a nother client has disconnected
        /// </summary>
        /// <param name="playerName">disconnected player name</param>
        /// <param name="playerColor">disconnected player color</param>
        public void notifyClientDisconnect(string playerName, PLAYERCOLOR playerColor);


        // return requested information/resources ------------------------

        /// <summary>
        /// Return error message to client.
        /// </summary>
        /// <param name="errorMessage">Message(String) describing the problem/error</param>
        public void notifyRejection(int playerID, string errorMessage);



        /// <summary>
        /// Tell the clients the new state of a player.
        /// </summary>
        /// <param name="currentClientID">ID of the client</param>
        /// <param name="playerName">playerName whose status changes</param>
        /// <param name="readyStatus">The players ready status</param>
        public void notifyPlayerReady(int currentClientID, string playerName, bool readyStatus);


        /// <summary>
        /// Returns the dice result to all clients.
        /// </summary>
        /// <param name="diceResult">two integer numbers as dice1 and dice2</param>
        public void notifyRollDice(int[] diceResult);


        /// <summary>
        /// Player is allowed to buy a development card and everybody is notified, how many devCards are left
        /// </summary>
        /// <param name="leftDevCards">how many devCards are left</param>
        public void acceptBuyDevelopement(int leftDevCards);


        /// <summary>
        /// Notify all players that a player played a developement card
        /// </summary>
        /// <param name="playerID">Player who has to be told that he can play his Card</param>
        /// <param name="developmentCard">played development card type</param>
        /// <param name="playerName">Player who played the developement card</param>

        public void notifyAcceptPlayDevelopement(int playerID, DEVELOPMENT_TYPE developmentCard, string playerName);

        /// <summary>
        /// Notify current player that the trade offer is accepted
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="buttonNumber"></param>
        public void notifyAcceptTradeOffer(int playerID, int buttonNumber);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateLeftBuildings"></param>
        /// <param name="updateResources"></param>
        /// <param name="updateDevCards"></param>
        /// <param name="playerID"></param>
        public void updateOwnPlayer(int[] updateLeftBuildings, Dictionary<RESOURCETYPE, int> updateResources,
            Dictionary<DEVELOPMENT_TYPE, int> updateDevCards, int playerID);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateNumbers"></param>
        public void updateRepPlayers(int[][] updateNumbers);
    }
}