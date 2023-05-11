using UnityEngine;

/// <summary>
/// Displays instructions on the screen when the game is in the run state, but the simulation is not playing.
/// </summary>
public class TextController : MonoBehaviour
{
    /// <summary>
    /// The UI GameObject containing the instruction text.
    /// </summary>
    public GameObject instructionText;


    /// <summary>
    /// Initializes the instruction text GameObject.
    /// </summary>
    private void Start()
    {
        if (instructionText != null)
        {
            instructionText.SetActive(false);
        }
    }

    /// <summary>
    /// Displays the instructions on the screen when the game is in the run state, but the simulation is not playing.
    /// </summary>
    void OnGUI()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying) return;
        #endif

        if (!ScenePlayer.Player.IsScenePlaying())
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 24;
            GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 80, 800, 160),
                "1. Add models to New_unity_Project_Data/Imported_Models\n" +
                "2. You can import a text file using the 'load text file' button\n" +
                "3. You can load a simulation state using the 'load scene' button\n" +
                "4. Press the play button to start the simulation",
            style);
        }
    }

    /// <summary>
    /// Updates the state of the instruction text GameObject based on whether the simulation is playing or not.
    /// </summary>
    private void Update()
    {
        if (Application.isPlaying && instructionText != null && !ScenePlayer.Player.IsScenePlaying())
        {
            instructionText.SetActive(true);
        }
        else if (instructionText != null)
        {
            instructionText.SetActive(false);
        }
    }
}
