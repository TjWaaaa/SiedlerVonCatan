using System.Collections.Generic;
using Enums;

public class Node
{
    private int posInArray;
    private int[][] adjacentHexagonsPos = new int[3][];
    private LinkedList<int> adjacentNodesPos = new LinkedList<int>();
    private LinkedList<int> adjacentEdgesPos = new LinkedList<int>();
    private PLAYERCOLOR occupant = PLAYERCOLOR.NONE;
    private BUILDING_TYPE buildingType = BUILDING_TYPE.NONE;

    public Node(int posInArray)
    {

    }

    public int getPosInArray()
    {
        return posInArray;
    }

    public void setAdjacentHexagonPos(int hexagonPosX, int hexagonPosY, int index)
    {
        adjacentHexagonsPos[index] = new[] { hexagonPosX, hexagonPosY };
    }

    public int[][] getAdjacentHexagonsPos()
    {
        return adjacentHexagonsPos;
    }

    public void setAdjacentNodePos(int nodePos)
    {
        adjacentNodesPos.AddLast(nodePos);
    }

    public LinkedList<int> getAdjacentNodesPos()
    {
        return adjacentNodesPos;
    }

    public void setAdjacentEdgePos(int edgePos, int index)
    {
        adjacentEdgesPos.AddLast(edgePos);
    }

    public LinkedList<int> getAdjacentEdgesPos()
    {
        return adjacentEdgesPos;
    }

    public void setOccupant(PLAYERCOLOR occupant)
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