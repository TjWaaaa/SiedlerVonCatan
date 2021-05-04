using System;
using System.Collections.Generic;
using HexagonType;


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

    private int[] neighborOffSetX = new int[] { 0, -1 };
    private int[] neighborOffSetY = new int[] { -1, 0 };
    private HEXAGONTYPE[] landHexagons = new[] {
        HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP,
        HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD,
        HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT,
        HEXAGONTYPE.BRICK, HEXAGONTYPE.BRICK, HEXAGONTYPE.BRICK,
        HEXAGONTYPE.ORE, HEXAGONTYPE.ORE, HEXAGONTYPE.ORE
    };

    private HEXAGONTYPE[] portHexagons = new[] {
        HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL,
        HEXAGONTYPE.PORTSHEEP,
        HEXAGONTYPE.PORTWOOD,
        HEXAGONTYPE.PORTBRICK,
        HEXAGONTYPE.PORTORE,
        HEXAGONTYPE.PORTWHEAT
    };
    public static void Main(String[] args)
    {
        Board board = new Board();
    }
    public Board()
    {
        nodes = initializeNodes();
        edges = initializeEdges();
        hexagons = initializeHexagons();
        assignNeighbors();
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
        Random random = new Random();

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
    private void assignNeighbors()
    {
        for (int row = 1; row < hexagons.Length - 1; row++)
        {
            for (int col = 1; col < gameboardConfig[row].Length - 1; row++)
            {
                if (hexagons[row, col] == null)
                {
                    continue;
                }

                Hexagon currentHex = hexagons[row, col];
                //only Port- and Landhexagons need to know their neighbors
                if (currentHex.isLand())
                {
                    assignNodes(row, col);
                }
            }
        }
    }

    private void assignNodes(int row, int col)
    {
        Hexagon currentHex = hexagons[row, col];
        for (int i = 0; i < 2; i++)
        {
            try
            {
                Hexagon neighbor = hexagons[neighborOffSetY[i], neighborOffSetX[i]];
                if (neighbor.isLand())
                {
                    switch (i)
                    {
                        case 0: appendNodes(currentHex, neighbor, i); break;
                        case 1: appendNodes(currentHex, neighbor, i); break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            currentHex.addNode(nodes[nextNodePosition()], 1);
                            currentHex.addNode(nodes[nextNodePosition()], 0);
                            break;
                        case 1:
                            currentHex.addNode(nodes[nextNodePosition()], 5);
                            currentHex.addNode(nodes[nextNodePosition()], 4);
                            break;
                    }
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                break;
            }
        }
        currentHex.addNode(nodes[nextNodePosition()], 2);
        currentHex.addNode(nodes[nextNodePosition()], 3);
    }

    private int nextNodePosition()
    {
        return posNodeArray++;
    }

    private void appendNodes(Hexagon currentHex, Hexagon neighbor, int pos)
    {
        if (pos == 0)
        {
            Node node1 = neighbor.getNode(3);
            Node node2 = neighbor.getNode(4);

            currentHex.addNode(node1, 1);
            currentHex.addNode(node2, 0);
        }
        else if (pos == 1)
        {
            Node node1 = neighbor.getNode(1);
            Node node2 = neighbor.getNode(2);

            currentHex.addNode(node1, 5);
            currentHex.addNode(node2, 4);
        }
    }
}
