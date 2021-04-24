using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : Unit
{
    public override void UpdateTarget() 
    {
        if (Direction == 'N')
        {
            Target = new Point(Location.X, Location.Y - 1);
        }
        else if (Direction == 'S')
        {
            Target = new Point(Location.X, Location.Y + 1);
        }
        else if (Direction == 'W')
        {
            Target = new Point(Location.X - 1, Location.Y);
        }
        else if (Direction == 'E')
        {
            Target = new Point(Location.X + 1, Location.Y);
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