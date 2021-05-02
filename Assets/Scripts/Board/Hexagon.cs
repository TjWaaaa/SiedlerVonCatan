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

    int fieldNumber; //
    public HEXAGONTYPE type;
    int[] position;
    Node[6] nodes;
    Edge[6] edges;

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
}