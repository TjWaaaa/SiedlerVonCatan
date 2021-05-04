using Resource;

namespace Networking
{
    public interface ClientToServerCommunication
    {
        // Phase: 1 (roll dice + Raw material yields + what ever happens here ...)
        /// <summary>
        /// Request if a player is allowed to begin a round.
        /// </summary>
        public void requestBeginRound();
        
        // Phase: 2 (trade)
        /// <summary>
        /// Requests if a player is allowed to trade a certain resource with the bank.
        /// </summary>
        public void requestTradeBank();
        
        
        /// <summary>
        /// Requests if a player is allowed to trade a certain resource with a port.
        /// </summary>
        // public void requestTradePort(acceptCallback acceptCallback, rejectCallback rejectCallback);

        
        // Phase: 3 (build)
        /// <summary>
        /// Request to build a building on the board
        /// </summary>
        /// <param name="type">type of building</param>
        /// <param name="x">x position on board</param>
        /// <param name="y">y position on board</param>
        public void requestBuild(BUYABLES type, int x, int y);
        
        
        /// <summary>
        /// Request to buy a development card
        /// </summary>
        public void requestBuyDevelopement();

        
        /// <summary>
        /// Request to play a development card
        /// </summary>
        public void requestPlayDevelopement();
        
        
        // End phase
        /// <summary>
        /// Client wants to end his turn
        /// </summary>
        /// <param name="acceptCallback">Player is allowed to end turn. Method is called with answer from server as parameter.</param>
        /// <param name="rejectCallback">Player is not allowed end turn now. Method is called with error message as parameter/param>
        public void requestEndTurn();
    }
}