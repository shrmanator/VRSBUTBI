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
    
    /// <summary>
    /// Finds the "StateManager" object and gets the SimFileHandler component.
    /// </summary>
    private void Start()
        {
            GameObject simFileHandlerObject = GameObject.Find("StateManager");
            simFileHandler = simFileHandlerObject.GetComponent<SimFileHandler>();
        }

    private void OnGUI()
    {
        /// <summary>
        /// Displays a button that, when clicked, will show the save dialog.
        /// </summary>
        if (GUI.Button(new Rect(40, 10, 100, 30), "Save")) {simFileHandler.OpenGameSaveDialog();}
    }

}
