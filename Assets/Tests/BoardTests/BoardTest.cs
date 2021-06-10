using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Enums;

public class BoardTest
{
    
    private int[] availableNumbers = new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };
    private BoardTestHelper helper = new BoardTestHelper();
   
    private int[] arie = new int[] { 2, 4, 21, 31 };
    private Stack<int> test = new Stack<int> { };


    [Test]
    public void testFieldnumberConstraints()
    {
        int numOfTests = 1000;
        bool constraintsMet = false;

        for (int i = 0; i < numOfTests; i++)
        {
            Stack<int> randomStack = helper.createRandomStack(availableNumbers);
            int[] delete = randomStack.ToArray();

            foreach (int item in randomStack)
            {

            }

            Board boardInstance = new Board(randomStack);
            Hexagon[][] hex = boardInstance.getHexagonsArray();
            constraintsMet = helper.fieldNumberConstraintsMet(boardInstance);

            if (!constraintsMet)
            {
                break;
            }
        }
        Assert.IsTrue(constraintsMet);
    }
 }

public class testCanPlaceBuilding
{
    [Test]
    public void testCanPlaceVillageInPreGamePhase_Positive()
    {
        Board board;

        // check all valid nodeIds with all valid player colors
        for (int playerColor = 0; playerColor < 4; playerColor++)
        {
            for (int nodeId = 0; nodeId < 54; nodeId++)
            {
                board = new Board();
                Assert.IsTrue(board.canPlaceBuilding(nodeId, (PLAYERCOLOR) playerColor, BUILDING_TYPE.VILLAGE, true));
            }
        }
    }

    [Test]
    public void testCanPlaceVillageInPreGamePhase_Negative()
    {
        Board board = new Board();
        
        // check PLAYERCOLOR.NONE
        Assert.IsFalse(board.canPlaceBuilding(0, PLAYERCOLOR.NONE, BUILDING_TYPE.VILLAGE, true));
        
        // check ocupied node
        board.placeBuilding(0, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE);
        Assert.IsFalse(board.canPlaceBuilding(0, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, true));
        
        // check node near ocupied node
        Assert.IsFalse(board.canPlaceBuilding(3, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, true));
        Assert.IsFalse(board.canPlaceBuilding(4, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, true));
        
        // check wrong BUILDINGTYPE
        board = new Board();
        Assert.IsFalse(board.canPlaceBuilding(0, PLAYERCOLOR.RED, BUILDING_TYPE.CITY, true));
        Assert.IsFalse(board.canPlaceBuilding(0, PLAYERCOLOR.RED, BUILDING_TYPE.ROAD, true));
        Assert.IsFalse(board.canPlaceBuilding(0, PLAYERCOLOR.RED, BUILDING_TYPE.NONE, true));
    }

    [Test]
    public void testCanPlaceVillageInPreGamePhase_InvalidInput()
    {
        Board board = new Board();
        
        // check nodeId out of bounds
        Assert.That(() => board.canPlaceBuilding(-1, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, true),
            Throws.TypeOf<IndexOutOfRangeException>());
        Assert.That(() => board.canPlaceBuilding(54, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, true),
            Throws.TypeOf<IndexOutOfRangeException>());
    }

    [Test]
    public void testCanPlaceVillageAfterPreGamePhase_Positive()
    {
        Board board = new Board();
        
        // check place place village near own road
        board.placeRoad(0, PLAYERCOLOR.RED);
        Assert.IsTrue(board.canPlaceBuilding(0, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, false));
        Assert.IsTrue(board.canPlaceBuilding(3, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, false));
    }

    [Test]
    public void testCanPlaceVillageAfterPreGamePhase_Negative()
    {
        Board board = new Board();
        // check place village without road
        Assert.IsFalse(board.canPlaceBuilding(0, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, false));
        Assert.IsFalse(board.canPlaceBuilding(20, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, false));
        Assert.IsFalse(board.canPlaceBuilding(23, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, false));
        
        // check place village near enemy road
        board.placeRoad(35, PLAYERCOLOR.BLUE);
        Assert.IsFalse(board.canPlaceBuilding(23, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, false));
        Assert.IsFalse(board.canPlaceBuilding(29, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, false));
        
        // check place village near & on other village
        board.placeBuilding(29, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE);
        board.placeRoad(35, PLAYERCOLOR.RED);
        Assert.IsFalse(board.canPlaceBuilding(23, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, false));
        Assert.IsFalse(board.canPlaceBuilding(35, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, false));
        Assert.IsFalse(board.canPlaceBuilding(29, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, false));
    }
}

public class testPlaceBuilding
{
    [Test]
    public void testPlaceVillage_Positive()
    {
        Board board;
        // test place village on empty node & place city on village
        for (int playerColor = 0; playerColor < 4; playerColor++)
        {
            board = new Board();
            for (int nodeId = 0; nodeId < 54; nodeId++)
            {
                Assert.IsTrue(board.placeBuilding(nodeId, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE));
                Assert.IsTrue(board.placeBuilding(nodeId, PLAYERCOLOR.RED, BUILDING_TYPE.CITY));
            }
        }
    }

    [Test]
    public void testPlaceVillage_Negative()
    {
        Board board = new Board();

        // place village on occupied node
        board.placeBuilding(5, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE);
        Assert.IsFalse(board.placeBuilding(5, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE));
        Assert.IsFalse(board.placeBuilding(5, PLAYERCOLOR.BLUE, BUILDING_TYPE.VILLAGE));
        
        board.placeBuilding(8, PLAYERCOLOR.YELLOW, BUILDING_TYPE.VILLAGE);
        Assert.IsFalse(board.placeBuilding(8, PLAYERCOLOR.YELLOW, BUILDING_TYPE.VILLAGE));
        Assert.IsFalse(board.placeBuilding(8, PLAYERCOLOR.WHITE, BUILDING_TYPE.VILLAGE));
        
        // place city on empty & enemy node & city node
        Assert.IsFalse(board.placeBuilding(4, PLAYERCOLOR.WHITE, BUILDING_TYPE.CITY)); // on empty
        Assert.IsFalse(board.placeBuilding(8, PLAYERCOLOR.WHITE, BUILDING_TYPE.CITY)); // on enemy village
        board.placeBuilding(10, PLAYERCOLOR.YELLOW, BUILDING_TYPE.VILLAGE);
        board.placeBuilding(10, PLAYERCOLOR.YELLOW, BUILDING_TYPE.CITY);
        Assert.IsFalse(board.placeBuilding(10, PLAYERCOLOR.YELLOW, BUILDING_TYPE.CITY)); // on own city
    }

    [Test]
    public void testPlaceVillage_InvalidInput()
    {
        Board board = new Board();
        
        Assert.That(() => board.placeBuilding(-1, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE),
            Throws.TypeOf<IndexOutOfRangeException>());
        Assert.That(() => board.placeBuilding(54, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE),
            Throws.TypeOf<IndexOutOfRangeException>());
    }
}

public class testCanPlaceRoad
{
    [Test]
    public void testCanPlaceRoadAfterPreGamePhase_Positive()
    {
        Board board;

        // test all valid edgeIds with all valid player colors
        for (int playerColor = 0; playerColor < 4; playerColor++)
        {
            board = new Board();
            for (int nodeId = 0; nodeId < 54; nodeId++)
            {
                board.placeBuilding(nodeId, (PLAYERCOLOR) playerColor, BUILDING_TYPE.VILLAGE);
            }
            for (int edgeId = 0; edgeId < 72; edgeId++)
            {
                Assert.IsTrue(board.canPlaceRoad(edgeId, (PLAYERCOLOR) playerColor));
            }
        }
        
        // test road near own city
        board = new Board();
        board.placeBuilding(8, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE);
        board.placeBuilding(8, PLAYERCOLOR.RED, BUILDING_TYPE.CITY);
        Assert.IsTrue(board.canPlaceRoad(7, PLAYERCOLOR.RED));
        Assert.IsTrue(board.canPlaceRoad(12, PLAYERCOLOR.RED));
        Assert.IsTrue(board.canPlaceRoad(13, PLAYERCOLOR.RED));
    }
    
    [Test]
    public void testCanPlaceRoadAfterPreGamePhase_Negative()
    {
        Board board = new Board();

        // test place road not near village or city
        Assert.IsFalse(board.canPlaceRoad(0, PLAYERCOLOR.RED));
        Assert.IsFalse(board.canPlaceRoad(19, PLAYERCOLOR.RED));
        
        // test place road near enemy village and city
        board.placeBuilding(0, PLAYERCOLOR.BLUE, BUILDING_TYPE.VILLAGE);
        Assert.IsFalse(board.canPlaceRoad(0, PLAYERCOLOR.RED));
        Assert.IsTrue(board.canPlaceRoad(0, PLAYERCOLOR.BLUE));
        board.placeBuilding(0, PLAYERCOLOR.BLUE, BUILDING_TYPE.CITY);
        Assert.IsFalse(board.canPlaceRoad(0, PLAYERCOLOR.WHITE));
        Assert.IsTrue(board.canPlaceRoad(0, PLAYERCOLOR.BLUE));
    }

    [Test]
    public void testCanPlaceRoadAfterPreGamePhase_InvalidInput()
    {
        Board board = new Board();
        
        // check edgeId out of bounds
        Assert.That(() => board.canPlaceBuilding(-1, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, true),
            Throws.TypeOf<IndexOutOfRangeException>());
        Assert.That(() => board.canPlaceBuilding(76, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, true),
            Throws.TypeOf<IndexOutOfRangeException>());
    }
}

public class testPlaceRoad
{
    [Test]
    public void testPlaceRoad_Positive()
    {
        Board board;
        
        // test place road on empty edge
        for (int playerColor = 0; playerColor < 4; playerColor++)
        {
            board = new Board();
            for (int edgeId = 0; edgeId < 72; edgeId++)
            {
                Assert.IsTrue(board.placeRoad(edgeId, (PLAYERCOLOR) playerColor));
            }
        }
    }

    [Test]
    public void testPlaceRoad_Negative()
    {
        Board board = new Board();
        
        // test place road on not empty edge
        board.placeRoad(0, PLAYERCOLOR.RED);
        Assert.IsFalse(board.placeRoad(0, PLAYERCOLOR.RED));
        Assert.IsFalse(board.placeRoad(0, PLAYERCOLOR.BLUE));
    }

    [Test]
    public void testPlaceRoad_InvalidInput()
    {
        Board board = new Board();
        
        // check edgeId out of bounds
        Assert.That(() => board.canPlaceBuilding(-1, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, true),
            Throws.TypeOf<IndexOutOfRangeException>());
        Assert.That(() => board.canPlaceBuilding(76, PLAYERCOLOR.RED, BUILDING_TYPE.VILLAGE, true),
            Throws.TypeOf<IndexOutOfRangeException>());
    }
}