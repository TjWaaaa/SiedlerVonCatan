using UnityEngine;

namespace Player
{
    public class RepresentativePlayer
    {
        public int playerID;
        private string playerName;
        private Color playerColor;

        private int victoryPoints;
        private int totalResourceAmount;
        private int devCardAmount;


        public RepresentativePlayer(int playerID, string playerName, Color playerColor)
        {
            this.playerID = playerID;
            this.playerName = playerName;
            this.playerColor = playerColor;
        }

        // getter

        public Color getPlayerColor()
        {
            return playerColor;
        }

        public string getPlayerName()
        {
            return playerName;
        }

        public int getVictoryPoints()
        {
            return victoryPoints;
        }

        public int getTotalResourceAmount()
        {
            return totalResourceAmount;
        }

        public int getDevCardAmount()
        {
            return devCardAmount;
        }

        public int getPlayerID()
        {
            return playerID;
        }

        // setter
        public void updateNumbers(int[] updates)
        {
            victoryPoints = updates[0];
            totalResourceAmount = updates[1];
            devCardAmount = updates[2];
        }
    }
}