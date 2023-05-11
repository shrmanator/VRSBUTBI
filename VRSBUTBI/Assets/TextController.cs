using UnityEngine;

/// <summary>
/// Controls the display of the instructions panel.
/// </summary>
public class TextController : MonoBehaviour
{
    /// <summary>
    /// The GameObject for the instruction panel.
    /// </summary>
    public GameObject instructionPanel;

    private ScenePlayer scenePlayer;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        scenePlayer = GameObject.FindObjectOfType<ScenePlayer>();
        instructionPanel.SetActive(false);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // Show the instructions when the game is in the run state and there is no scene playing
        if (!scenePlayer.IsSceneLoaded() && !scenePlayer.isPlayingScene)
        {
            instructionPanel.SetActive(true);
        }
        else
        {
            // Hide the instructions
            instructionPanel.SetActive(false);
        }
    }
}
