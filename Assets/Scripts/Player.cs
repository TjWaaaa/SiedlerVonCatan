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

    private int points;

    private Dictionary<RESOURCETYPE, int> resources = new Dictionary<RESOURCETYPE, int>
    {
        {RESOURCETYPE.SHEEP, 0},
        {RESOURCETYPE.ORE, 0},
        {RESOURCETYPE.BRICK, 0},
        {RESOURCETYPE.WOOD, 0},
        {RESOURCETYPE.WHEAT,0}

    };


    public Player(string playerName, Color color)
    {
        this.playerName = playerName;
        this.color = color;
    }

    public Color GetColor()
    {
        return color;
    }

    public string GetName()
    {
        return playerName;
    }

    public void setResourceAmount(RESOURCETYPE resource, int amount)
    {
        resources[resource] += amount;
    }

    public int getResourceAmount(RESOURCETYPE resource)
    {
        return resources[resource];
    }



    public Boolean canTrade(RESOURCETYPE resource)
    {
        if (resources[resource] >= 4)
        {
            return true;
        }
        else
        {
            Debug.Log("only " + resources[resource] + " of " + resource);
            return false;
        }

        //should only return true if there are at least 4
    }

    public void trade(RESOURCETYPE giveResource, RESOURCETYPE getResource)
    {
        resources[giveResource] -= 4;
        resources[getResource] += 1;
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
