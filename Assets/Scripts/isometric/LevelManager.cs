using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private MapLayout Map;
    private UnitLayout Units;
    private PullControls Controls;

    public Vector2Int Buffers;

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
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int rawLocation = Controls.ConvertMousePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (Controls.CanPull(rawLocation))
            {
                int dimension = Controls.GetPullDimension(rawLocation);
                int direction = Controls.GetPullDirection(rawLocation);

                Map.ShiftTiles(dimension, rawLocation, direction);
                Units.ShiftTiles(dimension, rawLocation, direction);
                Controls.UpdateControls();
            }
        }

    }

}
