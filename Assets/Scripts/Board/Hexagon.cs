using HexagonType;

public class Hexagon
{

    private int fieldNumber; //
    private HEXAGONTYPE type;
    private int[] position;
    private Node[] nodes = new Node[6];

    public Hexagon(int fieldNumber, HEXAGONTYPE type)
    {
        this.fieldNumber = fieldNumber;
        this.type = type;
    }
    public Hexagon(HEXAGONTYPE type)
    {
        this.fieldNumber = 0;
        this.type = type;
    }

    public bool isPort()
    {
        switch (this.type)
        {
            case HEXAGONTYPE.PORTNORMAL:
            case HEXAGONTYPE.PORTSHEEP:
            case HEXAGONTYPE.PORTWOOD:
            case HEXAGONTYPE.PORTBRICK:
            case HEXAGONTYPE.PORTORE:
            case HEXAGONTYPE.PORTWHEAT: return true;
            default: return false;
        }
    }
    public bool isLand()
    {
        switch (this.type)
        {
            case HEXAGONTYPE.SHEEP:
            case HEXAGONTYPE.WOOD:
            case HEXAGONTYPE.BRICK:
            case HEXAGONTYPE.ORE:
            case HEXAGONTYPE.WHEAT:
            case HEXAGONTYPE.DESERT: return true;
            default: return false;
        }
    }

    public Node getNode(int index)
    {
        if (index > 5 || index < 0)
        {
            throw new System.ArgumentOutOfRangeException("Index out of Range!");
        }
        else
        {
            return nodes[index];
        }
    }

    public void addNode(Node node, int index)
    {
        if (index < 0 || index > 5 || nodes[index] != null || nodes == null)
        {
            throw new System.ArgumentException("Illegal Arguments:" + node);
        }
        else
        {
            nodes[index] = node;
        }
    }

    public HEXAGONTYPE getType()
    {
        return type;
    }
}