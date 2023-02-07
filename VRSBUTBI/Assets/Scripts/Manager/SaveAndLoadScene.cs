using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// This class implements the save and load prompt for the game.
/// </summary>
public class SaveLoadPrompt : MonoBehaviour
{
    /// <summary>
    /// The save button.
    /// </summary>
    public Button saveButton;

    /// <summary>
    /// The load button.
    /// </summary>
    public Button loadButton;

    private string savePath;

    /// <summary>
    /// Initializes the save and load buttons.
    /// </summary>
    private void Start()
    {
        // Create the save button
        saveButton = CreateButton("Save", new Vector3(0, 100, 0));
        saveButton.onClick.AddListener(SaveCurrentScene);

        // Create the load button
        loadButton = CreateButton("Load", new Vector3(0, 50, 0));
        loadButton.onClick.AddListener(LoadScene);
    }

    /// <summary>
    /// Creates a new button with the specified text and position.
    /// </summary>
    /// <param name="text">The text to display on the button.</param>
    /// <param name="position">The position of the button in the scene.</param>
    /// <returns>The new button component.</returns>
    private Button CreateButton(string text, Vector3 position)
    {
        // Create a new GameObject for the button
        GameObject buttonGO = new GameObject(text + " Button");
        buttonGO.transform.SetParent(transform);
        buttonGO.transform.localPosition = position;

        // Add a Button component to the GameObject
        Button button = buttonGO.AddComponent<Button>();

        // Create a text component for the button label
        Text label = buttonGO.AddComponent<Text>();
        label.text = text;
        label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        // Add a CanvasRenderer component for the button to be displayed
        buttonGO.AddComponent<CanvasRenderer>();

        // Return the Button component
        return button;
    }

    /// <summary>
    /// Saves the current scene to a file specified by the user.
    /// </summary>
    public void SaveCurrentScene()
    {
        savePath = EditorUtility.SaveFilePanel("Save Scene", "", "Scene", "unity");

        if (!string.IsNullOrEmpty(savePath))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.SaveScene(currentScene, savePath);
        }
    }

    /// <summary>
    /// Loads the specified scene.
    /// </summary>
    public void LoadScene()
    {
        string loadPath = EditorUtility.OpenFilePanel("Open Scene", "", "unity");

        if (!string.IsNullOrEmpty(loadPath))
        {
            SceneManager.LoadScene(loadPath);
        }
    }
}
