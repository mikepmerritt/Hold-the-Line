using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PullControls : MonoBehaviour
{

    private Tilemap Tiles;
    private int MinX, MinY, MaxX, MaxY;
    private int Width, Height;
    private int BufferWidth, BufferHeight;
    private MapLayout Map;
    private UnitLayout Units;
    
    // arrows to add
    public TileBase SE, SW, NE, NW;

    public void DrawInitialControls(int bufferWidth, int bufferHeight)
    {
        Tiles = GetComponent<Tilemap>();

        Map = FindObjectOfType<MapLayout>();
        Units = FindObjectOfType<UnitLayout>();

        MinX = Map.GetMinCell().x;
        MinY = Map.GetMinCell().y;
        Width = Map.GetDimensions().x;
        Height = Map.GetDimensions().y;
        MaxX = Width - MinX + 1;
        MaxY = Height - MinY + 1;
        BufferWidth = bufferWidth;
        BufferHeight = bufferHeight;

        Debug.Log(Width + 2 * BufferWidth);

        UpdateControls();
    }

    public void UpdateControls()
    {
        // checking all columns to see if arrows should be added
        for (int c = 0; c < Width + 2 * BufferWidth; c++)
        {
            // check if shifting up is allowed
            if (Map.CanShiftTiles(0, new Vector2Int(0, c), 1) && 
                Units.CanShiftTiles(0, new Vector2Int(0, c), 1) &&
                !Map.TilesAreEmpty(0, new Vector2Int(0, c)))
            {
                Tiles.SetTile(new Vector3Int(c + MinX - BufferWidth, MinY + Height + BufferHeight, 0), NW);
            }
            else 
            {
                Tiles.SetTile(new Vector3Int(c + MinX - BufferWidth, MinY + Height + BufferHeight, 0), null);
            }
            // check if shifting up is allowed
            if (Map.CanShiftTiles(0, new Vector2Int(0, c), -1) && 
                Units.CanShiftTiles(0, new Vector2Int(0, c), -1) &&
                !Map.TilesAreEmpty(0, new Vector2Int(0, c)))
            {
                Tiles.SetTile(new Vector3Int(c + MinX - BufferWidth, MinY - BufferHeight - 1, 0), SE);
            }
            else 
            {
                Tiles.SetTile(new Vector3Int(c + MinX - BufferWidth, MinY - BufferHeight - 1, 0), null);
            }
        }

        // checking all columns to see if arrows should be added
        for (int r = 0; r < Height + 2 * BufferHeight; r++)
        {
            // check if shifting right is allowed
            if (Map.CanShiftTiles(1, new Vector2Int(r, 0), 1) && 
                Units.CanShiftTiles(1, new Vector2Int(r, 0), 1) &&
                !Map.TilesAreEmpty(1, new Vector2Int(r, 0)))
            {
                Tiles.SetTile(new Vector3Int(MinX + Width + BufferWidth, r + MinY - BufferHeight, 0), NE);
            }
            else 
            {
                Tiles.SetTile(new Vector3Int(MinX + Width + BufferWidth, r + MinY - BufferHeight, 0), null);
            }
            // check if shifting up is allowed
            if (Map.CanShiftTiles(1, new Vector2Int(r, 0), -1) && 
                Units.CanShiftTiles(1, new Vector2Int(r, 0), -1) &&
                !Map.TilesAreEmpty(1, new Vector2Int(r, 0)))
            {
                Tiles.SetTile(new Vector3Int(MinX - BufferWidth - 1, r + MinY - BufferHeight, 0), SW);
            }
            else 
            {
                Tiles.SetTile(new Vector3Int(MinX - BufferWidth - 1, r + MinY - BufferHeight, 0), null);
            }
        }
    }

    public Vector2Int ConvertMousePosition(Vector3 mousePosition)
    {
        Vector3Int cellPosition = Tiles.WorldToCell(mousePosition);
        return new Vector2Int(cellPosition.x, cellPosition.y);
    }

}
