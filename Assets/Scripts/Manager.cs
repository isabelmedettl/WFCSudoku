using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject board;
    public Cell[,] cellGrid = new Cell[9,9];
    public GameObject[] buttons = new GameObject[81];
    public Sprite[] spritesToGive = new Sprite[9];
    public Vector2[][] boxes = new Vector2[9][];

    private void Start()
    {
        PopulateCellGrid();
        PopulateBoxes();
        Button[] tempButtons = board.GetComponentsInChildren<Button>();
        for (int i = 0; i < tempButtons.Length; i++)
        {
            buttons[i] = tempButtons[i].gameObject;
        }
        SetTextToButtons();
    }

    private void SetTextToButtons()
    {
        for (int x = 0; x < buttons.Length; x++)
        {
            Vector2 coords = buttons[x].GetComponent<CellClickButtonHandler>().position;
            Cell cell = cellGrid[(int)coords.x, (int)coords.y];
            string textToSet = cell.GetNumberOfPossibleStates().ToString();
            Debug.Log(textToSet);
            buttons[x].GetComponent<CellClickButtonHandler>().SetText(textToSet);
        }
    }
    

    private void PopulateCellGrid()
    {
        for (int x = 0; x < cellGrid.GetLength(0); x++)
        {
            for (int y = 0; y < cellGrid.GetLength(1); y++)
            {
                cellGrid[x, y] = new Cell();
            }
        }
    }

    private void PopulateBoxes()
    {
        for (int i = 0; i < 9; i++)
        {
            boxes[i] = new Vector2[9];
            
        }
        BoxHelper(0, 3, 0, 3, 0);
        BoxHelper(0, 3, 3, 6, 1);
        BoxHelper(0, 3, 6, 9, 2);
        BoxHelper(3, 6, 0, 3, 3);
        BoxHelper(3, 6, 3, 6, 4);
        BoxHelper(3, 6, 6, 9, 5);
        BoxHelper(6, 9, 0, 3, 6);
        BoxHelper(6, 9, 3, 6, 7);
        BoxHelper(6, 9, 6, 9, 8);
    }

    private void BoxHelper(int startX, int stopX, int startY, int stopY, int box)
    {
        int boxIndex = 0;
        for (int x = startX; x < stopX; x++)
        {
            for (int y = startY; y < stopY; y++)
            {
                boxes[box][boxIndex] = new Vector2((float)x, (float)y);
                boxIndex++;
            }
        }
    }

    public void CollapseSequence(Vector2 coords)
    {
        Cell cellToCollapse = cellGrid[(int)coords.x, (int)coords.y];
        if (!cellToCollapse.isCollapsed)
        {
            cellToCollapse.Collapse();
            int[] stateToRemove = { cellToCollapse.currentState };
            GetSpriteToCell(coords);
            Vector2[] currentBox = BoxFinder(coords);

            for (int i = 0; i < 9; i++)
            {
                if (!cellGrid[(int)coords.x, i].isCollapsed)
                {
                    cellGrid[(int)coords.x, i].RemoveStates(stateToRemove);
                    if (cellGrid[(int)coords.x, i].GetNumberOfPossibleStates() == 1)
                    {
                        CollapseSequence(new Vector2((int)coords.x, i));
                        GetSpriteToCell(new Vector2((int)coords.x, i));
                    }
 

                }
                if (!cellGrid[i, (int)coords.y].isCollapsed)
                {
                    cellGrid[i, (int)coords.y].RemoveStates(stateToRemove);
                    if (cellGrid[i, (int)coords.y].GetNumberOfPossibleStates() == 1)
                    {
                        CollapseSequence(new Vector2(i, (int)coords.y));
                        GetSpriteToCell(new Vector2(i, coords.y));
                    }
                }
                Vector2 cellCoords = currentBox[i];
                if (!cellGrid[(int)cellCoords.x, (int)cellCoords.y].isCollapsed)
                {
                    Debug.Log("Success");
                    cellGrid[(int)cellCoords.x, (int)cellCoords.y].RemoveStates(stateToRemove);
                    if (cellGrid[(int)cellCoords.x, (int)cellCoords.y].GetNumberOfPossibleStates() == 1)
                    {
                        CollapseSequence(cellCoords);
                        GetSpriteToCell(cellCoords);
                    }
                }
            }
            SetTextToButtons();
        }
    }

    public Vector2[] BoxFinder(Vector2 cellCoords)
    {
        foreach (Vector2[] box in boxes)
        {
            foreach(Vector2 cell in box)
            {
                if (cell.x== cellCoords.x && cell.y == cellCoords.y)
                {
                    
                    return box;
                }
            }
        }
        return null;
    }

    public void GetSpriteToCell(Vector2 coords)
    {
        foreach (GameObject button in buttons)
        {
            if (button.GetComponent<CellClickButtonHandler>().position.x == coords.x && button.GetComponent<CellClickButtonHandler>().position.y == coords.y)
            {
                Cell cell = FindCell(coords);
                button.GetComponent<CellClickButtonHandler>().SetSprite(spritesToGive[cell.currentState - 1]);
            }
        }
    }

    public Cell FindCell(Vector2 coordsToFind)
    {
        return cellGrid[(int)coordsToFind.x, (int)coordsToFind.y];
    }
}

public struct Box
{
    public Cell [,] cells;
}
