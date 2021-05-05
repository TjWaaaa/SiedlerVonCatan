namespace Networking
{
    public interface INetworkableServer
    {


        // Phase: 1 (roll dice + Raw material yields + what ever happens here ...)
        /// <summary>
        /// React to client call requestBeginRound(). Check conditions, roll dice, handle resouce cards and return result
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        /// <param name="acceptCallback">method called if request was accepted. Send diece result and resource cards. Update all other client resources</param>
        /// <param name="rejectCallback">client is not allowed to begin the round. Send error message</param>
        public void handleBeginRound(Packet clientPacket);
        
        
        // Phase: 2 (trade)
        /// <summary>
        /// React to client call requestTradeBank(). If client funds are sufficient allow trade and update resources
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        /// <param name="acceptCallback">method called if request was accepted. Updated client resources</param>
        /// <param name="rejectCallback">client is not allowed to trade or the resources ar wrong. Send error message</param>
        public void handleTradeBank(Packet clientPacket);
        
        
        /// <summary>
        /// React to client call requestTradePort(). If client funds are sufficient allow trade.
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        /// <param name="acceptCallback">>method called if request was accepted.</param>
        /// <param name="rejectCallback"></param>
        //public void handleTradePort(Packet clientPacket);

        
        // Phase: 3 (build)
        /// <summary>
        /// React to client call requestBuild(). If client funds are sufficient allow placement of building. 
        /// </summary>
        /// <param name="clientPacket">Information of requesting client</param>
        /// <param name="acceptCallback">Method called if request was accepted. Send build information</param>
        /// <param name="rejectCallback">Method called if request was rejected.</param>
        public void handleBuild(Packet clientPacket);
        
        
        /// <summary>
        /// React to client call requestBuyDevelopement(). If client funds are sufficient allow purchase of development card. 
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        /// <param name="acceptCallback">method called if request was accepted. Send development card</param>
        /// <param name="rejectCallback">Method called if request was rejected. Send error message</param>
        public void handleBuyDevelopement(Packet clientPacket);

        
        /// <summary>
        /// React to client call requestPlayDevelopement(). Check if client is allowed.
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        /// <param name="acceptCallback">method called if request was accepted. Notify all other players</param>
        /// <param name="rejectCallback">Method called if request was rejected. Send error message</param>
        public void handlePlayDevelopement(Packet clientPacket);
        
        
        // End phase
        /// <summary>
        /// React to client call requestEndTurn(). Player ends his turn, call next player to start his round
        /// </summary>
        /// <param name="clientPacket">information of requesting client</param>
        /// <param name="acceptCallback">method called if request was accepted. </param>
        /// <param name="rejectCallback">Method called if request was rejected. Send error message</param>
        public void handleEndTurn(Packet clientPacket);

        
        /// <summary>
        /// If server noticed a client disconnect this method is called.
        /// </summary>
        public void handleClientDisconnect();
    }
}