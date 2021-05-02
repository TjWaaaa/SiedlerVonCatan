using HexagonType;

namespace HexagonType
{
    public enum HEXAGONTYPE
    {
        WATER, DESERT,
        PORTNORMAL, PORTSHEEP, PORTWOOD, PORTBRICK, PORTORE, PORTWHEAT,
        SHEEP, WOOD, BRICK, ORE, WHEAT,
        NONE
    }
}

public class Hexagon
{

    int fieldNumber;
    public HEXAGONTYPE type;
    int[] position;
    Node[] nodes;
    Edge[] edges;

    public Hexagon(int fieldNumber, HEXAGONTYPE type)
    {
        this.fieldNumber = fieldNumber;
        this.type = type;
    }
}