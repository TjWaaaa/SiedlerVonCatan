using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Resource;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    private string playerName;
    private Color color;

    private int points;

    private Dictionary<RESOURCE, int> resources = new Dictionary<RESOURCE, int>
    {
        {RESOURCE.SHEEP, 0},
        {RESOURCE.ORE, 0},
        {RESOURCE.BRICK, 0},
        {RESOURCE.WOOD, 0},
        {RESOURCE.WHEAT,0}

    }; 
    
    
    public Player(string playerName, Color color) {
        this.playerName = playerName;
        this.color = color;
    }

    public Color GetColor() {
        return color;
    }

    public string GetName() {
        return playerName;
    }

    public void setResourceAmount(RESOURCE resource, int amount)
    {
        resources[resource] += amount;
    }

    public int getResourceAmount(RESOURCE resource)
    {
        return resources[resource];
    }
    
    

    public Boolean canTrade(RESOURCE resource)
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

    public void trade(RESOURCE giveResource, RESOURCE getResource)
    {
        resources[giveResource] -= 4;
        resources[getResource] += 1;
    }

    public Boolean canBuildStreet()
    {
        if (resources[RESOURCE.BRICK] >= 1 && resources[RESOURCE.WOOD] >= 1) return true;
        else return false;
    }

    public Boolean canBuildVillage()
    {
        if (resources[RESOURCE.BRICK] >= 1
            && resources[RESOURCE.WOOD] >= 1
            && resources[RESOURCE.SHEEP] >= 1
            && resources[RESOURCE.WHEAT] >= 1)
        {return true;}
        else return false;
    }

    public Boolean canBuildCity()
    {
        if (resources[RESOURCE.ORE] >= 3
            && resources[RESOURCE.WHEAT] >= 2)
            return true;
        else return false;
    }

    public void buyStreet()
    {
        resources[RESOURCE.BRICK] -= 1;
        resources[RESOURCE.WOOD] -= 1;
    }
    
    public void buyVillage()
    {
        resources[RESOURCE.BRICK] -= 1;
        resources[RESOURCE.WOOD] -= 1;
        resources[RESOURCE.SHEEP] -= 1;
        resources[RESOURCE.WHEAT] -= 1;
    }
    
    public void buyCity()
    {
        resources[RESOURCE.ORE] -= 3;
        resources[RESOURCE.WHEAT] -= 2;
    }
}
