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
        FileBrowser.SetFilters( false, new FileBrowser.Filter( ".obj", ".obj") );
        FileBrowser.ShowSaveDialog(OnSaveSuccess, OnSaveCancel, FileBrowser.PickMode.Files, false, null, "new_file", "Save File", "Save");
    }

    /// <summary>
    /// Opens the load file dialog (only accepting .txt files).
    /// </summary>
    public void OpenLoadDialog()
    {
        FileBrowser.SetFilters( false, new FileBrowser.Filter( ".txt", ".txt") );
        FileBrowser.ShowLoadDialog(OnLoadSuccess, OnLoadCancel, FileBrowser.PickMode.Files, false, null, "", "Load File", "Load");
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

    /// <summary>
    /// Handles a successful load.
    /// </summary>
    /// <param name="filePaths">The paths of the saved files.</param>
    private void OnLoadSuccess(string[] filePaths)
    {
        Debug.Log("Selected file: " + filePaths[0] + " loaded!");
    }

    /// <summary>
    /// Handles a canceled load.
    /// </summary>
    private void OnLoadCancel()
    {
        Debug.Log("Load canceled.");
    }
}
