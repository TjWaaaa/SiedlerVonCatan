using System;
using System.Collections.Generic;

public class Node
{
    private String nodeID;
    private Hexagon[] adjacentHexagons;
    private Node[] adjacentNodes;
    private Edge[] adjacentEdges;
    private Player occupiedBy;
    private String placedBuilding;

    public Node()
    {
        //TODO: implement ID:   this.nodeID = this.GetHashCode;
        this.adjacentHexagons = new Hexagon[3];
        this.adjacentNodes = new Node[3];
        this.adjacentEdges = new Edge[3];
        this.occupiedBy = null;
        this.placedBuilding = null;
    }
    protected void setPlayer(Player player)
    {
        this.occupiedBy = player;
    }
    protected void setBuilding(String building)
    {
        this.placedBuilding = building;
    }

    protected void addAdjacentEdge(Edge edge, int index)
    {
        this.adjacentEdges[index] = edge;
    }

    protected void addAdjacentHexagon(Hexagon hexagon, int index)
    {
        this.adjacentHexagons[index] = hexagon;
    }

    protected void addAdjacentNode(Node node, int index)
    {
        this.adjacentNodes[index] = node;
    }
}