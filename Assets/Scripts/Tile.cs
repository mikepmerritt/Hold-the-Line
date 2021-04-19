using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Sprite Texture;
    public Unit Unit;

    // update the unit stored in the current tile
    // returns if the new unit was successfully added to the tile
    public bool SetUnit(Unit unit)
    {
        if (Unit == null)
        {
            Unit = unit;
            return true; // unit added successfully
        }
        else
        {
            return false; // cannot put two units on same tile
        }
    }

    public Unit GetUnit() 
    {
        return Unit;
    }
}
