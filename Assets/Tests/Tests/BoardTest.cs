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

    [Test]
    public void testFieldnumberConstraints()
    {
        int numOfTests = 10;
        bool constraintsMet = false;

        for (int i = 0; i < numOfTests; i++)
        {
            Board boardInstance = new Board(helper.createRandomStack(availableNumbers));
            constraintsMet = helper.fieldNumberConstraintsMet(boardInstance);

            if (!constraintsMet)
            {
                break;
            }
        }
        Assert.IsTrue(constraintsMet);
    }
   

}
