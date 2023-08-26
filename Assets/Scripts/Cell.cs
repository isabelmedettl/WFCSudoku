using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell 
{
    public int[] cellStates = {1, 2, 3, 4, 5, 6, 7, 8, 9};
    public bool isCollapsed = false;
    public int currentState = 0;

    public Cell() { }

    public void RemoveStates(int[] statesToRemove)
    {
        foreach(int state in statesToRemove)
        {
            if(cellStates.Contains(state))
            {
                int[] tempCellStates = new int[cellStates.Length - 1];
                int counter = 0;
                for (int i = 0; i < tempCellStates.Length; i++)
                {
                    if (cellStates[i] != state)
                    {
                        tempCellStates[i] = cellStates[counter];
                        counter++;
                    } else
                    {
                        counter++;
                        tempCellStates[i] = cellStates[counter];
                        counter++;
                    }
                }
                cellStates = tempCellStates;
            }
        }
        /*
        if (cellStates.Length == 1)
        {
            Collapse();
        }
        */
    }

    public void Collapse()
    {
        isCollapsed = true;
        currentState = cellStates[UnityEngine.Random.Range(0, cellStates.Length)];
        Debug.Log(currentState);
        cellStates = new int[0];
    }

    public int GetNumberOfPossibleStates()
    {
        return cellStates.Length;
    }
}
