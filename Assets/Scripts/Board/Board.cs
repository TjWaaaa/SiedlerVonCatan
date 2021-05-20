using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuildingType;
using HexagonType;
using PlayerColor;
using UnityEngine;

public class Board
{
    private Hexagon[,] hexagons;
    private Node[] nodes = new Node[54];
    private Edge[] edges = new Edge[72];

    private Hexagon[][] hexagonDiceNumbers =
    {
        new Hexagon[1], // 2
        new Hexagon[2], // 3
        new Hexagon[2], // 4
        new Hexagon[2], // 5
        new Hexagon[2], // 6
        new Hexagon[2], // 8
        new Hexagon[2], // 9
        new Hexagon[2], // 10
        new Hexagon[2], // 11
        new Hexagon[1], // 12
    };

    private readonly int[][] boardConfig = {
        new[]    {4, 1, 4, 1},
        new[]   {1, 2, 2, 2, 4},
        new[]  {4, 2, 2, 2, 2, 1},
        new[] {1, 2, 2, 3, 2, 2, 4},
        new[]  {4, 2, 2, 2, 2, 1},
        new[]   {1, 2, 2, 2, 4},
        new[]    {4, 1, 4, 1}
    };

    private int[] neighborOffsetX = new int[] { 0, -1, -1, 0, 1, 1 }; //specifies the position of adjacent hexagons in vertical direction
    private int[] neighborOffsetY = new int[] { -1, -1, 0, 1, 1, 0 }; //specifies the position of adjacent hexagons in horizontal direction
    private int[] availableNumbers = new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };

    private readonly HEXAGONTYPE[] landHexagons = {
        HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP,
        HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD,
        HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT,
        HEXAGONTYPE.BRICK, HEXAGONTYPE.BRICK, HEXAGONTYPE.BRICK,
        HEXAGONTYPE.ORE, HEXAGONTYPE.ORE, HEXAGONTYPE.ORE
    };

    private readonly HEXAGONTYPE[] portHexagons = {
        HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL,
        HEXAGONTYPE.PORTSHEEP,
        HEXAGONTYPE.PORTWOOD,
        HEXAGONTYPE.PORTBRICK,
        HEXAGONTYPE.PORTORE,
        HEXAGONTYPE.PORTWHEAT
    };

    private Stack<int> numberStack;

    private const string path = "Assets/Scripts/Board/";

    public Board()
    {
        numberStack = createRandomHexagonNumberStack(availableNumbers);
        Debug.Log("alksjd");
        nodes = initializeNodes();
        edges = initializeEdges();
        hexagons = initializeHexagons();
        testNeighbors();
        checkPlacementConstraints();
        testNeighbors();
        // assignNeighborsToHexagons();
        // assignNeighborsToNodes();
        // assignNeighborsToEdges();

    }
    /// <summary>
    /// delete this method, its just for testing
    /// </summary>
    private void testNeighbors()
    {
        string test = "";
        test += "\n";
        for (int row = 1; row < 6; row++)
        {
            for (int col = 1; col < 6; col++)
            {
                Hexagon hex = hexagons[row, col];
                if (hex != null && hex.isLand())
                {
                    Debug.Log(hex.getFieldNumber());
                    test += Convert.ToString(hex.getFieldNumber()) + "|";
                }

            }
            test += "\n";
        }
        Debug.Log(test);
    }

    /// <summary>
    /// creates a 7x7 array of Hexagons with randomly placed hexagonTypes and hexagonNumbers
    /// </summary>
    /// <returns>returns the array</returns>
    private Hexagon[,] initializeHexagons()
    {
        Stack<HEXAGONTYPE> landStack = createRandomHexagonStackFromArray(landHexagons);
        Stack<HEXAGONTYPE> portStack = createRandomHexagonStackFromArray(portHexagons);

        Hexagon[,] hexagons = new Hexagon[7, 7];

        for (int row = 0; row < boardConfig.Length; row++)
        {
            for (int col = 0; col < boardConfig[row].Length; col++)
            {
                int currentConfig = boardConfig[row][col];

                HEXAGONTYPE type = currentConfig switch
                {
                    1 => HEXAGONTYPE.WATER,
                    2 => landStack.Pop(),
                    3 => HEXAGONTYPE.DESERT,
                    4 => portStack.Pop(),
                    _ => HEXAGONTYPE.NONE
                };

                if (currentConfig == 2)
                {
                    Hexagon newHexagon = new Hexagon(type, numberStack.Pop());
                    hexagons[row, col] = newHexagon;
                }
                else
                {
                    hexagons[row, col] = new Hexagon(type);
                }
            }
        }
        return hexagons;
    }

    /// <summary>
    /// creates a HEXAGONTYPE stack with types of a given array
    /// </summary>
    /// <param name="array">array of HEXAGONTYPEs with specific types</param>
    /// <returns>random HEXAGONTYPE stack</returns>
    private Stack<HEXAGONTYPE> createRandomHexagonStackFromArray(HEXAGONTYPE[] array)
    {


        return new Stack<HEXAGONTYPE>(array.OrderBy(n => Guid.NewGuid()).ToArray());
    }

    /// <summary>
    /// creates a int stack with numbers of a given array
    /// </summary>
    /// <param name="array">int array with specific numbers</param>
    /// <returns>random int stack</returns>
    private Stack<int> createRandomHexagonNumberStack(int[] array)
    {
        return new Stack<int>(array.OrderBy(n => Guid.NewGuid()).ToArray());
    }

    private Node[] initializeNodes()
    {
        Node[] nodes = new Node[54];

        for (int i = 0; i < 54; i++)
        {
            nodes[i] = new Node(i);
        }
        return nodes;
    }

    private Edge[] initializeEdges()
    {
        Edge[] edges = new Edge[72];

        for (int i = 0; i < 72; i++)
        {
            edges[i] = new Edge(i);
        }
        return edges;
    }

    /// <summary>
    /// reads from config file and adds to all hexagons in hexagons[] the adjacent nodes 
    /// </summary>
    private void assignNeighborsToHexagons()
    {
        StreamReader file = new StreamReader(path + "AdjacentNodesToHexagons.txt");

        for (int row = 0; row < boardConfig.Length; row++)
        {
            for (int col = 0; col < boardConfig[row].Length; col++)
            {
                // continue if there is no hexagon at given index
                if (hexagons[row, col] == null) continue;

                Hexagon currentHexagon = hexagons[row, col];
                Console.WriteLine(currentHexagon.GetType());

                // give all neighbors a hexagon needs to know
                string line = file.ReadLine();
                string[] subStrings = line.Split(',');

                for (int i = 0; i < 6; i++)
                {
                    if (subStrings[i] == "-") continue;

                    int neighborPos = int.Parse(subStrings[i]);
                    currentHexagon.addNode(nodes[neighborPos], i);
                }


                // set field number
                int fieldNumber = numberStack.Pop();
                //currentHexagon.setFieldNumber(fieldNumber);

                //TODO: whats the purpose of this for loop? @timo
                for (int i = 0; i < hexagonDiceNumbers[fieldNumber].Length; i++)
                {
                    hexagonDiceNumbers[fieldNumber][i] ??= currentHexagon;  // only adds Hexagon to slot if slot empty
                }
            }
        }
        file.Close();
    }

    /// <summary>
    /// reads from config files and adds to all nodes in nodes[] the adjacent hexagons, nodes and edges 
    /// </summary>
    private void assignNeighborsToNodes()
    {
        StreamReader hexagonsFile = new StreamReader(path + "AdjacentHexagonsToNodes.txt");
        StreamReader nodesFile = new StreamReader(path + "AdjacentNodesToNodes.txt");
        StreamReader edgesFile = new StreamReader(path + "AdjacentEdgesToNodes.txt");

        foreach (Node currentNode in nodes)
        {
            string[] nHexagons = hexagonsFile.ReadLine().Split(',');
            string[] nNodes = nodesFile.ReadLine().Split(',');
            string[] nEdges = edgesFile.ReadLine().Split(',');

            for (int i = 0; i < 3; i++)
            {
                // sets adjacent hexagon
                string[] nHexagonCoordinates = nHexagons[i].Split('.');
                int nHexagonPosX = int.Parse(nHexagonCoordinates[0]);
                int nHexagonPosY = int.Parse(nHexagonCoordinates[1]);
                currentNode.setAdjacentHexagon(hexagons[nHexagonPosX, nHexagonPosY], i);

                // sets adjacent node
                if (nNodes[i] != "-")
                {
                    int nNodePos = int.Parse(nNodes[i]);
                    currentNode.setAdjacentNode(nodes[nNodePos], i);
                }

                // sets adjacent edge
                if (nEdges[i] != "-")
                {
                    int nEdgePos = int.Parse(nEdges[i]);
                    currentNode.setAdjacentEdge(edges[nEdgePos], i);
                }
            }
        }
        hexagonsFile.Close();
        nodesFile.Close();
        edgesFile.Close();
    }

    /// <summary>
    /// reads from config files and adds to all edges ind edges[] the adjacent nodes and edges
    /// </summary>
    private void assignNeighborsToEdges()
    {
        StreamReader nodesFile = new StreamReader(path + "AdjacentNodesToEdges.txt");
        StreamReader edgesFile = new StreamReader(path + "AdjacentEdgesToEdges.txt");

        foreach (Edge currentEdge in edges)
        {
            string[] nNodes = nodesFile.ReadLine().Split(',');
            string[] nEdges = edgesFile.ReadLine().Split(',');

            for (int i = 0; i < 2; i++)
            {
                if (nNodes[i] != "-")
                {
                    int nNodePos = int.Parse(nNodes[i]);
                    currentEdge.setAdjacentNode(nodes[nNodePos], i);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (nEdges[i] != "-")
                {
                    int nEdgePos = int.Parse(nEdges[i]);
                    currentEdge.setAdjacentEdge(edges[nEdgePos], i);
                }
            }
        }
        nodesFile.Close();
        edgesFile.Close();
    }

    /// <summary>
    /// This function has to get called to place a village or city onto a specific node.
    /// If the player is allowed to place a building on the Node with id 'nodeId', a village
    /// gets built, or gets upgraded into a city
    /// </summary>
    /// <param name="nodeId">position of a node in the nodes[] array</param>
    /// <param name="player">color of the player who tries to build</param>
    public void placeBuilding(int nodeId, PLAYERCOLOR player)
    {
        Node currentNode = nodes[nodeId];

        if (!allowedToBuildOnNode(currentNode, player)) return;

        if (currentNode.getBuildingType() == BUILDINGTYPE.NONE)
        {
            currentNode.setBuildingType(BUILDINGTYPE.VILLAGE);
        }
        else if (currentNode.getBuildingType() == BUILDINGTYPE.VILLAGE)
        {
            currentNode.setBuildingType(BUILDINGTYPE.CITY);
        }
    }

    /// <summary>
    /// Checks rules whether a player is allowed to build on a given node or not 
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="player">color of the player who tries to build</param>
    /// <returns>true if player is allowed to build on given node</returns>
    private bool allowedToBuildOnNode(Node currentNode, PLAYERCOLOR player)
    {
        // false if node is already occupied by enemy OR is city 
        if (currentNode.getOccupant() != PLAYERCOLOR.NONE
            && currentNode.getOccupant() != player
            || currentNode.getBuildingType() == BUILDINGTYPE.CITY)
        {
            return false;
        }

        Node[] neighborNodes = currentNode.getAdjacentNodes();
        Edge[] neighborEdges = currentNode.getAdjacentEdges();

        foreach (Node neighbor in neighborNodes)
        {
            // false if a neighborNode is already occupied
            if (neighbor.getBuildingType() != BUILDINGTYPE.NONE) return false;
        }

        foreach (Edge neighbor in neighborEdges)
        {
            // true if at least 1 edge is occupied by player
            if (neighbor.getOccupant() == player) return true;
        }

        return false;
    }

    /// <summary>
    /// This function has to get called to place a road onto a specific edge.
    /// If the player is allowed to place a road on the Edge with id 'edgeId',
    /// a road gets built
    /// </summary>
    /// <param name="edgeId">position of an edge in the edges[] array</param>
    /// <param name="player">color of the player who tries to build</param>
    public void placeRoad(int edgeId, PLAYERCOLOR player)
    {
        Edge currentEdge = edges[edgeId];

        if (!allowedToBuildOnEdge(currentEdge, player)) return;

        currentEdge.setOccupant(player);
    }

    /// <summary>
    /// Checks rules whether a player is allowed to build on a given edge or not 
    /// </summary>
    /// <param name="currentEdge"></param>
    /// <param name="player">color of the player who tries to build</param>
    /// <returns>true if player is allowed to build on given edge</returns>
    private bool allowedToBuildOnEdge(Edge currentEdge, PLAYERCOLOR player)
    {
        if (currentEdge.getOccupant() != PLAYERCOLOR.NONE) return false;

        Node[] neighborNodes = currentEdge.getAdjacentNodes();
        Edge[] neighborEdges = currentEdge.getAdjacentEdges();

        foreach (Node neighbor in neighborNodes)
        {
            if (neighbor.getOccupant() == player) return true;
        }

        foreach (Edge neighbor in neighborEdges)
        {
            if (neighbor.getOccupant() == player) return true;
        }

        return false;
    }
    /// <summary>
    /// Checks if Hexagons with a fieldnumber of 6 or 8 are beside each other
    /// </summary>
    private void checkPlacementConstraints()
    {

        for (int row = 1; row < 6; row++)
        {
            for (int col = 1; col < 6; col++)
            {
                Hexagon currentHex = hexagons[row, col];
                if (currentHex != null && currentHex.getFieldNumber() != 0)
                {
                    int fieldNumber = currentHex.getFieldNumber();
                    Debug.Log("row: " + row + "col: " + col);
                    //neighbors just needs to be checked if fieldnumber is 6 or 8
                    if (fieldNumber == 6 || fieldNumber == 8)
                    {
                        for (int offsetIndex = 0; offsetIndex < neighborOffsetX.Length; offsetIndex++)
                        {
                            int yOffset = row + neighborOffsetY[offsetIndex];
                            int xOffset = col + neighborOffsetX[offsetIndex];
                            Hexagon neighbor = hexagons[yOffset, xOffset];
                            //if one of the neighbors fieldnumber is 6 or 8 the hexagon needs to be moved
                            if (neighbor != null && (neighbor.getFieldNumber() == 6 || neighbor.getFieldNumber() == 8))
                            {
                                int[] suitablePos = findSuitablePos();
                                swapHexagonPositions(row, col, suitablePos[0], suitablePos[1]);
                                break;

                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// checks the hexagons array for a suitable position where none of the adjacent hexagons has a 6 or 8 as fielnumber.
    /// </summary>
    /// <returns>an int array with the first occouring coordinates of a suitable position</returns>
    private int[] findSuitablePos()
    {
        bool suitable = true;
        for (int row = 1; row < 6; row++)
        {
            for (int col = 1; col < 6; col++)
            {
                Hexagon currentHex = hexagons[row, col];
                if (currentHex != null && currentHex.getFieldNumber() != 0)
                {
                    int fieldNumber = currentHex.getFieldNumber();
                    if (fieldNumber != 6 && fieldNumber != 8 && fieldNumber != 0)
                    {
                        for (int offsetIndex = 0; offsetIndex < neighborOffsetX.Length; offsetIndex++)
                        {
                            int yOffset = row + neighborOffsetY[offsetIndex];
                            int xOffset = col + neighborOffsetX[offsetIndex];
                            int neighborFieldnumber = 0;
                            try
                            {
                                neighborFieldnumber = hexagons[yOffset, xOffset].getFieldNumber();
                            }
                            catch (NullReferenceException e)
                            {

                            }
                            if (neighborFieldnumber == 6 || neighborFieldnumber == 8)
                            {
                                break;
                            }
                            else if (offsetIndex == neighborOffsetX.Length - 1)
                            {
                                return new int[] { row, col };
                            }
                        }
                    }
                }
            }
        }
        return new int[] { };
    }

    /// <summary>
    /// Swaps the position of two hexagons in the hexagons array.
    /// </summary>
    /// <param name="firstHexRow">specifies the row in the hexagons array of the first hexagon to swap</param>
    /// <param name="firstHexCol">specifies the column in the hexagons array of the first hexagon to swap</param>
    /// <param name="secondHexRow">specifies the row in the hexagons array of the second hexagon to swap</param>
    /// <param name="secondHexCol">specifies the column in the hexagons array of the second hexagon to swap</param>

    private void swapHexagonPositions(int firstHexRow, int firstHexCol, int secondHexRow, int secondHexCol)
    {
        Hexagon tempHex = hexagons[firstHexRow, firstHexCol];
        hexagons[firstHexRow, firstHexCol] = hexagons[secondHexRow, secondHexCol];
        hexagons[secondHexRow, secondHexCol] = tempHex;
    }
}
