using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Player
{
    public class OwnClientPlayer
    {
        private int playerID;
        
        private int leftStreets = 15;
        private int leftVillages = 5;
        private int leftCitys = 4;
        private Dictionary<RESOURCETYPE, int> resources = new Dictionary<RESOURCETYPE, int>
        {
            {RESOURCETYPE.SHEEP, 0},
            {RESOURCETYPE.ORE, 0},
            {RESOURCETYPE.BRICK, 0},
            {RESOURCETYPE.WOOD, 0},
            {RESOURCETYPE.WHEAT, 0}
        };

        public OwnClientPlayer(int playerID)
        {
            this.playerID = playerID;
        }
        
        
        // getter
                
                public int getResourceAmount(RESOURCETYPE resourcetype)
                        {
                            return resources[resourcetype];
                        }
                
                public int getLeftStreets()
                {
                    return leftStreets;
                }
                
                public int getLeftVillages()
                {
                    return leftVillages;
                }
                
                public int getLeftCitys()
                {
                    return leftCitys;
                }
                
                
        // setter
        
        // maybe there is a better update method later on
        public void setResourceAmount(RESOURCETYPE resourcetype, int amount)
                {
                    resources[resourcetype] += amount;
                }

        public void setLeftStreets(int leftStreets)
        {
            this.leftStreets = leftStreets;
        }
        
        public void setLeftVillages(int leftVillages)
        {
            this.leftVillages = leftVillages;
        }
        
        public void setLeftCitys(int leftCitys)
        {
            this.leftCitys = leftCitys;
        }
        
        
        
    }
}