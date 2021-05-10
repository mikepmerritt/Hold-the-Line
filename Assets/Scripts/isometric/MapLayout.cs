using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLayout : MonoBehaviour
{
    private Tilemap tiles;

    private void Start() 
    {
        tiles = GetComponent<Tilemap>();

        bool[,] tileArray = new bool[tiles.size.y, tiles.size.x];
        for (int r = 0; r < tiles.size.y; r++) 
        {
            for (int c = 0; c < tiles.size.x; c++) 
            {
                tileArray[r, c] = tiles.HasTile(new Vector3Int(c + tiles.origin.x, r + tiles.origin.y, 0));
            }
        }

        // printing tilemap
        string res = "";
        for (int r = 0; r < tiles.size.y; r++) 
        {
            for (int c = 0; c < tiles.size.x; c++) 
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
    }

}
