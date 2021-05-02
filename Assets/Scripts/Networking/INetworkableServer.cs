namespace Networking
{
    public interface INetworkableServer
    {
        public delegate void acceptCallback(Packet acceptResult);

        public delegate void rejectCallback(Packet acceptResult);


        // Phase: 1 (roll dice + Raw material yields + what ever happens here ...)
        public void handleBeginRound(Packet clientPacket, acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        // Phase: 2 (trade)
        public void handleTradeBank(Packet clientPacket, acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        public void handleTradePort(Packet clientPacket, acceptCallback acceptCallback, rejectCallback rejectCallback);

        
        // Phase: 3 (build)
        public void handleBuild(Packet clientPacket, acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        public void handleBuyDevelopement(Packet clientPacket, acceptCallback acceptCallback, rejectCallback rejectCallback);

        public void handlePlayDevelopement(Packet clientPacket, acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        
        // End phase
        public void handleEndTurn(Packet clientPacket, acceptCallback acceptCallback, rejectCallback rejectCallback);

    }
}