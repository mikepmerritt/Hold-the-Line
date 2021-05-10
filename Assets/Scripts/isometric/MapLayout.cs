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

    private void Start() 
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

        Debug.Log(Width + " " + Height);
        Debug.Log(Tiles.cellBounds);

        // printing tilemap
        string res = "";
        for (int r = 0; r < Tiles.size.y; r++) 
        {
            for (int c = 0; c < Tiles.size.x; c++) 
            {
                if (tileArray[r, c]) 
                {
                    res += "T";
                }
                else 
                {
                    res += "F";
                }
                res += "\t";
            }
            res += "\n";
        }
        Debug.Log(res);

        CreateBufferedMap(2, 2);

        // printing tilemap
        // string res2 = "";
        // for (int r = 0; r < BufferedMap.GetLength(0); r++) 
        // {
        //     for (int c = 0; c < BufferedMap.GetLength(1); c++) 
        //     {
        //         if (BufferedMap[r, c] == null) 
        //         {
        //             res2 += "notile";
        //         }
        //         else
        //         {
        //             res2 += BufferedMap[r, c];
        //         }
        //         res2 += "\t";
        //     }
        //     res2 += "\n";
        // }
        // Debug.Log(res2);
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(ShiftTiles(2));
        }
    }

    public void CreateBufferedMap(int bufferWidth, int bufferHeight) 
    {
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

    public bool ShiftTiles(int row) 
    {
        Vector3Int[] positions = new Vector3Int[BufferedMap.GetLength(1)];
        TileBase[] removed = new TileBase[BufferedMap.GetLength(1)];

        // saving old tiles
        for (int i = 0; i < BufferedMap.GetLength(1); i++) 
        {
            positions[i] = new Vector3Int(i + MinX - BufferWidth, row + MinY - BufferHeight, 0);
            removed[i] = BufferedMap[row, i];
        }

        // if last space is not empty, move cannot be done
        if (removed[removed.Length - 1] != null)
        {
            Debug.LogError("Invalid move, no space to shift tiles.");
            return false;
        }

        TileBase[] moved = new TileBase[BufferedMap.GetLength(1)];

        // clear leftmost spot
        moved[0] = null;

        // shift all tiles
        for (int i = 1; i < removed.Length; i++)
        {
            moved[i] = removed[i - 1];
        }

        for (int i = 0; i < positions.Length; i++)
        {
            Debug.Log(positions[i] + ": " + moved[i]);
        }

        Tiles.SetTiles(positions, moved);
        UpdateBufferedMap();

        return true;
    }

}
