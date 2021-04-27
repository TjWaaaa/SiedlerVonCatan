using System;

namespace Networking
{
    public interface NetworkableClient
    {
        public delegate void acceptCallback(IAsyncResult acceptPromise);

        public delegate void rejectCallback(IAsyncResult rejectPromise);


        // Phase: 1 (Raw material yields)
        public void requestRollDice(acceptCallback acceptCallback, rejectCallback rejectCallback);
        
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