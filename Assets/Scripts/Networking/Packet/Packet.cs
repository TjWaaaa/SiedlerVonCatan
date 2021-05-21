using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Networking.Package
{
    public class Packet
    {
        public int type { get; set; } // what method needs to be called? -> set default to -1 to prevent wrong messages
        public string playerName { get; set; }
        public float[] playerColor { get; set; } // [r,g,b,a]
        public int playerNumber { get; set; } // player ID of the client who receives the packet
        
        public int currentPlayerNumber { get; set; }
        
        public bool isReady { get; set; } // is a player ready or not (lobby only)
        
        public ArrayList lobbyContent { get; set; } // {{PlayerName, PlayerColor, isReady}}
        
        public string currentPlayerName { get; set; } //target of actions or current player
        public int[][] gameBoard { get; set; }
        
        public int[] diceResult { get; set; }
        public int[] resourcesObtained { get; set; } // [+ gain resources, - spent resources]
        
        public int developmentCard { get; set; } // ID
        
        public int[] tradeResourcesOffer { get; set; } // what i want to spent [0,0,0,0,0]
        public int[] tradeResourcesExpect { get; set; } // resources i want [0,0,0,0,0]
        
        public int buildID { get; set; }
        public int buildType { get; set; } // building i want to build
        public string buildColor { get; set; } // color of building
        
        
        public int victoryPoint { get; set; }
        
        public string errorMessage { get; set; } // when client request is rejected
    }
}