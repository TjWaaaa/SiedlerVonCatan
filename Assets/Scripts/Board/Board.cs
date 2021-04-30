using System;
using System.Collections;
using System.Collections.Generic;

public class Board
{
    Hexagon[,] hexagons;
    Node[] nodes = new Node[54];
    Edge[] edges = new Edge[72];

    public int[,] gameboardConfig = {
        {0, 0, 4, 1, 4, 1, 0},
        { 0, 1, 2, 2, 2, 4, 0},
        {0, 4, 2, 2, 2, 2, 1},
        { 1, 2, 2, 3, 2, 2, 4},
        {0, 4, 2, 2, 2, 2, 1},
        { 0, 1, 2, 2, 2, 4, 0},
        {0, 0, 4, 1, 4, 1, 0},
    };

    public Board()
    {
        hexagons = initializeHexagons();
        //nodes = initializeNodes();
        //edges = initializeEdges();



    }

    private Hexagon[,] initializeHexagons()
    {
        Stack<HARALD> randomStack = getRandomResourceStack();

        hexagons = new Hexagon[10, 3];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                
            }
        }



        return hexagons;
    }

    private Stack<HARALD> getRandomResourceStack()
    {
        List<HARALD> list = new List<HARALD>() {
            HARALD.SHEEP, HARALD.SHEEP, HARALD.SHEEP, HARALD.SHEEP,
            HARALD.WOOD, HARALD.WOOD, HARALD.WOOD, HARALD.WOOD,
            HARALD.WHEAT, HARALD.WHEAT, HARALD.WHEAT, HARALD.WHEAT,
            HARALD.BRICK, HARALD.BRICK, HARALD.BRICK,
            HARALD.ORE, HARALD.ORE, HARALD.ORE
        };

        Stack<HARALD> stack = new Stack<HARALD>();
        Random r = new Random();

        for (int i = 0; i < 18; i++)
        {
            int randomPos = r.Next(18 - i);
            stack.Push(list[randomPos]);
            list.RemoveAt(randomPos);
        }

        return new Stack<HARALD>();
    }
}
