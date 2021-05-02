﻿namespace Networking
{
    public interface ServerToClientCommunication
    {
        public delegate void acceptCallback(Packet acceptResult);

        public delegate void rejectCallback(Packet acceptResult, string errorMessage);
        
        /// <summary>
        /// Pre game: send a notification to all clients that a new client has joined the game.
        /// </summary>
        /// <param name="playerID">id of new player</param>
        /// <param name="playerName">name of new player</param>
        /// <param name="color">color of new player</param>
        public void notifyClientJoined(int playerID, string playerName, string color);

        
        /// <summary>
        /// Game start: when the game starts send the board to all clients.
        /// </summary>
        /// <param name="gameBoard"></param>
        public void gamestartInitialize(int[][] gameBoard);

        
        /// <summary>
        /// In game: when other player built something, send this information to clients.
        /// </summary>
        /// <param name="type">type of building</param>
        /// <param name="x">x coordinate on board</param>
        /// <param name="y">y coordinate on board</param>
        /// <param name="color">color of building owner</param>
        public void notifyObjectPlacement(BUYABLES type, int x, int y, string color);

        
        /// <summary>
        /// Move control to the next player.
        /// </summary>
        public void notifyNextPlayer();

        
        /// <summary>
        /// Tell all clients the winner of the game.
        /// </summary>
        /// <param name="playerID">winner id</param>
        /// <param name="PlayerName">winner name</param>
        /// <param name="color">winner color</param>
        public void notifyVictory(int playerID, string PlayerName, string color);

        /// <summary>
        /// Pass trade request to all players.
        /// </summary>
        /// <param name="offer">offer of requesting player</param>
        /// <param name="expectation">expectation of resources</param>
        /// <param name="playerColor">requesting player color</param>
        /// <param name="playerName">requesting player name</param>
        // public void playerToPlayerTradeRequest(int[] offer, int[] expectation, string playerColor, string playerName);

        
        /// <summary>
        /// Regular Ping that is sent to all clients to find out if all clients are still connected.
        /// </summary>
        public void keepAlivePing(acceptCallback acceptCallback, rejectCallback rejectCallback);
    }
}