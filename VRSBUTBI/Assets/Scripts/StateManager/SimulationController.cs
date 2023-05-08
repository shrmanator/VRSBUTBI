using UnityEngine;

public class SimulationController : MonoBehaviour
{
    public bool SimulationRunning { get; private set; } = false;
    public bool InitialRun { get; private set; } = true;

    public void ToggleSimulation()
    {
        SimulationRunning = !SimulationRunning;

        if (SimulationRunning)
        {
            HandleSimulationStartOrResume();
        }
        else
        {
            PauseSimulation();
            Debug.Log("Simulation stopped");
        }
    }

    private void HandleSimulationStartOrResume()
    {
        StartSimulation();

        if (InitialRun)
        {
            Debug.Log("Simulation started");
        }
        else
        {
            Debug.Log("Simulation resumed");
        }

        InitialRun = false;
    }

    private void PauseSimulation()
    {
        Time.timeScale = 0;
    }

    private void StartSimulation()
    {
        Time.timeScale = 1;
    }
}
