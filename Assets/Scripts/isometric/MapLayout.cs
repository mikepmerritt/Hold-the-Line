using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLayout : MonoBehaviour
{

    private Tilemap Tiles;
    private int MinX, MinY, MaxX, MaxY;
    private int Width, Height;
    private int BufferWidth, BufferHeight;
    private TileBase[,] BufferedMap;

    public void CreateBufferedMap(int bufferWidth, int bufferHeight) 
    {
        Tiles = GetComponent<Tilemap>();

        bool[,] tileArray = new bool[Tiles.size.y, Tiles.size.x];
        for (int r = 0; r < Tiles.size.y; r++) 
        {
            for (int c = 0; c < Tiles.size.x; c++) 
            {
                if(Tiles.HasTile(new Vector3Int(c + Tiles.origin.x, r + Tiles.origin.y, 0))) 
                {
                    MinX = Mathf.Min(c + Tiles.origin.x, MinX);
                    MaxX = Mathf.Max(c + Tiles.origin.x, MaxX);
                    MinY = Mathf.Min(r + Tiles.origin.y, MinY);
                    MaxY = Mathf.Max(r + Tiles.origin.y, MaxY);
                }
                tileArray[r, c] = Tiles.HasTile(new Vector3Int(c + Tiles.origin.x, r + Tiles.origin.y, 0));
            }
        }

        Width = MaxX - MinX + 1;
        Height = MaxY - MinY + 1;

        BufferedMap = new TileBase[Height + bufferHeight * 2, Width + bufferWidth * 2];

        BufferWidth = bufferWidth;
        BufferHeight = bufferHeight;

        for (int r = 0; r < Height; r++) 
        {
            for (int c = 0; c < Width; c++) 
            {
                BufferedMap[r + bufferHeight, c + bufferWidth] = Tiles.GetTile(new Vector3Int(c + MinX, r + MinY, 0));
            }
        }
    }

    public void UpdateBufferedMap() 
    {
        for (int r = 0; r < BufferedMap.GetLength(0); r++)
        {
            for (int c = 0; c < BufferedMap.GetLength(1); c++)
            {
                BufferedMap[r, c] = Tiles.GetTile(new Vector3Int(c + MinX - BufferWidth, r + MinY - BufferHeight, 0));
            }
        }
    }

    // returns the top left cell in the grid in the form (minCol, minRow)
    public Vector2Int GetMinCell()
    {
        return new Vector2Int(MinX, MinY);
    }

    // returns the width and height of the grid in the form (width, height)
    public Vector2Int GetDimensions() 
    {
        return new Vector2Int(Width, Height);
    }

    // location should be given in the form (row, column)
    public bool ShiftTiles(int dimension, Vector2Int location, int direction) 
    {
        // int dimension;
        // // if shifting a row
        // if (location.y == 0) 
        // {
        //     dimension = 1;
        // }
        // // else shifting a column
        // else 
        // {
        //     dimension = 0;
        // }

        Vector3Int[] positions = new Vector3Int[BufferedMap.GetLength(dimension)];
        TileBase[] removed = new TileBase[BufferedMap.GetLength(dimension)];

        // saving old tiles
        // if shifting a row, go through the columns to save the tiles
        if (dimension == 1) 
        {
            for (int i = 0; i < BufferedMap.GetLength(dimension); i++) 
            {
                positions[i] = new Vector3Int(i + MinX - BufferWidth, location.x + MinY - BufferHeight, 0);
                removed[i] = BufferedMap[location.x, i];
            }
        }
        // else shifting a column, go through the rows to save the tiles
        else 
        {
            for (int i = 0; i < BufferedMap.GetLength(dimension); i++) 
            {
                positions[i] = new Vector3Int(location.y + MinX - BufferWidth, i + MinY - BufferHeight, 0);
                removed[i] = BufferedMap[i, location.y];
            }
        }

        if (direction == 1) 
        {
            // if last space is not empty, move cannot be done
            if (removed[removed.Length - 1] != null)
            {
                Debug.LogError("Invalid move, no space to shift tiles.");
                return false;
            }

            TileBase[] moved = new TileBase[BufferedMap.GetLength(dimension)];

            // clear leftmost spot
            moved[0] = null;

            // shift all tiles
            for (int i = 1; i < removed.Length; i++)
            {
                moved[i] = removed[i - 1];
            }

            Tiles.SetTiles(positions, moved);
            UpdateBufferedMap();
        }
        else if (direction == -1) 
        {
            // if first space is not empty, move cannot be done
            if (removed[0] != null)
            {
                Debug.LogError("Invalid move, no space to shift tiles.");
                return false;
            }

            TileBase[] moved = new TileBase[BufferedMap.GetLength(dimension)];

            // clear rightmost spot
            moved[moved.Length - 1] = null;

            // shift all tiles
            for (int i = 0; i < removed.Length - 1; i++) 
            {
                moved[i] = removed[i + 1];
            }

            Tiles.SetTiles(positions, moved);
            UpdateBufferedMap();
        }

        return true;
    }

    public bool CanShiftTiles(int dimension, Vector2Int location, int direction)
    {
        // if shifting a row
        if (dimension == 1) 
        {
            // if shifting right
            if (direction == 1)
            {
                return BufferedMap[location.x, BufferedMap.GetLength(1) - 1] == null;
            }
            // else shifting left
            else
            {
                return BufferedMap[location.x, 0] == null;
            }
        }
        // else shifting a column
        else 
        {
            // if shifting up
            if (direction == 1)
            {
                return BufferedMap[BufferedMap.GetLength(0) - 1, location.y] == null;
            }
            // else shifting down
            else
            {
                return BufferedMap[0, location.y] == null;
            }
        }
    }

    public bool TilesAreEmpty(int dimension, Vector2Int location)
    {
        // if shifting a row
        if (dimension == 1) 
        {
            for (int i = 0; i < BufferedMap.GetLength(1); i++)
            {
                if (BufferedMap[location.x, i] != null)
                {
                    return false;
                }
            }
            return true;
        }
        // else shifting a column
        else 
        {
            for (int i = 0; i < BufferedMap.GetLength(0); i++)
            {
                if (BufferedMap[i, location.y] != null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
