using UnityEngine;
using SimpleFileBrowser; 

/// <summary>
/// Implements the load and save functions
/// </summary>
public class LoadAndSaveManager : MonoBehaviour
{
    /// <summary>
    /// Instance of the LoadAndSaveManager to allow for easy access in other scripts.
    /// </summary>
    public static LoadAndSaveManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Saves the current state of the game.
    /// </summary>
    public void SaveGame()
    {
        // Code for saving the game state
    }

    /// <summary>
    /// Loads a saved state of the game.
    /// </summary>
    public void LoadGame()
    {
        // Code for loading the game state
    }
}
