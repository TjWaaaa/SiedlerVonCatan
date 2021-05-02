public class Edge
{
    private String edgeId;
    private Edge[4] neighborEdges;
    private Node[2] neighborNodes;

    public Edge()
    {

        this.neighborEdges = new Edge[4] { };
        this.neighborNodes = new Node[2] { };
    }

}