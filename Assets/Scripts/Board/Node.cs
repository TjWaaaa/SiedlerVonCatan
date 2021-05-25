using System;
using System.Collections.Generic;
using Enums;
using PlayerColor;

public class Node
{
    private int 
    private int[,] adjacentHexagonsPos = new int[3, 3];
    private int[] adjacentNodesPos = new int[3];
    private int[] adjacentEdgesPos = new int[3];
    private PLAYERCOLOR occupant = PLAYERCOLOR.NONE;
    private BUILDING_TYPE buildingType = BUILDING_TYPE.NONE;

    public Node()
    {
        
    }

    public void setAdjacentHexagonPos(int hexagonPosX, int hexagonPosY, int index)
    {
        adjacentHexagonsPos[index, 0] = hexagonPosX;
        adjacentHexagonsPos[index, 1] = hexagonPosY;
    }

    public int[,] getAdjacentHexagonsPos()
    {
        return adjacentHexagonsPos;
    }

    public void setAdjacentNodePos(int nodePos, int index)
    {
        adjacentNodesPos[index] = nodePos;
    }
    
    public int[] getAdjacentNodesPos()
    {
        return adjacentNodesPos;
    }

    public void setAdjacentEdgePos(int edgePos, int index)
    {
        adjacentEdgesPos[index] = edgePos;
    }

    public int[] getAdjacentEdgesPos()
    {
        return adjacentEdgesPos;
    }
    
    protected void setOccupant(PLAYERCOLOR occupant)
    {
        this.occupant = occupant;
    }
    
    public PLAYERCOLOR getOccupant()
    {
        return occupant;
    }

    public void setBuildingType(BUILDING_TYPE buildingType)
    {
        this.buildingType = buildingType;
    }

    public BUILDING_TYPE getBuildingType()
    {
        return buildingType;
    }
}