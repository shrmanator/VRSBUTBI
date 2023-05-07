using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameButtons : MonoBehaviour
{
    [SerializeField]
    private SimFileHandler simFileHandler;

    [SerializeField]
    private SimulationController simulationController;

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
        if (GUI.Button(new Rect(120, 10, 120, 30), buttonLabel))
        {
            simulationController.ToggleSimulation();
        }

        // Load STROBO button:
        if (GUI.Button(new Rect(240, 10, 140, 30), "Load STROBO file"))
        {
            simFileHandler.OpenTextFileLoadDialog();
        }

        // Load State button:
        if (GUI.Button(new Rect(380, 10, 110, 30), "Load Saved file"))
        {
            simFileHandler.OpenSimStateLoadDialog();
        }
    }
}
