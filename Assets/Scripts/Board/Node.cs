using System;
using System.Collections.Generic;
using BuildingType;
using PlayerColor;

public class Node
{
    private int nodeId;
    private Hexagon[] adjacentHexagons = new Hexagon[3];
    private Node[] adjacentNodes = new Node[3];
    private Edge[] adjacentEdges = new Edge[3];
    private PLAYERCOLOR occupant = PLAYERCOLOR.NONE;
    private BUILDINGTYPE buildingType = BUILDINGTYPE.NONE;

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

    public void setBuildingType(BUILDINGTYPE buildingType)
    {
        this.buildingType = buildingType;
    }

    public BUILDINGTYPE getBuildingType()
    {
        return buildingType;
    }
}