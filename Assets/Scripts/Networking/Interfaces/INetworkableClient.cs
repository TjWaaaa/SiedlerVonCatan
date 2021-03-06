using Networking.Package;

namespace Networking.Interfaces
{
    public interface INetworkableClient
    {
        /// <summary>
        /// A new client joined -> update the UI
        /// </summary>
        /// <param name="serverPacket">New client information</param>
        public void handleClientJoined(Packet serverPacket);

        /// <summary>
        /// A player changed his/her ready status -> search UI for corresponding object -> update.
        /// </summary>
        /// <param name="serverPacket">Playername, readyStatus</param>
        public void handlePlayerReadyNotification(Packet serverPacket);


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
        /// <param name="serverPacket">Name of next Player</param>
        public void handleNextPlayer(Packet serverPacket);


        /// <summary>
        /// The winner of the game is announced -> display winner in UI
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleVictory(Packet serverPacket);


        // /// <summary>
        // /// Handle incoming trade request from other player
        // /// </summary>
        // /// <param name="serverPacket">Packet from server</param>
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
        /// The trade offer request was approved.
        /// </summary>
        /// <param name="clientPacket"></param>
        public void handleAcceptTradeOffer(Packet clientPacket);


        // /// <summary>
        // /// Server returns result of trade
        // /// </summary>
        // /// <param name="serverPacket">Packet from server</param>
        //public void handleAcceptTradePort(Packet serverPacket);


        /// <summary>
        /// Server returns type of Development card
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleAcceptBuyDevelopement(Packet serverPacket);


        /// <summary>
        /// Server returns result of Development card
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleAcceptPlayDevelopement(Packet serverPacket);


        /// <summary>
        /// Server sends a updateRepresentativePlayer packet. Updates all representative players ingame
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleUpdateRP(Packet serverPacket);

        /// <summary>
        /// Server sends a updateOwnPlayer packet. Updates only the own player object
        /// </summary>
        /// <param name="serverPacket">Packet from server</param>
        public void handleUpdateOP(Packet serverPacket);
    }
}