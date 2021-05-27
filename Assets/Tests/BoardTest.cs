using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
            string input = "Input: ";
            foreach (int item in randomStack)
            {
                input += item + "|";
            }
            Debug.Log(input);
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
    [Test]
   public void hermann()
    {
        test.Push(2);
        test.Push(9);
        test.Push(10);
        test.Push(11);
        test.Push(6);
        test.Push(8);
        test.Push(10);
        test.Push(4);
        test.Push(4);
        test.Push(8);
        test.Push(11);
        test.Push(9);
        test.Push(5);
        test.Push(6);
        test.Push(12);
        test.Push(3);
        test.Push(5);
        test.Push(3);

        Board boardInstance = new Board(test);
        bool constraintsMet = helper.fieldNumberConstraintsMet(boardInstance);
        Assert.IsTrue(constraintsMet);
    }

}
