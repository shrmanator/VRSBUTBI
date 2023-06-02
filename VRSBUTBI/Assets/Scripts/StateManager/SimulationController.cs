using UnityEngine;

/// <summary>
/// Controller class for running and managing a simulation in the scene.
/// </summary>
public class SimulationController : MonoBehaviour
{
    /// <summary>
    /// Flag for checking if the simulation is currently running.
    /// </summary>
    public bool SimulationRunning { get; private set; } = false;

    /// <summary>
    /// Flag for checking if the simulation is on its initial run.
    /// </summary>
    public bool InitialRun { get; private set; } = true;

    /// <summary>
    /// Toggles the running state of the simulation between running and paused.
    /// </summary>
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

    /// <summary>
    /// Handles the start or resume of the simulation.
    /// </summary>
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

    /// <summary>
    /// Pauses the simulation by setting the time scale to 0.
    /// </summary>
    private void PauseSimulation()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// Starts the simulation by setting the time scale to 1.
    /// </summary>
    private void StartSimulation()
    {
        Time.timeScale = 1;
    }
}
