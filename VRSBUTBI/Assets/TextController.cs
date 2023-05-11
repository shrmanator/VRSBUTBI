using UnityEngine;

public class TextController : MonoBehaviour
{
    public GameObject instructionText;

    private void Start()
    {
        if (instructionText != null)
        {
            instructionText.SetActive(false);
        }
    }

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
            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 40, 400, 80),
                "Please import a text file or load a simulation state, then press \"Start Simulation\"",
                style);
        }
    }


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
