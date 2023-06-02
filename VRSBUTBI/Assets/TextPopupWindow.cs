using UnityEngine;

/// <summary>
/// A class for displaying a pop-up window with instructions in a Unity application.
/// </summary>
public class TextPopupWindow : MonoBehaviour
{
    private bool showPopup = true;
    private Rect popupRect;
    private string[] bulletList = { 
        "1. First, add your models to New_Unity_Project_Data/Imported_Models",
        "2. Press load text file to load in your STROBOSCOPE file",
        "3. Press the play button to begin the simulation" 
    };

    // Create a GUIStyle to set the font size
    private GUIStyle guiStyle = new GUIStyle();

    /// <summary>
    /// Initialize the pop-up window's properties at start. 
    /// </summary>
    private void Start()
    {
        // Set the font size
        guiStyle.fontSize = 20;

        // Set the text color
        guiStyle.normal.textColor = Color.white;

        // Center the popup window
        popupRect = new Rect(Screen.width / 2 - 350, Screen.height / 2 - 125, 700, 250);
    }

    /// <summary>
    /// Displays the pop-up window if showPopup is set to true.
    /// </summary>
    private void OnGUI()
    {
        if (showPopup)
        {
            popupRect = GUI.Window(0, popupRect, ShowPopupWindow, "Start Here");
        }
    }

    /// <summary>
    /// Defines the content and behaviour of the pop-up window. 
    /// Displays instruction text and a button that can close the window.
    /// </summary>
    /// <param name="windowID">The unique identifier for the window.</param>
    private void ShowPopupWindow(int windowID)
    {
        // Add some text to the popup window
        GUI.Label(new Rect(10, 20, popupRect.width - 20, 30), "Welcome to VerbaContruct!", guiStyle);

        // Display bullet list items
        int bulletListTop = 70; // vertical position of the bullet list
        for (int i = 0; i < bulletList.Length; i++)
        {
            GUI.Label(new Rect(10, bulletListTop + (i * 40), popupRect.width - 20, 30), bulletList[i], guiStyle);
        }

        // Add some space below the bullet list
        bulletListTop += (bulletList.Length * 40) + 20;

        // Add an OK button that closes the popup when clicked
        if (GUI.Button(new Rect(popupRect.width / 2 - 50, bulletListTop, 100, 30), "OK"))
        {
            showPopup = false;
        }

        // Make the window draggable
        GUI.DragWindow(new Rect(0, 0, popupRect.width, popupRect.height));
    }
}
