public class Node
{
    private String nodeID;
    private Hexagon[3] adjacentHexagons;
    private Edge[3] adjacentEdges;
    private Player occupiedBy;
    private Building placedBuilding;

    public Node()
    {
        //TODO: implement ID:   this.nodeID = this.GetHashCode;
        this.adjacentHexagons = new Hexagon[3] { };
        this.adjacentEdges = new Edge[3] { };
        this.occupiedBy = null;
        this.placedBuilding = null;
    }
    protected setPlayer(Player player)
    {
        this.occupiedBy = player;
    }
    protected setBuilding(Building building)
    {
        this.placedBuilding = building;
    }

    protected setEdge(Edge edge)
    {
        this.adjacentEdges.add(edge);
    }

    protected setAdjacentHexagon(Hexagon hexagon)
    {
        this.adjacentHexagons.add(hexagon);
    }
}