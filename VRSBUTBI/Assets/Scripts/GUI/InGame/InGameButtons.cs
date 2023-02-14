using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the display of the save and load buttons
/// </summary>
public class InGameButtons : MonoBehaviour
{

    private SimFileHandler simFileHandler;
    
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
        if (GUI.Button(new Rect(40, 10, 100, 30), "Save")) {simFileHandler.OpenSaveDialog();}

        /// <summary>
        /// Displays a button that, when clicked, will show the load dialog.
        /// </summary>
        if (GUI.Button(new Rect(150, 10, 100, 30), "Load File")) {simFileHandler.OpenLoadDialog();}
    }
}
