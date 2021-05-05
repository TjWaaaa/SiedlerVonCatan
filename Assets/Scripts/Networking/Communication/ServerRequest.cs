namespace Networking.Communication
{
    public class ServerRequest : ServerToClientCommunication
    {
        public void notifyClientJoined(int playerID, string playerName, string color)
        {
            throw new System.NotImplementedException();
        }

        public void gamestartInitialize(int[][] gameBoard)
        {
            throw new System.NotImplementedException();
        }

        public void notifyObjectPlacement(BUYABLES type, int x, int y, string color)
        {
            throw new System.NotImplementedException();
        }

        public void notifyNextPlayer()
        {
            throw new System.NotImplementedException();
        }

        public void notifyVictory(int playerID, string PlayerName, string color)
        {
            throw new System.NotImplementedException();
        }

        public void notifyClientDisconnect(string playerName, string color)
        {
            throw new System.NotImplementedException();
        }
    }
}