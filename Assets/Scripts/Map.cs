﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // 2D level array
    private Tile[,] LevelMap;

    // dimensions of the level and some info about how far it can be shifted
    public int StartWidth, StartHeight, HorizontalBuffer, VerticalBuffer;
    private int Width, Height, FirstRow, FirstColumn;
    private int MapWidth, MapHeight;

    // list of units to be added to the level, made in the editor
    private Dictionary<Point, Unit> Units;
    public UnitsToAdd UnitsToAdd;

    // empty tile prefab to load into map
    public GameObject EmptyTilePrefab;

    private void Start()
    {
        // initialize dimensions
        Width = StartWidth;
        Height = StartHeight;
        MapWidth = StartWidth + 2 * VerticalBuffer;
        MapHeight = StartHeight + 2 * HorizontalBuffer;

        // stored in rows then columns
        LevelMap = new Tile[MapHeight, MapWidth];

        // Load dictionary
        Units = UnitsToAdd.BuildDictionary();

        // generating tiles using empty tile prefab
        for (int i = VerticalBuffer, y = 1; y <= StartHeight; i++, y++) 
        {
            for (int j = HorizontalBuffer, x = 1; x <= StartWidth; j++, x++) 
            {
                LevelMap[i, j] = Instantiate(EmptyTilePrefab).GetComponent<Tile>();
                Unit unitToPlace;
                if (Units.TryGetValue(new Point(x, y), out unitToPlace)) 
                {
                    LevelMap[i, j].SetUnit(unitToPlace);
                    Debug.Log("Successfully added " + LevelMap[i, j].GetUnit() + " at (" + x + "," + y + ").");
                }
                else
                {
                    Debug.Log("Nothing found at (" + x + "," + y + ").");
                }
            }
        }

        // print initial map 
        Debug.Log(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShiftRowRight(1);
            Debug.Log("Width: " + Width + "\tHeight: " + Height);
            Debug.Log(this);
        }
    }

    private bool ShiftRowRight(int row)
    {
        // if a tile is in the rightmost spot
        if (LevelMap[row, MapWidth - 1] != null) 
        {
            Debug.LogError("Invalid shift, no buffer remaining.");
            return false;
        }
        else
        {
            // shift the tiles to the right of the current column into 
            // the current column, starting with the rightmost column
            for (int col = MapWidth - 1; col > 0; col--)
            {
                LevelMap[row, col] = LevelMap[row, col - 1];
            }
            UpdateDimensions();
            return true;
        }
    }

    private void UpdateDimensions() 
    {
        int newMinRow = -1, newMaxRow = -1;
        // find the first and last row with tiles
        for (int row = 0; row < MapHeight; row++) 
        {
            for (int col = 0; col < MapWidth; col++) 
            {
                // found first row
                if (LevelMap[row, col] != null && newMinRow == -1) 
                {
                    newMinRow = row;
                    newMaxRow = row;
                    break; // move to next row
                }
                // found another row past the first
                else if (LevelMap[row, col] != null) 
                {
                    newMaxRow = row;
                    break; // move to next row
                }
            }
        }

        int newMinCol = -1, newMaxCol = -1;
        // find the first and last columns with tiles
        for (int col = 0; col < MapWidth; col++) 
        {
            for (int row = 0; row < MapHeight; row++) 
            {
                // found first column
                if (LevelMap[row, col] != null && newMinCol == -1) 
                {
                    newMinCol = col;
                    newMaxCol = col;
                    break; // move to next column
                }
                // found another column past the first
                else if (LevelMap[row, col] != null) 
                {
                    newMaxCol = col;
                    break; // move to next column
                }
            }
        }

        FirstRow = newMinRow;
        FirstColumn = newMinCol;
        Width = newMaxCol - newMinCol + 1;
        Height = newMaxRow - newMinRow + 1;
    }

    public override string ToString()
    {
        string output = "";
        for (int row = 0; row < MapHeight; row++) 
        {
            for (int col = 0; col < MapWidth; col++)
            {
                if (LevelMap[row, col] == null)
                {
                    output += "NA";
                }
                else
                {
                    if (LevelMap[row, col].GetUnit() == null)
                    {
                        output += "EM";
                    }
                    else 
                    {
                        output += LevelMap[row, col].GetUnit().ToString();
                    }
                }
                output += "\t";
            }
            output += "\n";
        }
        return output;
    }
}