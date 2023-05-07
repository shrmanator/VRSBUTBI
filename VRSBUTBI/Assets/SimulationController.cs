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
            ResumeSimulation();

            if (InitialRun) {
                print("simulation start");
            }
            else {
                print("simulation resumed");
            }

            InitialRun = false;
        }

        else
        {
            PauseSimulation();
            print("simulation stopped");

        }
    }

    private void PauseSimulation()
    {
        Time.timeScale = 0;
    }

    private void ResumeSimulation()
    {
        Time.timeScale = 1;
    }
}
