using Networking.Package;

namespace Networking.Interfaces
{
    public interface INetworkableServer
    {
        /// <summary>
        /// React to client call requestJoinLobby(). Update Player information
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>u
        /// <param name="currentClientID">id of current client</param>
        public void handleRequestJoinLobby(Packet clientPacket, int currentClientID);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientPacket"></param>
        public void handleRequestPlayerReady(Packet clientPacket, int currentClientID);

        // Phase: 1 (roll dice + Raw material yields + what ever happens here ...)
        /// <summary>
        /// React to client call requestBeginRound(). Check conditions, roll dice, handle resouce cards and return result
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        public void handleBeginRound(Packet clientPacket);


        // Phase: 2 (trade)
        /// <summary>
        /// React to client call requestTradeBank(). If client funds are sufficient allow trade and update resources
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        public void handleTradeBank(Packet clientPacket);


        /// <summary>
        /// Checks if player has enough resources to offer
        /// </summary>
        /// <param name="clientPacket"></param>
        public void handleTradeOffer(Packet clientPacket);


        // /// <summary>
        // /// React to client call requestTradePort(). If client funds are sufficient allow trade.
        // /// </summary>
        // /// <param name="clientPacket">information of requesting client</param>
        //public void handleTradePort(Packet clientPacket);


        // Phase: 3 (build)
        /// <summary>
        /// React to client call requestBuild(). If client funds are sufficient allow placement of building. 
        /// </summary>
        /// <param name="clientPacket">Information of requesting client</param>
        public void handleBuild(Packet clientPacket);


        /// <summary>
        /// React to client call requestBuyDevelopement(). If client funds are sufficient allow purchase of development card. 
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        public void handleBuyDevelopement(Packet clientPacket);


        /// <summary>
        /// React to client call requestPlayDevelopement(). Check if client is allowed.
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        public void handlePlayDevelopement(Packet clientPacket);


        // End phase
        /// <summary>
        /// React to client call requestEndTurn(). Player ends his turn, call next player to start his round
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        public void handleEndTurn(Packet clientPacket);


        /// <summary>
        /// If server noticed a client disconnect this method is called.
        /// </summary>
        public void handleClientDisconnectServerCall(int disconnectedClientID);


        /// <summary>
        /// Generates a new player object with a playerID. 
        /// </summary>
        /// <param name="playerId">ID of the new player</param>
        public void generatePlayer(int playerId);
    }
}