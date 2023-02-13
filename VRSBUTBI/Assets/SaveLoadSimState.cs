using UnityEngine;
using SimpleFileBrowser;

/// <summary>
/// Handles the saving and loading of simulation state.
/// </summary>
public class SaveLoadSimState : MonoBehaviour
{
    /// <summary>
    /// Delegate for handling a successful save.
    /// </summary>
    /// <param name="filePaths">The paths of the saved files.</param>
    public delegate void OnSuccess(string[] filePaths);

    /// <summary>
    /// Delegate for handling a canceled save.
    /// </summary>
    public delegate void OnCancel();

    /// <summary>
    /// Opens the save file dialog.
    /// </summary>
    public void OpenSaveDialog()
    {
        FileBrowser.ShowSaveDialog(OnSaveSuccess, OnSaveCancel, FileBrowser.PickMode.Files, false, null, "new_file.txt", "Save File", "Save");
    }

    public void OpenLoadDialog()
    {
        FileBrowser.ShowLoadDialog(OnSaveSuccess, OnSaveCancel, FileBrowser.PickMode.Files, false, null, "new_file.txt", "Save File", "Save");
    }

    /// <summary>
    /// Handles a successful save.
    /// </summary>
    /// <param name="filePaths">The paths of the saved files.</param>
    private void OnSaveSuccess(string[] filePaths)
    {
        Debug.Log("Selected file: " + filePaths[0] + " saved!");
    }

    /// <summary>
    /// Handles a canceled save.
    /// </summary>
    private void OnSaveCancel()
    {
        Debug.Log("Save canceled.");
    }
}