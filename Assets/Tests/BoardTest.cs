using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardTest
{
    private int[] neighborOffsetX = new int[] { 0, -1, -1, 0, 1, 1 }; //specifies the position of adjacent hexagons in vertical direction
    private int[] neighborOffsetY = new int[] { -1, -1, 0, 1, 1, 0 }; //specifies the position of adjacent hexagons in horizontal direction
    // A Test behaves as an ordinary method
    [Test]
    public void BoardTestSimplePasses()
    {
        // Use the Assert class to test conditions

        Stack<int> testStack = new Stack<int> { };
        testStack.Push(9);
        testStack.Push(8);
        testStack.Push(9);
        testStack.Push(4);
        testStack.Push(3);
        testStack.Push(6);
        testStack.Push(3);
        testStack.Push(11);
        testStack.Push(10);
        testStack.Push(11);
        testStack.Push(10);
        testStack.Push(5);
        testStack.Push(2);
        testStack.Push(6);
        testStack.Push(12);
        testStack.Push(5);
        testStack.Push(4);
        testStack.Push(8);

        Board boardInstance = new Board(testStack);
        Hexagon[][] hexagons = boardInstance.getHexagons();
        for(int row=1;row<hexagons.Length;row++)
        {
            for(int col = 1; col < hexagons[row].Length;col++)
            {
                
                if (hexagons[row][col].getFieldNumber() != 6 && hexagons[row][col].getFieldNumber() != 8)
                {
                    continue;
                }

                for (int i = 0; i < neighborOffsetX.Length; i++)
                {
                   Hexagon neighbor = hexagons[row + neighborOffsetY[i]][col + neighborOffsetX[i]];
                    if(neighbor != null &&(neighbor.getFieldNumber() == 6 || neighbor.getFieldNumber() == 8))
                    {
                        Assert.IsTrue(false);
                    }
                }

            }
        }
        Assert.IsTrue(true);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator BoardTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
