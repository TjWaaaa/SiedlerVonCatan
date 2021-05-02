using System;
using System.Collections.Generic;

public class Node
{
    private String nodeID;
    private List<Hexagon> adjacentHexagons;
    private List<Edge> adjacentEdges;
    private Player occupiedBy;
    private String placedBuilding;

    public Node()
    {
        //TODO: implement ID:   this.nodeID = this.GetHashCode;
        this.adjacentHexagons = new List<Hexagon>();
        this.adjacentEdges = new List<Edge>();
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

    protected void setEdge(Edge edge)
    {
        this.adjacentEdges.Add(edge);
    }

    protected void setAdjacentHexagon(Hexagon hexagon)
    {
        this.adjacentHexagons.Add(hexagon);
    }
}