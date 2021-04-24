using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitsToAdd 
{
    public UnitTemplate[] UnitTemplates;
    public Point[] Points;

    public Dictionary<Point, Unit> BuildDictionary()
    {
        Dictionary<Point, Unit> dictionary = new Dictionary<Point, Unit>();
        for (int i = 0; i < UnitTemplates.Length && i < Points.Length; i++) 
        {
            UnitTemplates[i].SetLocation(Points[i]);
            dictionary.Add(Points[i], UnitTemplates[i].CreateUnit());
        }
        return dictionary;
    }
}
