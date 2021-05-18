using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using ResourceType;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    private string playerName;
    private Color color;
    private int playerID;
    private bool isReady;

    private int points;

    private Dictionary<RESOURCETYPE, int> resources = new Dictionary<RESOURCETYPE, int>
    {
        {RESOURCETYPE.SHEEP, 0},
        {RESOURCETYPE.ORE, 0},
        {RESOURCETYPE.BRICK, 0},
        {RESOURCETYPE.WOOD, 0},
        {RESOURCETYPE.WHEAT, 0}

    };

public Player(int playerID) {
        this.playerID = playerID;
    }
    
    // TODO: remove this one, only use the upper one with the id!!!
    public Player(string playerName, Color color)
    {
        this.playerName = playerName;
        this.color = color;
    }

    public int GetPlayerID() {
        return playerID;
    }
    
    public Color GetColor()
    {
        return color;
    }
    
    public void setColor(Color color)
    {
        this.color = color;
    }

    public string GetName()
    {
        return playerName;
    }
    
    public void setPlayerName(string name)
    {
        this.playerName = name;
    }

    public void setIsReady(bool isReady)
    {
        this.isReady = isReady;
    }

    public bool getIsReady()
    {
        return isReady;
    }

    public void setResourceAmount(RESOURCETYPE resourcetype, int amount)
    {
        resources[resourcetype] += amount;
    }

    public int getResourceAmount(RESOURCETYPE resourcetype)
    {
        return resources[resourcetype];
    }



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

    public void trade(RESOURCETYPE giveResourcetype, RESOURCETYPE getResourcetype)
    {
        resources[giveResourcetype] -= 4;
        resources[getResourcetype] += 1;
    }

    public Boolean canBuildStreet()
    {
        if (resources[RESOURCETYPE.BRICK] >= 1 && resources[RESOURCETYPE.WOOD] >= 1) return true;
        else return false;
    }

    public Boolean canBuildVillage()
    {
        if (resources[RESOURCETYPE.BRICK] >= 1
            && resources[RESOURCETYPE.WOOD] >= 1
            && resources[RESOURCETYPE.SHEEP] >= 1
            && resources[RESOURCETYPE.WHEAT] >= 1)
        { return true; }
        else return false;
    }

    public Boolean canBuildCity()
    {
        if (resources[RESOURCETYPE.ORE] >= 3
            && resources[RESOURCETYPE.WHEAT] >= 2)
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
}
