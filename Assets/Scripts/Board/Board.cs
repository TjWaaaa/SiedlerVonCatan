using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Enums;
using PlayerColor;


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

    private int[] neighborOffsetX = new int[] { 0, -1, -1, 0, 1, 1 };
    private int[] neighborOffsetY = new int[] { -1, -1, 0, 1, 1, 0 };

    private readonly HEXAGON_TYPE[] landHexagons = {
        HEXAGON_TYPE.SHEEP, HEXAGON_TYPE.SHEEP, HEXAGON_TYPE.SHEEP, HEXAGON_TYPE.SHEEP,
        HEXAGON_TYPE.WOOD, HEXAGON_TYPE.WOOD, HEXAGON_TYPE.WOOD, HEXAGON_TYPE.WOOD,
        HEXAGON_TYPE.WHEAT, HEXAGON_TYPE.WHEAT, HEXAGON_TYPE.WHEAT, HEXAGON_TYPE.WHEAT,
        HEXAGON_TYPE.BRICK, HEXAGON_TYPE.BRICK, HEXAGON_TYPE.BRICK,
        HEXAGON_TYPE.ORE, HEXAGON_TYPE.ORE, HEXAGON_TYPE.ORE
    };

    private readonly HEXAGON_TYPE[] portHexagons = {
        HEXAGON_TYPE.PORTNORMAL, HEXAGON_TYPE.PORTNORMAL, HEXAGON_TYPE.PORTNORMAL, HEXAGON_TYPE.PORTNORMAL,
        HEXAGON_TYPE.PORTSHEEP,
        HEXAGON_TYPE.PORTWOOD,
        HEXAGON_TYPE.PORTBRICK,
        HEXAGON_TYPE.PORTORE,
        HEXAGON_TYPE.PORTWHEAT
    };
    
    private readonly int[] randomNumArray = {0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9};
    
    private const string path = "Assets/Scripts/Board/";

    public Board()
    {
        nodes = initializeNodes();
        edges = initializeEdges();
        hexagons = initializeHexagons();
        assignNeighborsToHexagons();
        assignNeighborsToNodes();
        assignNeighborsToEdges();
    }

    /// <summary>
    /// creates a 7x7 array of Hexagons with randomly placed hexagon_Types and hexagonNumbers
    /// </summary>
    /// <returns>returns the array</returns>
    private Hexagon[,] initializeHexagons()
    {
        Stack<HEXAGON_TYPE> landStack = createRandomHexagonStackFromArray(landHexagons);
        Stack<HEXAGON_TYPE> portStack = createRandomHexagonStackFromArray(portHexagons);
        Stack<int> numberStack = createRandomHexagonNumberStack(randomNumArray);

        Hexagon[,] hexagons = new Hexagon[7, 7];

        for (int row = 0; row < boardConfig.Length; row++)
        {
            for (int col = 0; col < boardConfig[row].Length; col++)
            {
                int currentConfig = boardConfig[row][col];
                /*
                hexagons[row, col] = currentConfig switch
                {
                    0 => null,
                    1 => new Hexagon(HEXAGON_TYPE.WATER),
                    2 => new Hexagon(landStack.Pop(), numberStack.Pop()),
                    3 => new Hexagon(HEXAGON_TYPE.DESERT),
                    4 => new Hexagon(portStack.Pop()),
                    _ => null,
                };
                */

                switch (currentConfig)
                {
                    case 1:
                        hexagons[row, col] = new Hexagon(HEXAGON_TYPE.WATER);
                        break;
                    case 2:
                        int fieldNumber = numberStack.Pop();
                        Hexagon newHexagon = new Hexagon(landStack.Pop(), fieldNumber);
                        hexagons[row, col] = newHexagon;
                        
                        // adds Hexagon to empty slot in array
                        Debug.Log(fieldNumber + " - " + hexagonDiceNumbers[fieldNumber].Length);
                        for (int i = 0; i < hexagonDiceNumbers[fieldNumber].Length; i++)
                        {
                            hexagonDiceNumbers[fieldNumber][i] ??= newHexagon;  // only adds Hexagon to slot if slot empty
                        }
                        break;
                    case 3:
                        hexagons[row, col] = new Hexagon(HEXAGON_TYPE.DESERT);
                        break;
                    case 4:
                        hexagons[row, col] = new Hexagon(portStack.Pop());
                        break;
                }
            }
        }
        return hexagons;
    }

    /// <summary>
    /// creates a HEXAGON_TYPE stack with types of a given array
    /// </summary>
    /// <param name="array">array of HEXAGON_TYPEs with specific types</param>
    /// <returns>random HEXAGON_TYPE stack</returns>
    private Stack<HEXAGON_TYPE> createRandomHexagonStackFromArray(HEXAGON_TYPE[] array)
    {
        return new Stack<HEXAGON_TYPE>(array.OrderBy(n => Guid.NewGuid()).ToArray());
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
                string line = file.ReadLine();
                string[] subStrings = line.Split(',');
                
                for (int i = 0; i < 6; i++)
                {
                    if (subStrings[i] == "-") continue;
                    
                    int neighborPos = int.Parse(subStrings[i]);
                    currentHexagon.addNode(nodes[neighborPos], i);
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
        
        if (currentNode.getBuilding_Type() == BUILDING_TYPE.NONE)
        {
            currentNode.setBuilding_Type(BUILDING_TYPE.VILLAGE);
        }
        else if (currentNode.getBuilding_Type() == BUILDING_TYPE.VILLAGE)
        {
            currentNode.setBuilding_Type(BUILDING_TYPE.CITY);
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
            || currentNode.getBuilding_Type() == BUILDING_TYPE.CITY)
        {
            return false;
        }

        Node[] neighborNodes = currentNode.getAdjacentNodes();
        Edge[] neighborEdges = currentNode.getAdjacentEdges();

        foreach (Node neighbor in neighborNodes)
        {
            // false if a neighborNode is already occupied
            if (neighbor.getBuilding_Type() != BUILDING_TYPE.NONE) return false;
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
}