using System;
using System.Collections.Generic;
using Enums;
using PlayerColor;

public class Node
{
    private int nodeId;
    private Hexagon[] adjacentHexagons = new Hexagon[3];
    private Node[] adjacentNodes = new Node[3];
    private Edge[] adjacentEdges = new Edge[3];
    private PLAYERCOLOR occupant = PLAYERCOLOR.NONE;
    private BUILDING_TYPE building_Type = BUILDING_TYPE.NONE;

    public Node(int nodeId)
    {
        this.nodeId = nodeId;
    }
    
    protected void setOccupant(PLAYERCOLOR occupant)
    {
        this.occupant = occupant;
    }
    
    public PLAYERCOLOR getOccupant()
    {
        return occupant;
    }

    public void setAdjacentHexagon(Hexagon hexagon, int index)
    {
        adjacentHexagons[index] = hexagon;
    }
    
    public Hexagon[] getAdjacentHexagons()
    {
        return adjacentHexagons;
    }

    public void setAdjacentNode(Node node, int index)
    {
        adjacentNodes[index] = node;
    }
    
    public Node[] getAdjacentNodes()
    {
        return adjacentNodes;
    }

    public void setAdjacentEdge(Edge edge, int index)
    {
        adjacentEdges[index] = edge;
    }

    public Edge[] getAdjacentEdges()
    {
        return adjacentEdges;
    }

    public void setBuilding_Type(BUILDING_TYPE building_Type)
    {
        this.building_Type = building_Type;
    }

    public BUILDING_TYPE getBuilding_Type()
    {
        return building_Type;
    }
}