using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class ServerPlayer
    {
        private string playerName;
        private Color playerColor;
        private int playerID;
        private bool isReady;
        private int victoryPoints;
        private int leftStreets = 15;
        private int leftVillages = 5;
        private int leftCitys = 4;
        private int devCardAmount = 0;

        private Dictionary<RESOURCETYPE, int> resources = new Dictionary<RESOURCETYPE, int>
        {
            {RESOURCETYPE.SHEEP, 0},
            {RESOURCETYPE.WOOD, 0},
            {RESOURCETYPE.BRICK, 0},
            {RESOURCETYPE.ORE, 0},
            {RESOURCETYPE.WHEAT, 0}

        };

        public ServerPlayer(int playerID)
        {
            this.playerID = playerID;
        }

        // TODO: remove this one, only use the upper one with the id!!!
        public ServerPlayer(string playerName, Color color)
        {
            this.playerName = playerName;
            this.playerColor = color;
        }


        // Getter

        public int getPlayerID()
        {
            return playerID;
        }

        public Color getPlayerColor()
        {
            return playerColor;
        }

        public string getPlayerName()
        {
            return playerName;
        }

        public bool getIsReady()
        {
            return isReady;
        }

        public int getResourceAmount(RESOURCETYPE resourcetype)
        {
            return resources[resourcetype];
        }

        public int getTotalResourceAmount()
        {
            int amount = 0;
            foreach (var res in resources)
            {
                amount += res.Value;
            }

            return amount;
        }

        // Setter

        public void setPlayerColor(Color color)
        {
            this.playerColor = color;
        }

        public void setPlayerName(string name)
        {
            this.playerName = name;
        }

        public void setIsReady(bool isReady)
        {
            this.isReady = isReady;
        }

        public void setResourceAmount(RESOURCETYPE resourcetype, int amount)
        {
            resources[resourcetype] += amount;
        }



        // Trade

        public Boolean canTrade(RESOURCETYPE resourcetype)
        {
            if (resources[resourcetype] >= 4)
            {
                return true;
            }
            else
            {
                Debug.Log("only " + resources[resourcetype] + " of " + resourcetype);
                return false;
            }

            //should only return true if there are at least 4
        }

        public void trade(RESOURCETYPE offerResourcetype, RESOURCETYPE expectResourcetype)
        {
            resources[offerResourcetype] -= 4;
            resources[expectResourcetype] += 1;
        }


        // Buy

        public Boolean canBuyStreet()
        {
            if (resources[RESOURCETYPE.BRICK] >= 1 && resources[RESOURCETYPE.WOOD] >= 1)
                return true;
            else return false;
        }

        public Boolean canBuyVillage()
        {
            if (resources[RESOURCETYPE.BRICK] >= 1
                && resources[RESOURCETYPE.WOOD] >= 1
                && resources[RESOURCETYPE.SHEEP] >= 1
                && resources[RESOURCETYPE.WHEAT] >= 1)
                return true;
            else return false;
        }

        public Boolean canBuyCity()
        {
            if (resources[RESOURCETYPE.ORE] >= 3
                && resources[RESOURCETYPE.WHEAT] >= 2)
                return true;
            else return false;
        }

        public Boolean canBuyDevCard()
        {
            if (resources[RESOURCETYPE.ORE] >= 1
                && resources[RESOURCETYPE.WHEAT] >= 1
                && resources[RESOURCETYPE.SHEEP] >= 1)
                return true;
            else return false;
        }


        public void buyStreet()
        {
            resources[RESOURCETYPE.BRICK] -= 1;
            resources[RESOURCETYPE.WOOD] -= 1;
        }

        public void buyVillage()
        {
            resources[RESOURCETYPE.BRICK] -= 1;
            resources[RESOURCETYPE.WOOD] -= 1;
            resources[RESOURCETYPE.SHEEP] -= 1;
            resources[RESOURCETYPE.WHEAT] -= 1;
        }

        public void buyCity()
        {
            resources[RESOURCETYPE.ORE] -= 3;
            resources[RESOURCETYPE.WHEAT] -= 2;
        }

        public void buyDevCard()
        {
            resources[RESOURCETYPE.ORE] -= 1;
            resources[RESOURCETYPE.WHEAT] -= 1;
            resources[RESOURCETYPE.SHEEP] -= 1;
        }

        public int[] convertFromSPToRP()
        {
            return new int[] {victoryPoints, getTotalResourceAmount(), devCardAmount };
        }
    }
}