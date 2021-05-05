namespace Networking
{
    public class Packet
    {
        public int type { get; set; } // what method needs to be called?
        public string myPlayerName { get; set; }
        public string myPlayerColor { get; set; }
        public int myPlayerNumber { get; set; }
        
        public string currentPlayerName { get; set; } //target of actions or current player
        public int[][] gameBoard { get; set; }
        
        public int[] diceResult { get; set; }
        public int[] resourcesObtained { get; set; } // [+ gain resources, - spent resources]
        
        public int developementCard { get; set; } // ID
        
        public int[] tradeResourcesOffer { get; set; } // what i want to spent [0,0,0,0,0]
        public int[] tradeResourcesExpect { get; set; } // resources i want [0,0,0,0,0]
        
        public int[] buildPosition { get; set; } // (x,y)
        public int buildType { get; set; } // building i want to build
        
        public int victoryPoint { get; set; }
        
        public string errorMessage { get; set; } // when client request is rejected
    }
}