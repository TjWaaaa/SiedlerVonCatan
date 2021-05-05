using System;

namespace Networking
{
    public interface INetworkableClient
    {
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
        /// Client is informed who the next player is.
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
        /// Handle incoming message that a player has disconnected -> display in UI
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleClientDisconnect(Packet serverPacket);
        
        
        // Answers from previous Requests -------------------------------------------
        
        /// <summary>
        /// Universal method for a rejection of any client request.
        /// </summary>
        /// <param name="serverPacket">Incoming Packet from server</param>
        public void handleRejection(Packet serverPacket);
        
        
        /// <summary>
        /// Server accepts begin round request and sends the dice result.
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleAccpetBeginRound(Packet serverPacket);

        
        /// <summary>
        /// The trade request was approved.
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleAcceptTradeBank(Packet serverPacket);
        
        
        /// <summary>
        /// Server returns result of trde
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        //public void handleAcceptTradePort(Packet serverPacket);

        
        /// <summary>
        /// Server returns the new building with the position
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleAcceptBuild(Packet serverPacket);
        
        
        /// <summary>
        /// Server returns the new resources. Updates clients resources
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleGetResources(Packet serverPacket);
        
        
        /// <summary>
        /// Server returns result of Development card
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleAcceptPlayDevelopement(Packet serverPacket);
        
        
        /// <summary>
        /// Client is told that his tourn has ended
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleAcceptEndTurn(Packet serverPacket);
    }
}