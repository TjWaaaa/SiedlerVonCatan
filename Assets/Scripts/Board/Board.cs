using System;
using System.Collections.Generic;
using HexagonType;
using UnityEngine;
using System.IO;


public class Board
{
    private Hexagon[,] hexagons;
    private Node[] nodes = new Node[54];
    private Edge[] edges = new Edge[72];
    int posNodeArray = 0;

    private int[][] gameboardConfig = new int[][] {
        new int[]    {4, 1, 4, 1},
        new int[]   {1, 2, 2, 2, 4},
        new int[]  {4, 2, 2, 2, 2, 1},
        new int[] {1, 2, 2, 3, 2, 2, 4},
        new int[]  {4, 2, 2, 2, 2, 1},
        new int[]   {1, 2, 2, 2, 4},
        new int[]    {4, 1, 4, 1},
    };

    private int[] neighborOffsetX = new int[] { 0, -1, -1, 0, 1, 1 };
    private int[] neighborOffsetY = new int[] { -1, -1, 0, 1, 1, 0 };

    private HEXAGONTYPE[] landHexagons = {
        HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP,
        HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD,
        HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT,
        HEXAGONTYPE.BRICK, HEXAGONTYPE.BRICK, HEXAGONTYPE.BRICK,
        HEXAGONTYPE.ORE, HEXAGONTYPE.ORE, HEXAGONTYPE.ORE
    };

    private HEXAGONTYPE[] portHexagons = {
        HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL,
        HEXAGONTYPE.PORTSHEEP,
        HEXAGONTYPE.PORTWOOD,
        HEXAGONTYPE.PORTBRICK,
        HEXAGONTYPE.PORTORE,
        HEXAGONTYPE.PORTWHEAT
    };
    public static void main(String[] args)
    {
        Board board = new Board();
    }
    public Board()
    {
        nodes = initializeNodes();
        edges = initializeEdges();
        hexagons = initializeHexagons();
        assignNeighborsToHexagons();
        assignAdjecentHexagonsToNodes();
        assignNeighborsNodesToNodes();
    }

    private Hexagon[,] initializeHexagons()
    {
        Stack<HEXAGONTYPE> landStack = createRandomHexagonStackFromArray(landHexagons);
        Stack<HEXAGONTYPE> portStack = createRandomHexagonStackFromArray(portHexagons);

        Hexagon[,] hexagons = new Hexagon[7, 7];
        int pos = 0;

        for (int row = 0; row < gameboardConfig.Length; row++)
        {
            for (int col = 0; col < gameboardConfig[row].Length; col++)
            {
                int currentConfig = gameboardConfig[row][col];
                hexagons[row, col] = currentConfig switch
                {
                    0 => null,
                    1 => new Hexagon(pos, HEXAGONTYPE.WATER),
                    2 => new Hexagon(pos, landStack.Pop()),
                    3 => new Hexagon(pos, HEXAGONTYPE.DESERT),
                    4 => new Hexagon(pos, portStack.Pop()),
                    _ => null,
                };

                pos++;
            }
        }

        return hexagons;
    }

    private Stack<HEXAGONTYPE> createRandomHexagonStackFromArray(HEXAGONTYPE[] array)
    {
        System.Random random = new System.Random();

        int n = array.Length;
        while (n > 1)
        {
            int k = random.Next(n--);
            HEXAGONTYPE temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }

        return new Stack<HEXAGONTYPE>(array);
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
            edges[i] = new Edge();
        }
        return edges;
    }
    private void assignNeighborsToHexagons()
    {
        StreamReader file = new StreamReader("Assets/Scripts/Board/AdjacentNodesToHexagons.txt");

        for (int row = 0; row < gameboardConfig.Length; row++)
        {
            for (int col = 0; col < gameboardConfig[row].Length; col++)
            {
                if (hexagons[row, col] == null)
                {
                    continue;
                }

                Hexagon currentHexagon = hexagons[row, col];
                Console.WriteLine(currentHexagon.GetType());
                string line = file.ReadLine();

                if (line != null && line != "")
                {
                    string[] subStrings = line.Split(',');
                    for (int i = 0; i < 6; i++)
                    {
                        if (subStrings[i] != "-")
                        {
                            int neighborPos = Int32.Parse(subStrings[i]);
                            currentHexagon.addNode(nodes[neighborPos], i);
                        }
                    }
                }
            }
        }
        file.Close();
    }

    private void assignNeighborsNodesToNodes()
    {
        System.IO.StreamReader file = new System.IO.StreamReader("Assets/Scripts/Board/AdjacentNodesToNodes.txt");
        foreach (Node currentNode in nodes)
        {
            string line = file.ReadLine();
            if (line != "")
            {
                string[] indexes = line.Split(',');
                for (int i = 0; i < 3; i++)
                {
                    if (indexes[i] != "-")
                    {
                        int neighborPos = Int32.Parse(indexes[i]);
                        currentNode.addAdjacentNode(nodes[neighborPos], i);
                    }
                }
            }

        }
        file.Close();
    }

    private void assignAdjecentHexagonsToNodes()
    {
        System.IO.StreamReader file = new System.IO.StreamReader("Assets/Scripts/Board/AdjacentHexagonsToNodes.txt");
        foreach (Node currentNode in nodes)
        {
            string line = file.ReadLine();
            if (line != "" && line != null)
            {
                string[] subString = line.Split(',');
                string[][] coordinates = new string[3][];
                for (int i = 0; i < subString.Length; i++)
                {
                    coordinates[i] = subString[i].Split('.');
                }
                for (int i = 0; i < 3; i++)
                {
                    if (coordinates[i][0] != "-")
                    {
                        int neighborYCoordinate = Int32.Parse(coordinates[i][0]);
                        int neighborXCoordinate = Int32.Parse(coordinates[i][1]);
                        currentNode.addAdjacentHexagon(hexagons[neighborYCoordinate, neighborXCoordinate], i);
                    }
                }
            }
        }
        file.Close();
    }

    private void assignNeighborEdgesToNodes()
    {
        System.IO.StreamReader file = new System.IO.StreamReader("Assets/Scripts/Board/AdjacentEdgesToNodes.txt");
        foreach (Node currentNode in nodes)
        {
            string line = file.ReadLine();
            if (line != "")
            {

                string[] indexes = line.Split(',');
                for (int i = 0; i < indexes.Length; i++)
                {
                    if (indexes[i] != "-")
                    {

                        int neighborPos = Int32.Parse(indexes[i]);
                        currentNode.addAdjacentEdge(edges[neighborPos], i);
                    }
                }
            }
        }
        file.Close();
    }

    private int[][] readConfigFromFile(string path)
    {
        //System.IO.StreamReader file = new System.IO.StreamReader(path);
        if (File.Exists(path))
        {
            string[] fileContent = File.ReadAllLines(path);
            int[][] indexes = new int[fileContent.Length][];

            for (int i = 0; i < fileContent.Length; i++)
            {
                if (fileContent[i] != "")
                {
                    string[] line = fileContent[i].Split(',');
                    int[] lineIndexes = new int[line.Length];

                    for (int j = 0; j < line.Length; j++)
                    {
                        if (line[j] != "-" && line[j] != null)
                        {
                            lineIndexes[j] = Int32.Parse(line[j]);
                        }
                    }
                    indexes[i] = lineIndexes;
                }

            }
            return indexes;
        }
        else
        {
            throw new System.ArgumentException("File not found at directory: " + path);
        }
    }

    public void placeBuilding(int nodeID, Player player)
    {
        if (allowedToBuild(nodeID, player))
        {
            //implement me
        }
    }

    private bool allowedToBuild(int nodeID, Player player)
    {
        bool hasNeighborRoad = false;
        bool hasNoNeighborBuilding = false;
        Node[] neighborNodes = nodes[nodeID].getAdjacentNodes();
        Edge[] neighborEdges = nodes[nodeID].getAdjacentEdges();

        foreach (Node neighbor in neighborNodes)
        {
            if (neighbor.isOccupiedBy() != null)
            {
                hasNoNeighborBuilding = true;
            }
        }
        return hasNeighborRoad && hasNoNeighborBuilding;
    }

}
