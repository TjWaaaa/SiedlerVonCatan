using System.Collections.Generic;
using System.Data;
using Enums;

/// <summary>
/// Nodes represent the places on the board where villages and cities can be built. There are 54 Nodes on the
/// Board, on every point of contact of 3 Hexagons.
/// 
/// Every Node needs to know their
///   - 3 adjacent Hexagons,
///   - up to 3 adjacent Nodes,
///   - up to 3 adjacent Edges 
///   - whether or not it is occupied by a certain player
///   - which type of building it holds
/// </summary>
public class Node
{
    private LinkedList<int[]> adjacentHexagonsPos = new LinkedList<int[]>();
    private LinkedList<int> adjacentNodesPos = new LinkedList<int>();
    private LinkedList<int> adjacentEdgesPos = new LinkedList<int>();
    private PLAYERCOLOR occupant = PLAYERCOLOR.NONE;
    private BUILDING_TYPE buildingType = BUILDING_TYPE.NONE;
    
    /// <summary>
    /// Gets coordinates to one Hexagon in Board.hexagonsArray and saves it to
    /// adjacentHexagonsPos.
    /// Because there can only be 3 adjacent Hexagons to
    /// every Node, this function throws an error, if adjacentHexagonsPos already
    /// holds 3 Hexagon coordinates. Furthermore because there always have to be
    /// a X and a Y coordinate, the param int array for this function always has
    /// to have length 2.
    /// </summary>
    /// <param name="hexagonPos">coordinates of one Hexagon in Board.hexagonsArray</param>
    /// <exception cref="DataException"></exception>
    public void setAdjacentHexagonPos(int[] hexagonPos)
    {
        if (adjacentHexagonsPos.Count < 3 && hexagonPos.Length == 2)
        {
            adjacentHexagonsPos.AddLast(hexagonPos);
        }
        else
        {
            throw new DataException(); // TODO welche Exception?
        }
    }

    /// <summary>
    /// This function returns a list of coordinates to adjacent Hexagons saved
    /// in Board.hexagonsArray
    /// </summary>
    /// <returns>LinkedList of int arrays length 2 holding coordinates</returns>
    public LinkedList<int[]> getAdjacentHexagonsPos()
    {
        return adjacentHexagonsPos;
    }

    /// <summary>
    /// Gets coordinate to one Node in Board.nodesArray and saves it to adjacentNodesPos.
    /// Because there can only be 3 adjacent Nodes to every Node, this function
    /// throws an error, if adjacentNodesPos already holds 3 Node coordinates
    /// </summary>
    /// <param name="nodePos">coordinate of one Node in Board.nodesArray</param>
    public void setAdjacentNodePos(int nodePos)
    {
        if (adjacentNodesPos.Count < 3)
        {
            adjacentNodesPos.AddLast(nodePos);
        }
        else
        {
            throw new DataException(); // TODO welche Exception?
        }
    }
    
    /// <summary>
    /// This function returns a list of coordinates to adjacent Nodes saved
    /// in Board.nodesArray
    /// </summary>
    /// <returns>LinkedList holding coordinates</returns>
    public LinkedList<int> getAdjacentNodesPos()
    {
        return adjacentNodesPos;
    }

    /// <summary>
    /// Gets coordinate to one Edge in Board.edgesArray and saves it to adjacentEdgesPos.
    /// Because there can only be 3 adjacent Edges to every Node, this function
    /// throws an error, if adjacentEdgesPos already holds 3 Edge coordinates
    /// </summary>
    /// <param name="edgePos">coordinate of one Edge in Board.edgesArray</param>
    public void setAdjacentEdgePos(int edgePos)
    {
        if (adjacentEdgesPos.Count < 3)
        {
            adjacentEdgesPos.AddLast(edgePos);
        }
        else
        {
            throw new DataException(); // TODO welche Exception?
        }
    }

    /// <summary>
    /// This function returns a list of coordinates to adjacent Edges saved
    /// in Board.edgesArray
    /// </summary>
    /// <returns>LinkedList holding coordinates</returns>
    public LinkedList<int> getAdjacentEdgesPos()
    {
        return adjacentEdgesPos;
    }
    
    public void setOccupant(PLAYERCOLOR occupant)
    {
        this.occupant = occupant;
    }
    
    public PLAYERCOLOR getOccupant()
    {
        return occupant;
    }

    public void setBuildingType(BUILDING_TYPE buildingType)
    {
        this.buildingType = buildingType;
    }

    public BUILDING_TYPE getBuildingType()
    {
        return buildingType;
    }
}