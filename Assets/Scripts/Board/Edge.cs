using PlayerColor;

public class Edge
{
    private int edgeId;
    private Node[] adjacentNodes;
    private Edge[] adjacentEdges;
    private PLAYERCOLOR occupant = PLAYERCOLOR.NONE;

    public Edge(int id)
    {
        edgeId = id;
        adjacentEdges = new Edge[4];
        adjacentNodes = new Node[2];
    }
    
    public void setOccupant(PLAYERCOLOR occupant)
    {
        this.occupant = occupant;
    }

    public PLAYERCOLOR getOccupant()
    {
        return occupant;
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
}