using Enums;
using Newtonsoft.Json;

public class Hexagon
{
    [JsonProperty] private int fieldNumber;
    [JsonProperty] private HEXAGON_TYPE type;
    [JsonProperty] private int[] adjacentNodesPos = new int[6];
    [JsonProperty] private bool blockedByRobber = false;

    public Hexagon(HEXAGON_TYPE type, int fieldNumber)
    {
        this.type = type;
        this.fieldNumber = fieldNumber;
    }

    [JsonConstructor]
    public Hexagon(HEXAGON_TYPE type)
    {
        this.type = type;
        this.fieldNumber = 0;
    }

    public int getFieldNumber()
    {
        return this.fieldNumber;
    }
    public HEXAGON_TYPE getType()
    {
        return type;
    }

    public void setAdjacentNodePos(int nodePos, int index)
    {
        adjacentNodesPos[index] = nodePos;
    }

    public int[] getAdjacentNodesPos()
    {
        return adjacentNodesPos;
    }

    public bool isBlockedByRobber()
    {
        return blockedByRobber;
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
            case HEXAGON_TYPE.WHEAT: return true;
            default: return false;
        }
    }
}