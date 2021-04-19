using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitsToAdd 
{
    public Unit[] Units;
    public Point[] Points;

    public Dictionary<Point, Unit> BuildDictionary()
    {
        Dictionary<Point, Unit> dictionary = new Dictionary<Point, Unit>();
        for (int i = 0; i < Units.Length && i < Points.Length; i++) 
        {
            dictionary.Add(Points[i], Units[i]);
        }
        return dictionary;
    }
}
