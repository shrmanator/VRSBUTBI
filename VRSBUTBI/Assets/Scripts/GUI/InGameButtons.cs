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
        if (GUI.Button(new Rect(40, 10, 100, 30), "Save"))
        {
            simFileHandler.OpenGameSaveDialog();
        }

        // Start : Stop simulation button:
        string buttonLabel = simulationController.SimulationRunning ? "Stop Simulation" : "Resume Simulation";
        if (simulationController.InitialRun) { buttonLabel = "Start Simulation";}
        if (GUI.Button(new Rect(150, 10, 150, 30), buttonLabel))
        {
            simulationController.ToggleSimulation();
        }
    }
}
