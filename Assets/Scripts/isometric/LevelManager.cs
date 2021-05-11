using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private MapLayout Map;
    private UnitLayout Units;
    private PullControls Controls;

    public Vector2Int Buffers;
    public int Row, Column;

    private void Start() 
    {
        Map = GetComponentInChildren<MapLayout>();
        Units = GetComponentInChildren<UnitLayout>();
        Controls = GetComponentInChildren<PullControls>();

        Map.CreateBufferedMap(Buffers.x, Buffers.y);
        Units.CreateBufferedMap(Buffers.x, Buffers.y);
        Controls.DrawInitialControls(Buffers.x, Buffers.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Map.ShiftTiles(1, new Vector2Int(Row, 0), -1)) 
            {
                Units.ShiftTiles(1, new Vector2Int(Row, 0), -1);
                Controls.UpdateControls();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (Map.ShiftTiles(1, new Vector2Int(Row, 0), 1))
            {
                Units.ShiftTiles(1, new Vector2Int(Row, 0), 1);
                Controls.UpdateControls();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (Map.ShiftTiles(0, new Vector2Int(0, Column), -1)) 
            {
                Units.ShiftTiles(0, new Vector2Int(0, Column), -1);
                Controls.UpdateControls();
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (Map.ShiftTiles(0, new Vector2Int(0, Column), 1))
            {
                Units.ShiftTiles(0, new Vector2Int(0, Column), 1);
                Controls.UpdateControls();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Controls.ConvertMousePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }

    }

}
