using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the display of the save and load buttons
/// </summary>
public class InGameButtons : MonoBehaviour
{

    private SaveLoadSimState saveLoadSimState;
    
    private void Start()
    {
        GameObject saveLoadSimStateObject = GameObject.Find("StateManager");
        saveLoadSimState = saveLoadSimStateObject.GetComponent<SaveLoadSimState>();
    }

    private void OnGUI()
    {
        /// <summary>
        /// Displays a button that, when clicked, will save the current game state
        /// </summary>
        if (GUI.Button(new Rect(150, 105, 100, 30), "Save")) {saveLoadSimState.OpenSaveDialog();}

        /// <summary>
        /// Displays a button that, when clicked, will load a previously saved game state
        /// </summary>
        if (GUI.Button(new Rect(270, 105, 100, 30), "Load File")) {saveLoadSimState.OpenSaveDialog();}
    }
}
