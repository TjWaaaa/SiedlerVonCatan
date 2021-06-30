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
        private Dictionary<DEVELOPMENT_TYPE, int> devCards = new Dictionary<DEVELOPMENT_TYPE, int>
        {
            {DEVELOPMENT_TYPE.VICTORY_POINT, 0},
            {DEVELOPMENT_TYPE.KNIGHT, 0},
            {DEVELOPMENT_TYPE.ROAD_BUILDING, 0},
            {DEVELOPMENT_TYPE.YEAR_OF_PLENTY, 0},
            {DEVELOPMENT_TYPE.MONOPOLY, 0}
        };
        private Dictionary<RESOURCE_TYPE, int> resources = new Dictionary<RESOURCE_TYPE, int>
        {
            {RESOURCE_TYPE.SHEEP, 0},
            {RESOURCE_TYPE.ORE, 0},
            {RESOURCE_TYPE.BRICK, 0},
            {RESOURCE_TYPE.WOOD, 0},
            {RESOURCE_TYPE.WHEAT, 0}
        };

        public OwnClientPlayer(int playerID)
        {
            this.playerID = playerID;
        }


        // getter

        public int getResourceAmount(RESOURCE_TYPE resourceType)
        {
            return resources[resourceType];
        }
        
        public int getDevCardAmount(DEVELOPMENT_TYPE type)
        {
            return devCards[type];
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

        

        public void updateOP(int[] updateNumbers, Dictionary<RESOURCE_TYPE, int> updateResources, Dictionary<DEVELOPMENT_TYPE, int> updateDevCards)
        {
            leftStreets = updateNumbers[0];
            leftVillages = updateNumbers[1];
            leftCitys = updateNumbers[2];

            resources = updateResources;
            devCards = updateDevCards;

        }

    }
}