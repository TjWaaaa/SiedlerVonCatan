using System.Collections.Generic;
using Enums;
using Newtonsoft.Json;

public class Hexagon
{
    [JsonProperty] private int fieldNumber;
    [JsonProperty] private HEXAGON_TYPE type;
    private LinkedList<int> adjacentNodesPos = new LinkedList<int>();
    private bool blockedByRobber = false;

    public Hexagon(HEXAGON_TYPE t, int fN)
    {
        this.t = t;
        this.fN = fN;
    }

    [JsonConstructor]
    public Hexagon(HEXAGON_TYPE t)
    {
        this.t = t;
        this.fN = 0;
    }

    public int getFieldNumber()
    {
        return this.fN;
    }
    public HEXAGON_TYPE getType()
    {
        return t;
    }

    public RESOURCETYPE getResourceType()
    {
        switch (t)
        {
            case HEXAGON_TYPE.SHEEP:
            case HEXAGON_TYPE.PORTSHEEP:
                return RESOURCETYPE.SHEEP;
            case HEXAGON_TYPE.ORE: 
            case HEXAGON_TYPE.PORTORE:
                return RESOURCETYPE.ORE;
            case HEXAGON_TYPE.BRICK:
            case HEXAGON_TYPE.PORTBRICK:
                return RESOURCETYPE.BRICK;
            case HEXAGON_TYPE.WOOD:
            case HEXAGON_TYPE.PORTWOOD:
                return RESOURCETYPE.WOOD;
            case HEXAGON_TYPE.WHEAT: 
            case HEXAGON_TYPE.PORTWHEAT:
                return RESOURCETYPE.WHEAT;
            default: return RESOURCETYPE.NONE;
        }
    }

    public void setAdjacentNodePos(int nodePos, int index)
    {
        adjacentNodesPos.AddLast(nodePos);
        //adjacentNodesPos[index] = nodePos;
    }

    public LinkedList<int> getAdjacentNodesPos()
    {
        return aN;
    }

    public bool isBlockedByRobber()
    {
        return bB;
    }

    public bool isPort()
    {
        switch (this.t)
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
        switch (this.t)
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