﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // 2D level array
    private Tile[,] LevelMap, LockMap, CompositeMap;

    // dimensions of the level and some info about how far it can be shifted
    public int StartWidth, StartHeight, HorizontalBuffer, VerticalBuffer;
    private int Width, Height, FirstRow, FirstColumn;
    private int MapWidth, MapHeight;
    public float CenterX, CenterY;
    private float MinX, MinY, MaxX, MaxY;

    // list of units to be added to the level, made in the editor
    private Dictionary<Point, Unit> Units, LockedUnits;
    public UnitsToAdd UnitsToAdd, LockedUnitsToAdd;

    // empty tile prefab to load into map
    public GameObject EmptyTilePrefab, LockedTilePrefab;
    private float TileWidth, TileHeight;

    // row and column selection variables
    private int Row, Column;
    private bool PullLeft, PullRight, PullDown, PullUp;

    // selection arrow gameobject
    public GameObject SelectionArrow;

    private void Start()
    {
        // initialize dimensions
        Width = StartWidth;
        Height = StartHeight;
        MapWidth = StartWidth + 2 * VerticalBuffer;
        MapHeight = StartHeight + 2 * HorizontalBuffer;

        // tile properties
        TileWidth = EmptyTilePrefab.GetComponent<SpriteRenderer>().bounds.max.x - EmptyTilePrefab.GetComponent<SpriteRenderer>().bounds.min.x;
        TileHeight = EmptyTilePrefab.GetComponent<SpriteRenderer>().bounds.max.y - EmptyTilePrefab.GetComponent<SpriteRenderer>().bounds.min.y;

        // borders of level
        MinX = CenterX - ((float) (MapWidth - 1) / 2 * TileWidth);
        MinY = CenterY - ((float) (MapHeight - 1) / 2 * TileHeight);
        MaxX = CenterX + ((float) (MapWidth - 1) / 2 * TileWidth);
        MaxY = CenterY + ((float) (MapHeight - 1) / 2 * TileHeight);

        // default row and col information
        FirstRow = VerticalBuffer;
        FirstColumn = HorizontalBuffer;

        // initialize selection variables
        Row = VerticalBuffer;
        Column = HorizontalBuffer;

        PullLeft = true; // start pulling rows to the left
        PullRight = false;
        PullDown = false;
        PullUp = false;

        // stored in rows then columns
        LevelMap = new Tile[MapHeight, MapWidth];
        LockMap = new Tile[MapHeight, MapWidth];

        // Load dictionary
        Units = UnitsToAdd.BuildDictionary();
        LockedUnits = LockedUnitsToAdd.BuildDictionary();

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
                    // Debug.Log("Successfully added " + LevelMap[i, j].GetUnit() + " at (" + x + "," + y + ").");
                }
                else
                {
                    // Debug.Log("Nothing found at (" + x + "," + y + ").");
                }
            }
        }

        // generate locked map
        for (int i = VerticalBuffer, y = 1; y <= StartHeight; i++, y++)
        {
            for (int j = HorizontalBuffer, x = 1; x <= StartWidth; j++, x++)
            {
                Unit lockedUnitToPlace;
                if (LockedUnits.TryGetValue(new Point(x, y), out lockedUnitToPlace))
                {
                    LockMap[i, j] = Instantiate(LockedTilePrefab).GetComponent<Tile>();
                    LockMap[i, j].SetUnit(lockedUnitToPlace);
                }
                else
                {
                    // don't place a lock tile and move on
                }
            }
        }

        // print initial map 
        Debug.Log(this);
        // display initial map
        DisplayMap();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W)) 
        {
            SelectNextClockwise();
        }
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            SelectNextAnticlockwise();
        }
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            PullSelected();
            DisplayMap();
        }
        ShowSelectionArrow();

        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShiftRowLeft(1);
            Debug.Log("Width: " + Width + "\tHeight: " + Height);
            Debug.Log(this);
            // display initial map
            DisplayMap();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ShiftRowRight(1);
            Debug.Log("Width: " + Width + "\tHeight: " + Height);
            Debug.Log(this);
            // display initial map
            DisplayMap();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ShiftColumnUp(1);
            Debug.Log("Width: " + Width + "\tHeight: " + Height);
            Debug.Log(this);
            // display initial map
            DisplayMap();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ShiftColumnDown(1);
            Debug.Log("Width: " + Width + "\tHeight: " + Height);
            Debug.Log(this);
            // display initial map
            DisplayMap();
        }
        */
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
            // shift the tile to the left of the current column into 
            // the current column, starting with the rightmost column
            for (int col = MapWidth - 1; col > 0; col--)
            {
                LevelMap[row, col] = LevelMap[row, col - 1];
            }
            LevelMap[row, 0] = null; // clear out leftmost tile leftover
            UpdateDimensions();
            return true;
        }
    }

    private bool ShiftRowLeft(int row)
    {
        // if a tile is in the leftmost spot
        if (LevelMap[row, 0] != null) 
        {
            Debug.LogError("Invalid shift, no buffer remaining.");
            return false;
        }
        else
        {
            // shift the tile to the right of the current column into 
            // the current column, starting with the leftmost column
            for (int col = 0; col < MapWidth - 1; col++)
            {
                LevelMap[row, col] = LevelMap[row, col + 1];
            }
            LevelMap[row, MapWidth - 1] = null; // clear out rightmost tile leftover
            UpdateDimensions();
            return true;
        }
    }

    private bool ShiftColumnDown(int col)
    {
        // if a tile is in the lowest spot
        if (LevelMap[MapHeight - 1, col] != null) 
        {
            Debug.LogError("Invalid shift, no buffer remaining.");
            return false;
        }
        else
        {
            // shift the tile above the current row into 
            // the current row, starting with the bottom row
            for (int row = MapHeight - 1; row > 0; row--)
            {
                LevelMap[row, col] = LevelMap[row - 1, col];
            }
            LevelMap[0, col] = null; // clear out highest tile leftover
            UpdateDimensions();
            return true;
        }
    }

    private bool ShiftColumnUp(int col)
    {
        // if a tile is in the highest spot
        if (LevelMap[0, col] != null) 
        {
            Debug.LogError("Invalid shift, no buffer remaining.");
            return false;
        }
        else
        {
            // shift the tile below the current row into 
            // the current row, starting with the top row
            for (int row = 0; row < MapHeight - 1; row++)
            {
                LevelMap[row, col] = LevelMap[row + 1, col];
            }
            LevelMap[MapHeight - 1, col] = null; // clear out lowest tile leftover
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

        if (PullLeft) 
        {
            Column = newMinCol;
        } 
        else if (PullRight)
        {
            Column = newMaxCol;
        }
        else if (PullDown)
        {
            Row = newMaxRow;
        }
        else if (PullUp)
        {
            Row = newMinRow;
        }
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

    public void DisplayMap()
    {
        for (int row = 0; row < MapHeight; row++) 
        {
            for (int col = 0; col < MapWidth; col++)
            {
                if (!(LockMap[row, col] == null))
                {
                    LockMap[row, col].transform.position = new Vector3(MinX + col * TileWidth, MaxY - row * TileHeight, 0f);
                }
                if (LevelMap[row, col] == null) 
                {
                    // skip this pass, no buffer to display
                    // Debug.Log("Nothing found at row " + row + " col " + col + ".");
                }
                else 
                {
                    LevelMap[row, col].transform.position = new Vector3(MinX + col * TileWidth, MaxY - row * TileHeight, 0f);
                    // Debug.Log("Found tile at row " + row + " col " + col + ".");
                }
            }
        }
    }

    public void SelectNextClockwise()
    {
        if (PullLeft)
        {
            if (Row == FirstRow)
            {
                PullUp = true;
                PullLeft = false;
            }
            else
            {
                Row--;
            }
        }
        else if (PullUp)
        {
            if (Column == FirstColumn + Width - 1)
            {
                PullRight = true;
                PullUp = false;
            }
            else 
            {
                Column++;
            }
        }
        else if (PullRight) 
        {
            if (Row == FirstRow + Height - 1)
            {
                PullDown = true;
                PullRight = false;
            }
            else
            {
                Row++;
            }
        }
        else if (PullDown)
        {
            if (Column == FirstColumn)
            {
                PullLeft = true;
                PullDown = false;
            }
            else 
            {
                Column--;
            }
        }
    }

    public void SelectNextAnticlockwise() 
    {
        if (PullLeft)
        {
            if (Row == FirstRow + Height - 1) 
            {
                PullDown = true;
                PullLeft = false;
            }
            else 
            {
                Row++;
            }
        }
        else if (PullDown)
        {
            if (Column == FirstColumn + Width - 1)
            {
                PullRight = true;
                PullDown = false;
            }
            else
            {
                Column++;
            }
        }
        else if (PullRight) 
        {
            if (Row == FirstRow)
            {
                PullUp = true;
                PullRight = false;
            }
            else
            {
                Row--;
            }
        }
        else if (PullUp)
        {
            if (Column == FirstColumn)
            {
                PullLeft = true;
                PullUp = false;
            }
            else 
            {
                Column--;
            }
        }
    }

    public void PullSelected()
    {
        if (PullLeft)
        {
            ShiftRowLeft(Row);
        }
        else if (PullRight)
        {
            ShiftRowRight(Row);
        }
        if (PullDown)
        {
            ShiftColumnDown(Column);
        }
        if (PullUp)
        {
            ShiftColumnUp(Column);
        }
    }

    public void ShowSelectionArrow()
    {
        if (PullLeft)
        {
            SelectionArrow.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            SelectionArrow.transform.position = new Vector3(MinX - TileWidth, MaxY - Row * TileHeight, 0f);
        }
        else if (PullRight)
        {
            SelectionArrow.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            SelectionArrow.transform.position = new Vector3(MaxX + TileWidth, MaxY - Row * TileHeight, 0f);
        }
        else if (PullDown)
        {
            SelectionArrow.transform.eulerAngles = new Vector3(0f, 0f, 90f);
            SelectionArrow.transform.position = new Vector3(MinX + Column * TileWidth, MinY - TileHeight, 0f);
        }
        else if (PullUp)
        {
            SelectionArrow.transform.eulerAngles = new Vector3(0f, 0f, 270f);
            SelectionArrow.transform.position = new Vector3(MinX + Column * TileWidth, MaxY + TileHeight, 0f);
        }
    }
}