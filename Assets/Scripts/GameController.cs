using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private char PullDirection;
    private Map Map;
    private List<Unit> Player1Units, Player2Units; 
    private bool IsPulling, IsPushing, ShowOverlay;
    private int SelectedUnit;

    private void Start()
    {
        Map = FindObjectOfType<Map>();
        IsPulling = true;
        IsPushing = false;
        ShowOverlay = false;
        SelectedUnit = 0;
    }

    private void Update() 
    {
        if (Player1Units == null && Player2Units == null)
        {
            Map.CorrectAllUnitPositions(out Player1Units, out Player2Units);
            
            Map.ConstructCompositeMap();

            // load initial targets
            UpdateAllTargets();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (IsPulling)
            {
                IsPulling = false;
                IsPushing = true;
                Map.HideSelectionArrow();
                SelectedUnit = 0;
                Map.ShowSelectionBox(Player1Units[SelectedUnit].Location.Y, Player1Units[SelectedUnit].Location.X);
            }
            else if (IsPushing)
            {
                IsPushing = false;
                IsPulling = true;
                Map.ShowSelectionArrow();
                Map.HideSelectionBox();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ShowOverlay = !ShowOverlay;
        }

        if (ShowOverlay)
        {
            // correct targets again before showing
            UpdateAllTargets();

            Map.ShowOverlay(Player1Units, Player2Units);
        }
        else
        {
            Map.HideOverlay();
        }
        

        if (IsPulling)
        {
            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                Map.SelectNextClockwise();
            }
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                Map.SelectNextAnticlockwise();
            }
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                Map.PullSelected();
                Map.DisplayMap();
                Map.CorrectAllUnitPositions(out Player1Units, out Player2Units);
                UpdateAllTargets();
            }
            Map.ShowSelectionArrow();
        }
        else if (IsPushing) 
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SelectedUnit = (SelectedUnit + 1) % Player1Units.Count;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                SelectedUnit = (SelectedUnit - 1 + Player1Units.Count) % Player1Units.Count;
            }

            // push up
            if (Input.GetKeyDown(KeyCode.W))
            {
                Player1Units[SelectedUnit].Push('N');
            }
            // push down
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Player1Units[SelectedUnit].Push('S');
            }
            // push left
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Player1Units[SelectedUnit].Push('W');
            }
            // push right
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Player1Units[SelectedUnit].Push('E');
            }
             
            Map.ShowSelectionBox(Player1Units[SelectedUnit].Location.Y, Player1Units[SelectedUnit].Location.X);
        }

        // THIS IS DEBUG ONLY
        // REMOVE BEFORE SUBMISSION
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (Unit unit in Player1Units)
            {
                unit.Act();
            }
            foreach (Unit unit in Player2Units)
            {
                unit.Act();
            }
        }
    }

    private void UpdateAllTargets()
    {
        // correcting targets
        foreach (Unit unit in Player1Units)
        {
            unit.UpdateTarget();
        }
        foreach (Unit unit in Player2Units)
        {
            unit.UpdateTarget();
        }
    }

}
