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

    private void Start()
    {
        // Calculate the position of the popup window to center it on the screen
        float windowWidth = 500;
        float windowHeight = 400;
        float windowX = (Screen.width - windowWidth) / 2;
        float windowY = (Screen.height - windowHeight) / 2;

        popupRect = new Rect(windowX, windowY, windowWidth, windowHeight);
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
        GUI.Label(new Rect(10, 20, popupRect.width - 20, 60), "Welcome to VerbaContruct");

        // Display bullet list items
        for (int i = 0; i < bulletList.Length; i++)
        {
            GUI.Label(new Rect(10, 80 + (i * 30), popupRect.width - 20, 60), bulletList[i]);
        }

        // Add an OK button that closes the popup when clicked
        if (GUI.Button(new Rect(popupRect.width / 2 - 50, 90 + (bulletList.Length * 30), 100, 30), "OK"))
        {
            showPopup = false;
        }

        // Make the window draggable
        GUI.DragWindow(new Rect(0, 0, popupRect.width, popupRect.height));
    }
}
