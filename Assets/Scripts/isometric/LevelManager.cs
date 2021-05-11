using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private MapLayout map;
    private UnitLayout units;

    public Vector2Int Buffers;
    public int Row, Column;

    private void Start() 
    {
        map = GetComponentInChildren<MapLayout>();
        units = GetComponentInChildren<UnitLayout>();

        map.CreateBufferedMap(Buffers.x, Buffers.y);
        units.CreateBufferedMap(Buffers.x, Buffers.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (map.ShiftTiles(new Vector2Int(Row, 0), -1)) 
            {
                units.ShiftTiles(new Vector2Int(Row, 0), -1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (map.ShiftTiles(new Vector2Int(Row, 0), 1))
            {
                units.ShiftTiles(new Vector2Int(Row, 0), 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (map.ShiftTiles(new Vector2Int(0, Column), -1)) 
            {
                units.ShiftTiles(new Vector2Int(0, Column), -1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (map.ShiftTiles(new Vector2Int(0, Column), 1))
            {
                units.ShiftTiles(new Vector2Int(0, Column), 1);
            }
        }
    }

}
