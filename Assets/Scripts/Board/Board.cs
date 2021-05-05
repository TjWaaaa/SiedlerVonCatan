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
            nodes[i] = new Node();
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

    private void assignNeighborsToNodes()
    {

        System.IO.StreamReader file = new System.IO.StreamReader("Assets/Scripts/Board/AdjacentNodesToNodes.txt");
        foreach (Node currentNode in nodes)
        {
            string line = file.ReadLine();
            if (line != "")
            {
                string[] indexes = line.Split(',');
                foreach (string index in indexes)
                {
                    if (index != "-")
                    {
                        int neighborPos = Int32.Parse(index);
                    }
                }
            }

        }
    }

    private void assignNeighborEdgesToNodes()
    {
        //System.IO.StreamReader file = new System.IO.StreamReader("Assets/Scripts/Board/")

    }
}
