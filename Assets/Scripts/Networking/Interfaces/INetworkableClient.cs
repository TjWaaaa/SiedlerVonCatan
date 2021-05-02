using System;

namespace Networking
{
    public interface INetworkableClient
    {
        /// <summary>
        /// Function Pointer: Request was accepted. Handle new information 
        /// -> this is only a placeholder, there will be more methods needed
        /// </summary>
        public delegate void acceptCallback(Packet acceptResult);

        // public delegate void rejectCallback(Packet acceptResult, string errorMessage);
        
        
        /// <summary>
        /// A new client joined -> update the UI
        /// </summary>
        /// <param name="serverPacket">New client information</param>
        public void handleClientJoined(Packet serverPacket);

        
        /// <summary>
        /// The game has started and the board has been sent -> render initial gameboard
        /// </summary>
        /// <param name="serverPacket">Initial gameboard</param>
        public void handleGameStartInitialize(Packet serverPacket);

        
        /// <summary>
        /// Another player hat built something -> display new building in UI
        /// </summary>
        /// <param name="serverPacket">New building and owner color</param>
        public void handleObjectPlacement(Packet serverPacket);

        
        /// <summary>
        /// Clinet is informed that he is the current player.
        /// </summary>
        public void handleNextPlayer();

        
        /// <summary>
        /// The winner of the game is announced -> display winner in UI
        /// </summary>
        /// <param name="playerID">ID of winning player</param>
        /// <param name="PlayerName">Name of winning player</param>
        /// <param name="color">Color of winning player</param>
        public void handleVictory(int playerID, string PlayerName, string color);

        /// <summary>
        /// Handle incoming trade request from other player
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        // public void hanldePlayerToPlayerTradeRequest(Packet serverPacket);

        
        /// <summary>
        /// Answer to the regular keep alive ping from server.
        /// </summary>
        public void handleKeepAlivePing(acceptCallback acceptCallback);


        /// <summary>
        /// Handle incoming message that a player has disconnected -> display in UI
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleClientDisconnect(Packet serverPacket);
    }
}