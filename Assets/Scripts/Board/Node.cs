using System;
using System.Collections.Generic;

public class Node
{
    private int nodeID;
    private Hexagon[] adjacentHexagons;
    private Node[] adjacentNodes;
    private Edge[] adjacentEdges;
    private Player occupiedBy;
    private String placedBuilding;

    public Node(int id)
    {
        this.nodeID = id;
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

    public void addAdjacentEdge(Edge edge, int index)
    {
        this.adjacentEdges[index] = edge;
    }

    public void addAdjacentHexagon(Hexagon hexagon, int index)
    {
        this.adjacentHexagons[index] = hexagon;
    }

    public void addAdjacentNode(Node node, int index)
    {
        this.adjacentNodes[index] = node;
    }

    public Node[] getAdjacentNodes()
    {
        return this.adjacentNodes;
    }

    public Hexagon[] getAdjacentHexagons()
    {
        return this.adjacentHexagons;
    }

    public Edge[] getAdjacentEdges()
    {
        return this.adjacentEdges;
    }

    public Player isOccupiedBy()
    {
        return this.occupiedBy;
    }
}