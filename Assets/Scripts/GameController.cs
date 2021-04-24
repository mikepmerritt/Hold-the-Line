using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private char PullDirection;
    private Map Map;
    private List<Unit> Player1Units, Player2Units; 
    private bool IsPulling, IsPushing, ShowOverlay;

    private void Start()
    {
        Map = FindObjectOfType<Map>();
        IsPulling = true;
        IsPushing = false;
        ShowOverlay = false;
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
            }
            else if (IsPushing)
            {
                IsPushing = false;
                IsPulling = true;
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
            if (Input.GetKeyDown(KeyCode.W)) 
            {
                Map.SelectNextClockwise();
            }
            if (Input.GetKeyDown(KeyCode.S)) 
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
            // not implemented yet
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
