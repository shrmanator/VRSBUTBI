using UnityEngine;

public class TextPopupWindow : MonoBehaviour
{
    private bool showPopup = true;
    private Rect popupRect;
    private string[] bulletList = { 
        "First, add your models to New_Unity_Project_Data/Imported_Models",
        "Press load text file to load in your STROBOSCOPE file",
        "Press the play button to begin the simulation" 
    };

    // Create a GUIStyle to set the font size
    private GUIStyle guiStyle = new GUIStyle();

    private void Start()
    {
        // Set the font size
        guiStyle.fontSize = 20; // adjust this value to change the font size

        // Set the text color
        guiStyle.normal.textColor = Color.white; // change this to the desired color

        // Center the popup window
        popupRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 700, 170);
    }

    private void OnGUI()
    {
        if (showPopup)
        {
            popupRect = GUI.Window(0, popupRect, ShowPopupWindow, "Popup Window");
        }
    }

    private void ShowPopupWindow(int windowID)
    {
        // Add some text to the popup window
        GUI.Label(new Rect(10, 20, popupRect.width - 20, 30), "Welcome to VerbaContruct", guiStyle);

        // Display bullet list items
        for (int i = 0; i < bulletList.Length; i++)
        {
            GUI.Label(new Rect(10, 50 + (i * 20), popupRect.width - 20, 30), bulletList[i], guiStyle);
        }

        // Add an OK button that closes the popup when clicked
        if (GUI.Button(new Rect(popupRect.width / 2 - 50, 60 + (bulletList.Length * 20), 100, 20), "OK"))
        {
            showPopup = false;
        }

        // Make the window draggable
        GUI.DragWindow(new Rect(0, 0, popupRect.width, popupRect.height));
    }
}
