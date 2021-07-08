using System.Collections.Generic;
using Enums;

public class Edge
{
    private int posInArray;
    private LinkedList<int> adjacentNodesPos = new LinkedList<int>();
    private LinkedList<int> adjacentEdgesPos = new LinkedList<int>();
    private PLAYERCOLOR occupant = PLAYERCOLOR.NONE;

    public Edge(int posInArray)
    {
        this.posInArray = posInArray;
    }

    public int getPosInArray()
    {
        return posInArray;
    }

    public void setAdjacentNodePos(int nodePos, int index)
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
}