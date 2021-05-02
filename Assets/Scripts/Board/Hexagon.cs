using HexagonType;

public class Hexagon
{

    int fieldNumber; //
    public HEXAGONTYPE type;
    int[] position;
    Node[] nodes;

    public Hexagon(int fieldNumber, HEXAGONTYPE type)
    {
        this.fieldNumber = fieldNumber;
        this.type = type;
        this.nodes = nodes;
    }
    public Hexagon(HEXAGONTYPE type)
    {
        this.fieldNumber = 0;
        this.type = type;
    }
}