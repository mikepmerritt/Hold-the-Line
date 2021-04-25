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
    protected Map Map;

    // UI
    private OutputBox Output;

    private void Awake() 
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.sprite = Sprite;
        Map = FindObjectOfType<Map>();
        Output = FindObjectOfType<OutputBox>();
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

    public virtual bool Push(char direction)
    {
        if (Map.IsLocked(Location.Y, Location.X))
        {
            Debug.LogError("The unit is on a locked tile and cannot be pushed.");
            return false;
        }

        if (direction == 'N')
        {
            Tile destination = Map.GetTileInComposite(Location.Y - 1, Location.X);
            if (destination == null || destination.GetUnit() != null || Map.IsLocked(Location.Y - 1, Location.X))
            {
                Output.ShowError("Cannot push into this location.");
                return false;
            }
            else 
            {
                Tile previous = Map.GetTileInComposite(Location.Y, Location.X);
                previous.RemoveUnit();
                destination.SetUnit(this);
                Location = new Point(Location.X, Location.Y - 1);
                UpdateTarget();
                Map.DisplayMap();
                return true;
            }
        }
        else if (direction == 'S')
        {
            Tile destination = Map.GetTileInComposite(Location.Y + 1, Location.X);
            if (destination == null || destination.GetUnit() != null || Map.IsLocked(Location.Y + 1, Location.X))
            {
                Output.ShowError("Cannot push into this location.");
                return false;
            }
            else 
            {
                Tile previous = Map.GetTileInComposite(Location.Y, Location.X);
                previous.RemoveUnit();
                destination.SetUnit(this);
                Location = new Point(Location.X, Location.Y + 1);
                UpdateTarget();
                Map.DisplayMap();
                return true;
            }
        }
        else if (direction == 'W')
        {
            Tile destination = Map.GetTileInComposite(Location.Y, Location.X - 1);
            if (destination == null || destination.GetUnit() != null || Map.IsLocked(Location.Y, Location.X - 1))
            {
                Output.ShowError("Cannot push into this location.");
                return false;
            }
            else 
            {
                Tile previous = Map.GetTileInComposite(Location.Y, Location.X);
                previous.RemoveUnit();
                destination.SetUnit(this);
                Location = new Point(Location.X - 1, Location.Y);
                UpdateTarget();
                Map.DisplayMap();
                return true;
            }
        }
        else if (direction == 'E')
        {
            Tile destination = Map.GetTileInComposite(Location.Y, Location.X + 1);
            if (destination == null || destination.GetUnit() != null || Map.IsLocked(Location.Y, Location.X + 1))
            {
                Output.ShowError("Cannot push into this location.");
                return false;
            }
            else 
            {
                Tile previous = Map.GetTileInComposite(Location.Y, Location.X);
                previous.RemoveUnit();
                destination.SetUnit(this);
                Location = new Point(Location.X + 1, Location.Y);
                UpdateTarget();
                Map.DisplayMap();
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public void Clear() 
    {
        if (Health == 0)
        {
            Tile location = Map.GetTileInComposite(Location.Y, Location.X);
            location.RemoveUnit();
            transform.position = new Vector3(0f, 0f, -20f); // hide behind camera
            Map.DisplayMap();
            Destroy(gameObject);
        }
    }
}
