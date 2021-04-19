using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // 2D level array
    private Tile[,] LevelMap;

    // dimensions of the level and some info about how far it can be shifted
    public int Width, Height, HorizontalBuffer, VerticalBuffer;

    // list of units to be added to the level, made in the editor
    private Dictionary<Point, Unit> Units;
    public UnitsToAdd UnitsToAdd;

    // empty tile prefab to load into map
    public GameObject EmptyTilePrefab;

    private void Start()
    {
        // stored in rows then columns
        LevelMap = new Tile[Height + 2 * VerticalBuffer, Width + 2 * HorizontalBuffer];

        // Load dictionary
        Units = UnitsToAdd.BuildDictionary();

        // generating tiles using empty tile prefab
        for (int i = VerticalBuffer, y = 1; y <= Height; i++, y++) 
        {
            for (int j = HorizontalBuffer, x = 1; x <= Width; j++, x++) 
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
    }

    private void Update()
    {

    }

    private void ShiftRow()
    {

    }

    private void ShiftColumn()
    {

    }
}