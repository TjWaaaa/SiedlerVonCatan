using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class ServerPlayer
    {
        private string playerName;
        private PLAYERCOLOR playerColor;
        private int playerID;
        private int victoryPoints;
        private int leftRoads = 15;
        private int leftVillages = 5;
        private int leftCities = 4;
        private bool isReady;
        
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

        public ServerPlayer(int playerID)
        {
            this.playerID = playerID;
        }

        


        // Getter

        public int getPlayerID()
        {
            return playerID;
        }

        public PLAYERCOLOR getPlayerColor()
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

        public int getResourceAmount(RESOURCE_TYPE resourceType)
        {
            return resources[resourceType];
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

        public int getTotalDevCardAmount()
        {
            int amount = 0;
            
            foreach (var card in devCards)
            {
                amount += card.Value;
            }

            return amount;
        }

        public int getLeftRoads()
        {
            return this.leftRoads;
        }

        public int getLeftVillages()
        {
            return this.leftVillages;
        }
        
        public int getVictoryPoints()
        {
            return victoryPoints;
        }

        // Setter

        public void setPlayerColor(PLAYERCOLOR playerColor)
        {
            this.playerColor = playerColor;
        }

        public void setPlayerName(string playerName)
        {
            this.playerName = playerName;
        }

        public void setIsReady(bool isReady)
        {
            this.isReady = isReady;
        }

        public void setResourceAmount(RESOURCE_TYPE resourceType, int amount)
        {
            resources[resourceType] += amount;
        }

        //Start phase
        public void reduceLeftRoads()
        {
            leftRoads--;
        }

        public void reduceLeftVillages()
        {
            leftVillages--;
            victoryPoints++;
        }

        public void reduceLeftCities()
        {
            leftCities--;
            leftVillages++;
            victoryPoints++;
        }

        // Trade
        public bool canTrade(RESOURCE_TYPE resourceType)
        {
            if (resources[resourceType] >= 4)
            {
                return true;
            }
            else
            {
                Debug.Log("CLIENT: You only have " + resources[resourceType] + resourceType.ToString().ToLower() + ". Trade something else.");
                return false;
            }
        }

        public void trade(int[] offer, int[] expect)
        {

            RESOURCE_TYPE offerRes = RESOURCE_TYPE.NONE;
            RESOURCE_TYPE expectRes = RESOURCE_TYPE.NONE;
            for (int i = 0; i < offer.Length; i++)
            {
                resources[resources.ElementAt(i).Key] -= offer[i];
                if (offer[i] > 0)
                {
                    offerRes = resources.ElementAt(i).Key;
                }
            }
            for (int i = 0; i < expect.Length; i++)
            {
                resources[resources.ElementAt(i).Key] += expect[i];
                if (expect[i] > 0)
                {
                    expectRes = resources.ElementAt(i).Key;
                }
            }
            Debug.Log("traded " + offerRes + " against " + expectRes);
        }

        // Buy
        public bool canBuyBuyable(BUYABLES buyable)
        {
            switch (buyable)
            {
                case BUYABLES.ROAD:
                    return leftRoads >= 1
                           && resources[RESOURCE_TYPE.WOOD] >= 1
                           && resources[RESOURCE_TYPE.BRICK] >= 1;
                case BUYABLES.VILLAGE:
                    return leftVillages >= 1
                           && resources[RESOURCE_TYPE.BRICK] >= 1
                           && resources[RESOURCE_TYPE.WOOD] >= 1
                           && resources[RESOURCE_TYPE.SHEEP] >= 1
                           && resources[RESOURCE_TYPE.WHEAT] >= 1;
                case BUYABLES.CITY:
                    return leftCities >= 1
                           && resources[RESOURCE_TYPE.ORE] >= 3
                           && resources[RESOURCE_TYPE.WHEAT] >= 2;
                case BUYABLES.DEVELOPMENT_CARDS:
                    return resources[RESOURCE_TYPE.ORE] >= 1
                           && resources[RESOURCE_TYPE.WHEAT] >= 1
                           && resources[RESOURCE_TYPE.SHEEP] >= 1;
                default: return false;
            }
        }

        public void buyBuyable(BUYABLES buyable)
        {
            switch (buyable)
            {
                case BUYABLES.ROAD:
                    resources[RESOURCE_TYPE.BRICK] -= 1;
                    resources[RESOURCE_TYPE.WOOD] -= 1;
                    break;
                case BUYABLES.VILLAGE:
                    resources[RESOURCE_TYPE.BRICK] -= 1;
                    resources[RESOURCE_TYPE.WOOD] -= 1;
                    resources[RESOURCE_TYPE.SHEEP] -= 1;
                    resources[RESOURCE_TYPE.WHEAT] -= 1;
                    break;
                case BUYABLES.CITY:
                    resources[RESOURCE_TYPE.ORE] -= 3;
                    resources[RESOURCE_TYPE.WHEAT] -= 2;
                    break;
                case BUYABLES.DEVELOPMENT_CARDS:
                    resources[RESOURCE_TYPE.ORE] -= 1;
                    resources[RESOURCE_TYPE.WHEAT] -= 1;
                    resources[RESOURCE_TYPE.SHEEP] -= 1;
                    break;
            }
        }

        public int[] convertFromSPToRP()
        {
            return new[] { victoryPoints, getTotalResourceAmount(), getTotalDevCardAmount() };
        }

        public int[] convertFromSPToOP()
        {
            return new[] { leftRoads, leftVillages, leftCities };
        }

        public Dictionary<RESOURCE_TYPE, int> convertSPToOPResources()
        {
            return resources;
        }
        
        public Dictionary<DEVELOPMENT_TYPE, int> convertSPToOPDevCards()
        {
            return devCards;
        }

        // DevCard
        public void playDevCard(DEVELOPMENT_TYPE type)
        {
            devCards[type]--;
            if (type == DEVELOPMENT_TYPE.VICTORY_POINT)
            {
                victoryPoints++;
            }
        }

        public void setNewDevCard(DEVELOPMENT_TYPE type)
        {
            devCards[type]++;
        }
        
        public int getDevCardAmount(DEVELOPMENT_TYPE type)
        {
            return devCards[type];
        }
    }
}