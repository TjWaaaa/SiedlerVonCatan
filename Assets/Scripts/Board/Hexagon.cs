using Enums;

public class Hexagon
{

    private int fieldNumber;
    private HEXAGON_TYPE type;
    private int[] position;
    private Node[] nodes = new Node[6];
    private bool blockedByRobber = false;

    public Hexagon(HEXAGON_TYPE type, int fieldNumber)
    {
        this.type = type;
        this.fieldNumber = fieldNumber;
    }
    public Hexagon(HEXAGON_TYPE type)
    {
        this.fieldNumber = 0;
        this.type = type;
    }

    public bool isPort()
    {
        switch (this.type)
        {
            case HEXAGON_TYPE.PORTNORMAL:
            case HEXAGON_TYPE.PORTSHEEP:
            case HEXAGON_TYPE.PORTWOOD:
            case HEXAGON_TYPE.PORTBRICK:
            case HEXAGON_TYPE.PORTORE:
            case HEXAGON_TYPE.PORTWHEAT: return true;
            default: return false;
        }
    }
    public bool isLand()
    {
        switch (this.type)
        {
            case HEXAGON_TYPE.SHEEP:
            case HEXAGON_TYPE.WOOD:
            case HEXAGON_TYPE.BRICK:
            case HEXAGON_TYPE.ORE:
            case HEXAGON_TYPE.WHEAT:
            case HEXAGON_TYPE.DESERT: return true;
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

    public HEXAGON_TYPE getType()
    {
        return type;
    }
}