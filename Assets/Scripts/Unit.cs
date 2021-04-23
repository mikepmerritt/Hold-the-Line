using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public Sprite Sprite; 
    public SpriteRenderer SpriteRenderer;
    public Point Location, Target;
    public char Direction; // direction that the unit is facing in
    public int Health, Team;

    private void Awake() 
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.sprite = Sprite;
    }

    public void UpdateLayer(int layer) 
    {
        // fixing an issue where the sprite did not load
        SpriteRenderer.sprite = Sprite;
        SpriteRenderer.sortingOrder = layer;
    }

    public virtual void UpdateTarget() 
    {
        // behavior to find target tile
    }

    public virtual void Act() 
    {
        // behavior to perform at end of turn
    }

    public override string ToString()
    {
        return "" + Team + Health;
    }
}
