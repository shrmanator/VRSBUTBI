using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the display of the save and load buttons
/// </summary>
public class SaveButton : MonoBehaviour
{
    private void OnGUI()
    {
        /// <summary>
        /// Displays a button that, when clicked, will save the current game state
        /// </summary>
        if (GUI.Button(new Rect(10, 10, 100, 30), "Save"))
        {
            LoadAndSaveManager.Instance.SaveGame();
        }

        /// <summary>
        /// Displays a button that, when clicked, will load a previously saved game state
        /// </summary>
        if (GUI.Button(new Rect(120, 10, 100, 30), "Load"))
        {
            LoadAndSaveManager.Instance.LoadGame();
        }
    }
}
