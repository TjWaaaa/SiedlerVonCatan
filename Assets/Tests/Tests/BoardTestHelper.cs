using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts.Board;

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
        Hexagon[][] hexagons = boardInstance.getHexagons();
        for (int row = 1; row < hexagons.Length-1; row++)
        {
            for (int col = 1; col < hexagons[row].Length-1; col++)
            {
                //only fieldnumbers 6 or 8 needs to be evaluated
                if (hexagons[row][col].getFieldNumber() != 6 && hexagons[row][col].getFieldNumber() != 8)
                {
                    continue;
                }

                for (int i = 0; i < neighborOffsetX.Length; i++)
                {
                    Hexagon neighbor = null;
                    try
                    {
                         neighbor = hexagons[row + neighborOffsetY[i]][col + neighborOffsetX[i]];
                    }catch(IndexOutOfRangeException e)
                    {
                        //nothing needs to be done, out of Range occours because the differnce in row length 
                    }

                    //adjacent neighbor is 6 or 8 <=> constraint not met
                    if (neighbor != null && (neighbor.getFieldNumber() == 6 || neighbor.getFieldNumber() == 8))
                    {
                        return false;
                    }
                }

            }
        }
        return true;
    }
}
