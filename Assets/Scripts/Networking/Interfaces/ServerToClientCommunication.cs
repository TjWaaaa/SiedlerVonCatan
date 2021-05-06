namespace Networking
{
    public interface ServerToClientCommunication
    {
        /// <summary>
        /// Pre game: send a notification to all clients that a new client has joined the game.
        /// </summary>
        /// <param name="playerID">id of new player</param>
        /// <param name="playerName">name of new player</param>
        /// <param name="color">color of new player</param>
        public void notifyClientJoined(int playerID, string playerName, string color);

        
        /// <summary>
        /// Game start: when the game starts send the board to all clients.
        /// </summary>
        /// <param name="gameBoard"></param>
        public void gamestartInitialize(int[][] gameBoard);


        /// <summary>
        /// Send new resources to player
        /// </summary>
        /// <param name="resources">[+ gain resources, - spent resources]</param>
        public void distributeResources(int playerID, int[] resources); 

        
        /// <summary>
        /// In game: when other player built something, send this information to clients.
        /// </summary>
        /// <param name="type">type of building</param>
        /// <param name="x">x coordinate on board</param>
        /// <param name="y">y coordinate on board</param>
        /// <param name="color">color of building owner</param>
        public void notifyObjectPlacement(BUYABLES buildType, int buildID, string color);

        
        /// <summary>
        /// Move control to the next player.
        /// </summary>
        public void notifyNextPlayer(string name);

        
        /// <summary>
        /// Tell all clients the winner of the game.
        /// </summary>
        /// <param name="playerID">winner id</param>
        /// <param name="PlayerName">winner name</param>
        /// <param name="color">winner color</param>
        public void notifyVictory(int playerID, string PlayerName, string color);

        /// <summary>
        /// Pass trade request to all players.
        /// </summary>
        /// <param name="offer">offer of requesting player</param>
        /// <param name="expectation">expectation of resources</param>
        /// <param name="playerColor">requesting player color</param>
        /// <param name="playerName">requesting player name</param>
        // public void playerToPlayerTradeRequest(int[] offer, int[] expectation, string playerColor, string playerName);


        /// <summary>
        /// notify all clients that a nother client has disconnected
        /// </summary>
        /// <param name="playerName">disconnected player name</param>
        /// <param name="color">disconnected player color</param>
        public void notifyClientDisconnect(string playerName, string color);
        

        // return requested information/resources -------------
        
        public void notifyRejection(string errorMessage);


        public void notifyAccpetBeginRound(int[] diceResult);


        public void notifyAcceptTradeBank();


        public void notifyAcceptBuild();

        
        public void notifyAcceptGetResources();


        public void notifyAcceptPlayDevelopement();

        
        public void notifyAcceptEndTurn();
    }
}