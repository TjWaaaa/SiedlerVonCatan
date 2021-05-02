using System;

public class Edge
{
    private String edgeId;
    private Edge[] neighborEdges;
    private Node[] neighborNodes;

    public Edge()
    {

        this.neighborEdges = new Edge[4];
        this.neighborNodes = new Node[2];
    }

}