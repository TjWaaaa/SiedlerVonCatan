using UnityEngine;

namespace Player
{
    public class RepresentativePlayer
    {
        private int playerID;
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
        
        // setter
        
        public void setVictoryPoints(int victoryPoints)
        {
            this.victoryPoints = victoryPoints;
        }
        
        public void setTotalResourceAmount(int totalResourceAmount)
        {
            this.totalResourceAmount = totalResourceAmount;
        }
        
        public void setDevCardAmount(int devCardAmount)
        {
            this.devCardAmount = devCardAmount;
        }

    }
}