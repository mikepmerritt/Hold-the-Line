using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private char PullDirection;
    private Map Map;
    private List<Unit> Player1Units, Player2Units; 
    private bool IsPulling, IsPushing, ShowOverlay;
    private int SelectedUnit;

    // turns
    public int TurnsRemaining;
    private bool TurnOver;

    // UI
    private OutputBox Output;
    public GameObject WinButton;

    // Build indexes
    private const int TutorialSelect = 2;
    private const int MainMenu = 0;

    private void Start()
    {
        Map = FindObjectOfType<Map>();
        IsPulling = true;
        IsPushing = false;
        ShowOverlay = false;
        SelectedUnit = 0;
        TurnOver = false;
        Output = FindObjectOfType<OutputBox>();
    }

    private void Update() 
    {
        // initial loads that need to happen sometime after start
        if (Player1Units == null && Player2Units == null)
        {
            Map.CorrectAllUnitPositions(out Player1Units, out Player2Units);
            
            Map.ConstructCompositeMap();

            // load initial targets
            UpdateAllTargets();
        }

        // overlay controls (can occur at any time)
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
        
        if (!TurnOver) {
            // checking for toggling the method of move during a turn
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
            // checking for moves during a turn
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
                    TurnOver = Map.PullSelected();
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
                    TurnOver = Player1Units[SelectedUnit].Push('N');
                }
                // push down
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    TurnOver = Player1Units[SelectedUnit].Push('S');
                }
                // push left
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    TurnOver = Player1Units[SelectedUnit].Push('W');
                }
                // push right
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    TurnOver = Player1Units[SelectedUnit].Push('E');
                }
                
                Map.ShowSelectionBox(Player1Units[SelectedUnit].Location.Y, Player1Units[SelectedUnit].Location.X);
            }
        }

        if (TurnOver && TurnsRemaining > 0)
        {
            Map.HideSelectionArrow();
            Map.HideSelectionBox();
            TurnsRemaining--;

            // end player 1 turn
            foreach (Unit unit in Player1Units)
            {
                unit.Act();
            }

            // clear out player 2's dead units
            foreach (Unit unit in Player2Units)
            {
                unit.Clear();
            }
            // update lists
            Map.CorrectAllUnitPositions(out Player1Units, out Player2Units);

            // end player 2 turn
            foreach(Unit unit in Player2Units)
            {
                unit.Act();
            }

            // clear out player 1's dead units
            foreach (Unit unit in Player1Units)
            {
                unit.Clear();
            }
            // update lists
            Map.CorrectAllUnitPositions(out Player1Units, out Player2Units);

            TurnOver = false; // set turn over to false only if turns are left

            // check win conditions
            if (Player2Units.Count == 0) 
            {
                Output.ShowText("You won!");
                WinButton.SetActive(true);
                TurnsRemaining = 0;
                TurnOver = true;
            }
            if (Player1Units.Count == 0)
            {
                Output.ShowWarning("You lost!");
                TurnsRemaining = 0;
                TurnOver = true;
            }
        }
        else if (TurnsRemaining == 0)
        {
            Map.HideSelectionArrow();
            Map.HideSelectionBox();
            TurnOver = true;
            TurnsRemaining--;
            if (Player2Units.Count != 0)
            {
                Output.ShowWarning("You are out of moves. Please retry.");
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

    public int GetTurnsRemaining()
    {
        return Mathf.Max(0, TurnsRemaining);
    }

    public void ResetLevel()
    {
        // reload current level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToTutorialMenu()
    {
        SceneManager.LoadScene(TutorialSelect);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }

}
