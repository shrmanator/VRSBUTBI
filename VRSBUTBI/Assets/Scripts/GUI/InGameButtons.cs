using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Controls how the in-game buttons are displayed in the scene.
/// </summary>
public class InGameButtons : MonoBehaviour
{
    private SimFileHandler simFileHandler;
    private SimulationController simulationController;
    
    /// <summary>
    /// Finds the "StateManager" object and gets the SimFileHandler component.
    /// </summary>
    private void Start()
        {
            GameObject simFileHandlerObject = GameObject.Find("StateManager");
            simFileHandler = simFileHandlerObject.GetComponent<SimFileHandler>();
            simulationController = simFileHandlerObject.GetComponent<SimulationController>();
        }

    private void OnGUI()
    {
        // Save button:
        if (GUI.Button(new Rect(20, 10, 100, 30), "Save"))
        {
            simFileHandler.OpenGameSaveDialog();
        }

        // Start : Stop simulation button:
        string buttonLabel = simulationController.SimulationRunning ? "Stop Simulation" : "Resume Simulation";
        if (simulationController.InitialRun) { buttonLabel = "Start Simulation";}
        if (GUI.Button(new Rect(120, 10, 130, 30), buttonLabel))
        {
            simulationController.ToggleSimulation();
        }

        // Load STROBO button:
        if (GUI.Button(new Rect(250, 10, 140, 30), "Load STROBO file"))
        {
            simFileHandler.OpenTextFileLoadDialog();
        }

        // Load State button:
        if (GUI.Button(new Rect(390, 10, 110, 30), "Load Saved file"))
        {
            simFileHandler.OpenSimStateLoadDialog();
        }
    }
}
