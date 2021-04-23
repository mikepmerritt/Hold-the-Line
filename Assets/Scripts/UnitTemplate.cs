using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTemplate
{
    public GameObject UnitModel;
    public Sprite Sprite;
    public Point Location;
    public char Direction;
    public int Health, Team;
    private Unit Unit;

    public Unit CreateUnit() {
        Unit = MonoBehaviour.Instantiate(UnitModel, new Vector3(0f, 0f, -20f), Quaternion.identity).GetComponent<Unit>();
        Unit.Sprite = Sprite;
        Unit.Location = Location;
        Unit.Direction = Direction;
        Unit.Health = Health;
        Unit.Team = Team;
        return Unit;
    }

}