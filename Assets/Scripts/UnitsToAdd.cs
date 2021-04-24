using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitsToAdd 
{
    public UnitTemplate[] UnitTemplates;

    public Dictionary<Point, Unit> BuildDictionary()
    {
        Dictionary<Point, Unit> dictionary = new Dictionary<Point, Unit>();
        for (int i = 0; i < UnitTemplates.Length; i++) 
        {
            dictionary.Add(UnitTemplates[i].Location, UnitTemplates[i].CreateUnit());
        }
        return dictionary;
    }
}
