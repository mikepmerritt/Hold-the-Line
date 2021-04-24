using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : Unit
{
    public override void UpdateTarget() 
    {
        if (Direction == 'N')
        {
            Target = new Point(Location.X, Location.Y - 2);
        }
        else if (Direction == 'S')
        {
            Target = new Point(Location.X, Location.Y + 2);
        }
        else if (Direction == 'W')
        {
            Target = new Point(Location.X - 2, Location.Y);
        }
        else if (Direction == 'E')
        {
            Target = new Point(Location.X + 2, Location.Y);
        }
    }

    public override void Act() 
    {
        Tile targetTile = Map.GetTileInComposite(Target.Y, Target.X); // fetch target tile to check for units
        if (targetTile != null && targetTile.GetUnit() != null)
        {
            targetTile.GetUnit().Health--;
        }
    }
}