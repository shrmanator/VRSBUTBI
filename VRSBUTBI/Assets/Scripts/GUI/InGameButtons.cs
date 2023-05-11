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

    // Space to leave at the edge of screen
    int endSpace = 20;

    // Space to leave between buttons
    int spacer = 5;

    // Y coordinate of buttons
    int buttonY = 10;

    // Height of button boxes
    int buttonHeight = 30;

    int saveSceneWidth = 90;

    int loadTextWidth = 110;

    int loadSceneWidth = 90;

    int playbackControlWidth = 100;

    
    
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
        //Buttons aligned to the left
        int leftButtonsPosition = endSpace;
        // Save button:
        if (GUI.Button(new Rect(leftButtonsPosition, buttonY, saveSceneWidth, buttonHeight), 
            "Save Scene"))
        {
            PauseScenePlayer();
            simFileHandler.OpenGameSaveDialog();
        }

        leftButtonsPosition += (saveSceneWidth + spacer);

        // Load STROBO button:
        if (GUI.Button(new Rect(leftButtonsPosition, buttonY, loadTextWidth, buttonHeight), 
            "Load Text File"))
        {
            PauseScenePlayer();
            simFileHandler.OpenTextFileLoadDialog();
        }

        leftButtonsPosition += (loadTextWidth + spacer);

        // Load State button:
        if (GUI.Button(new Rect(leftButtonsPosition, buttonY, loadSceneWidth, buttonHeight), 
            "Load Scene"))
        {
            PauseScenePlayer();
            simFileHandler.OpenSimStateLoadDialog();
        }

        leftButtonsPosition += (loadSceneWidth+ spacer);


        // Buttons alighned to the right. Buttons are listed from right to left in order of appearance
        int rightButtonsPosition = Screen.width - endSpace;

        rightButtonsPosition -= playbackControlWidth;
        // Start : Stop simulation button:
        //string buttonLabel = simulationController.SimulationRunning ? "Stop Simulation" : "Resume Simulation";
        //if (simulationController.InitialRun) { buttonLabel = "Start Simulation";}
        if (GUI.Button(new Rect(rightButtonsPosition, buttonY, playbackControlWidth, buttonHeight),
            "Clear Scene"))
        {
            ScenePlayer.Player.ClearScene();
        }

        rightButtonsPosition -= (playbackControlWidth + spacer);
        if (GUI.Button(new Rect(rightButtonsPosition, buttonY, playbackControlWidth, buttonHeight),
            "Reset Scene"))
        {
            ScenePlayer.Player.ResetScene();
        }

        rightButtonsPosition -= (playbackControlWidth + spacer);
        if (GUI.Button(new Rect(rightButtonsPosition, buttonY, playbackControlWidth, buttonHeight),
            "Pause Scene"))
        {
            ScenePlayer.Player.PauseScene();
        }


        rightButtonsPosition -= (playbackControlWidth + spacer);

        if (GUI.Button(new Rect(rightButtonsPosition, buttonY, playbackControlWidth, buttonHeight),
            "Play Scene"))
        {
            ScenePlayer.Player.PlayScene();
        }   
    }

    private void PauseScenePlayer()
    {
        if (!ScenePlayer.Player.isPaused){ScenePlayer.Player.PauseScene();}
    }
}
