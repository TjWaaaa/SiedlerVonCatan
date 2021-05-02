using System;
using System.Collections.Generic;
using HexagonType;

public class Board
{
    Hexagon[,] hexagons;
    Node[] nodes = new Node[54];
    Edge[] edges = new Edge[72];

    int[][] gameboardConfig = new int[][] {
        new int[] {4, 1, 4, 1},
        new int[] {1, 2, 2, 2, 4},
        new int[] {4, 2, 2, 2, 2, 1},
        new int[] {1, 2, 2, 3, 2, 2, 4},
        new int[] {4, 2, 2, 2, 2, 1},
        new int[] {1, 2, 2, 2, 4},
        new int[] {4, 1, 4, 1},
    };

    HEXAGONTYPE[] landHexagons = new[] {
        HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP, HEXAGONTYPE.SHEEP,
        HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD, HEXAGONTYPE.WOOD,
        HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT, HEXAGONTYPE.WHEAT,
        HEXAGONTYPE.BRICK, HEXAGONTYPE.BRICK, HEXAGONTYPE.BRICK,
        HEXAGONTYPE.ORE, HEXAGONTYPE.ORE, HEXAGONTYPE.ORE
    };

    HEXAGONTYPE[] portHexagons = new[] {
        HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL, HEXAGONTYPE.PORTNORMAL,
        HEXAGONTYPE.PORTSHEEP,
        HEXAGONTYPE.PORTWOOD,
        HEXAGONTYPE.PORTBRICK,
        HEXAGONTYPE.PORTORE,
        HEXAGONTYPE.PORTWHEAT
    };

    public void Start()
    {
        hexagons = initializeHexagons();
        //nodes = initializeNodes();
        //edges = initializeEdges();



    }

    private Hexagon[,] initializeHexagons()
    {
        Stack<HEXAGONTYPE> landStack = randomizeHexagonTypeArray(landHexagons);
        Stack<HEXAGONTYPE> portStack = randomizeHexagonTypeArray(portHexagons);

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

    private Stack<HEXAGONTYPE> randomizeHexagonTypeArray(HEXAGONTYPE[] array)
    {
        Random random = new Random();

        int n = array.Length;
        while (n > 1)
        {
            int k = random.Next(n--);   //Random.Range(0, n--); //      random.Next(n--);
            HEXAGONTYPE temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }

        return new Stack<HEXAGONTYPE>(array);
    }
}
