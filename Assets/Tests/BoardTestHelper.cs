using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardTestHelper
{
       private int[] neighborOffsetY = new int[] { -1, -1, 0, 1, 1, 0 }; //specifies the position of adjacent hexagons in vertical 
    private int[] neighborOffsetX = new int[] { 0, -1, -1, 0, 1, 1 }; //specifies the position of adjacent hexagons in horizontal direction

    public Stack<int> createRandomStack(int[] numbersToRandomize)
    {
        return new Stack<int>(numbersToRandomize.OrderBy(n => Guid.NewGuid()).ToArray());
    }

    public bool fieldNumberConstraintsMet(Board boardInstance)
    {
        string test ="---------Output--------\n";
        Hexagon[][] hexagons = boardInstance.getHexagonsArray();
        for (int row = 1; row < hexagons.Length-1; row++)
        {
            for (int col = 1; col < hexagons[row].Length-1; col++)
            {
               test +=hexagons[row][col].getFieldNumber() + "|";

                //only fieldnumbers 6 or 8 needs to be evaluated
                if (hexagons[row][col].getFieldNumber() != 6 && hexagons[row][col].getFieldNumber() != 8)
                {
                    continue;
                }

                for (int i = 0; i < neighborOffsetX.Length; i++)
                {
                    int offsetY = row + neighborOffsetY[i];
                    int offsetX = col + neighborOffsetX[i];

                    //if index is out of range there is no adjacent hexagon, therefore the constraint for this neighbor is met
                    if (offsetY > hexagons.Length - 1 || offsetX > hexagons[offsetY].Length - 1)
                    {
                        continue;
                    }
                    Hexagon neighbor =  hexagons[row + neighborOffsetY[i]][col + neighborOffsetX[i]];

                    //adjacent neighbor is 6 or 8 <=> constraint not met
                    if (neighbor != null && (neighbor.getFieldNumber() == 6 || neighbor.getFieldNumber() == 8))
                    {
                        return false;
                    }
                }

            }
            test += "\n";
        }
        test+="----------------------";
        Debug.Log(test);
        return true;
    }
}
