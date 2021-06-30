using System.Collections;
using System.Collections.Generic;
using Enums;

namespace Networking.Package
{
    public class Packet
    {
        public int type { get; set; } // what method needs to be called? -> set default to -1 to prevent wrong messages
        public string playerName { get; set; }
        public PLAYERCOLOR playerColor { get; set; } // [r,g,b,a]
        public int myPlayerID { get; set; } // player ID of the client who receives the packet
        
        public int? currentPlayerID { get; set; } // ID of the current player
        public int? previousPlayerID { get; set; } // ID of the previous player
        
        public bool isReady { get; set; } // is a player ready or not (lobby only)
        
        public string lobbyIP { get; set; }
        
        public ArrayList lobbyContent { get; set; } // {{PlayerName, PlayerColor, isReady}}
        
        public string currentPlayerName { get; set; } //target of actions or current player
        public Hexagon[][] gameBoard { get; set; }
        
        public int[] diceResult { get; set; }
        public int[] resourcesObtained { get; set; } // [+ gain resources, - spent resources]
        
        public DEVELOPMENT_TYPE developmentCard { get; set; } // ID
        
        public int[] tradeResourcesOffer { get; set; } // what i want to spent [0,0,0,0,0]
        public int[] tradeResourcesExpect { get; set; } // resources i want [0,0,0,0,0]
        
        public int? buildID { get; set; }
        public int? buildType { get; set; } // building i want to build
        public PLAYERCOLOR buildColor { get; set; } // color of building
        
        
        public int? victoryPoint { get; set; }
        
        public string errorMessage { get; set; } // when client request is rejected

        public int[][] updateRP { get; set; }

        public int[] updateOP {get; set; }

        public Dictionary<RESOURCE_TYPE, int> updateResourcesOnOP {get; set;}
        public Dictionary<DEVELOPMENT_TYPE, int> updateDevCardsOnOP {get; set;}
        
        public int? resourceType { get; set; }
        
        public int? buttonNumber { get; set; }
        
        public int? leftDevCards { get; set; }
    }
}