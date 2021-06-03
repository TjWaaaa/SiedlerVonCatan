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
