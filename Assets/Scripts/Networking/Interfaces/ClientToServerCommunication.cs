using Enums;

namespace Enums
{
    public interface ClientToServerCommunication
    {
        /// <summary>
        /// client requests to join lobby with his selected name.
        /// </summary>
        /// <param name="playerName">Name of the player</param>
        public void requestJoinLobby(string playerName);

        
        /// <summary>
        /// tell the server if th client is ready to play
        /// </summary>
        /// <param name="isReady">true -> i'm ready | false -> i'm not ready</param>
        public void requestPlayerReady(bool isReady);
        
        // Phase: 1 (roll dice + Raw material yields + what ever happens here ...)
        /// <summary>
        /// Request if a player is allowed to begin a round and able to toll the dice.
        /// </summary>
        public void requestRollDice();
        
        // Phase: 2 (trade)
        /// <summary>
        /// Requests if a player is allowed to trade a certain resource with the bank.
        /// </summary>
        public void requestTradeBank(int[] offer, int[] expect);
        
        
        // /// <summary>
        // /// Requests if a player is allowed to trade a certain resource with a port.
        // /// </summary>
        // public void requestTradePort();

        
        // Phase: 3 (build)
        /// <summary>
        /// Request to build a building on the board
        /// </summary>
        /// <param name="buildType">type of building</param>
        /// <param name="buildID">ID of building</param>
        public void requestBuild(BUYABLES buildType, int buildID);
        
        
        /// <summary>
        /// Request to buy a development card
        /// </summary>
        public void requestBuyDevelopement();

        
        /// <summary>
        /// Request to play a development card
        /// </summary>
        public void requestPlayDevelopement(DEVELOPMENT_TYPE developmentType);
        
        
        // End phase
        /// <summary>
        /// Client wants to end his turn
        /// </summary>
        public void requestEndTurn();
    }
}