using Resource;

namespace Networking
{
    public interface ClientToServerCommunication
    {
        /// <summary>
        /// Function Pointer: Request was accepted. Handle new information 
        /// -> this is only a placeholder, there will be more methods needed
        /// </summary>
        public delegate void acceptCallback(Packet acceptResult);

        
        /// <summary>
        /// Function Pointer: Request was not accepted. Return error message.
        /// -> this is only a placeholder, there will be more methods needed (maybe not)
        /// </summary>
        public delegate void rejectCallback(Packet acceptResult, string errorMessage);


        // Phase: 1 (roll dice + Raw material yields + what ever happens here ...)
        /// <summary>
        /// Request if a player is allowed to begin a round.
        /// </summary>
        /// <param name="acceptCallback">Player is allowed. Method is called with answer from server as parameter.</param>
        /// <param name="rejectCallback">Player is not allowed. Method is called with error message as parameter.</param>
        public void requestBeginRound(acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        // Phase: 2 (trade)
        /// <summary>
        /// Requests if a player is allowed to trade a certain resource with the bank.
        /// </summary>
        /// <param name="acceptCallback">Method is called if the player is allowed.</param>
        /// <param name="rejectCallback">Method is called if the player is not allowed.</param>
        public void requestTradeBank(RESOURCE offer, RESOURCE expectation, acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        
        /// <summary>
        /// Requests if a player is allowed to trade a certain resource with a port.
        /// </summary>
        /// <param name="acceptCallback">Method is called if the player is allowed.</param>
        /// <param name="rejectCallback">Method is called if the player is not allowed.</param>
        // public void requestTradePort(acceptCallback acceptCallback, rejectCallback rejectCallback);

        
        // Phase: 3 (build)
        /// <summary>
        /// Request to build a building on the board
        /// </summary>
        /// <param name="type">type of building</param>
        /// <param name="x">x position on board</param>
        /// <param name="y">y position on board</param>
        /// <param name="acceptCallback">Player is allowed to build. Method is called with answer from server as parameter.</param>
        /// <param name="rejectCallback">Player is not allowed to build there. Method is called with error message as parameter.</param>
        public void requestBuild(BUYABLES type, int x, int y, acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        
        /// <summary>
        /// Request to buy a development card
        /// </summary>
        /// <param name="acceptCallback">Player is allowed to purchase card. Method is called with answer from server as parameter.</param>
        /// <param name="rejectCallback">Player is not allowed to purchase card. Method is called with error message as parameter.</param>
        public void requestBuyDevelopement(acceptCallback acceptCallback, rejectCallback rejectCallback);

        
        /// <summary>
        /// Request to play a development card
        /// </summary>
        /// <param name="acceptCallback">Player is allowed to play card. Method is called with answer from server as parameter.</param>
        /// <param name="rejectCallback">Player is not allowed to play card. Method is called with error message as parameter</param>
        public void requestPlayDevelopement(acceptCallback acceptCallback, rejectCallback rejectCallback);
        
        
        // End phase
        /// <summary>
        /// Client wants to end his turn
        /// </summary>
        /// <param name="acceptCallback">Player is allowed to end turn. Method is called with answer from server as parameter.</param>
        /// <param name="rejectCallback">Player is not allowed end turn now. Method is called with error message as parameter/param>
        public void requestEndTurn(acceptCallback acceptCallback, rejectCallback rejectCallback);
    }
}