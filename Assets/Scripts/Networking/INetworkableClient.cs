using System;

namespace Networking
{
    public interface INetworkableClient
    {
        public delegate void acceptCallback(object acceptResult);

        public delegate void rejectCallback(object acceptResult);


        // Phase: 1 (roll dice + Raw material yields + what ever happens here ...)
        public void requestBeginRound(acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        // Phase: 2 (trade)
        public void requestTradeBank(acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        public void requestTradePort(acceptCallback acceptCallback, rejectCallback rejectCallback);

        
        // Phase: 3 (build)
        public void requestBuild(EnumBuyables type, int x, int y, acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        public void requestBuyDevelopement(acceptCallback acceptCallback, rejectCallback rejectCallback);

        public void requestPlayDevelopement(acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        
        // End phase
        public void requestEndTurn(acceptCallback acceptCallback, rejectCallback rejectCallback);


    }
}