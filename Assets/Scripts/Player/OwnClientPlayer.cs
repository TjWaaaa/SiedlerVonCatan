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



        public void updateOP(int[] updateNumbers, Dictionary<RESOURCETYPE, int> updateResources, Dictionary<DEVELOPMENT_TYPE, int> updateDevCards)
        {
            leftStreets = updateNumbers[0];
            leftVillages = updateNumbers[1];
            leftCitys = updateNumbers[2];

            resources = updateResources;
            devCards = updateDevCards;

        }

    }
}