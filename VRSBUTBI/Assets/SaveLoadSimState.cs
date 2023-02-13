using UnityEngine;
using SimpleFileBrowser;

public class SaveLoadSimState : MonoBehaviour
{
    public delegate void OnSuccess(string[] filePaths);
    public delegate void OnCancel();

    public void OpenSaveDialog()
    {
        FileBrowser.ShowSaveDialog(OnSaveSuccess, OnSaveCancel,  FileBrowser.PickMode.Files, false, null, "new_file.txt", "Save File", "Save");
    }

    private void OnSaveSuccess(string[] filePaths)
    {
        Debug.Log("Selected file: " + filePaths[0] " saved!");
    }

    private void OnSaveCancel()
    {
        Debug.Log("Save cancelled.");
    }
}
