using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;


public class FileBrowserHelper : MonoBehaviour{

    /// <summary>
    /// Starts the coroutine for the FileBrowser for a single file
    /// <param name="onSuccess">The callback function for a successful load</param>
    /// <param name="onCancel">The callback function for a successful load</param>
    /// <param name="title">The text displayed at the top of the window</param>
    /// <param name="loadButton">The text displayed on the load button</param>
    /// <param name="filter">The list of file types to display</param>
    /// </summary>
    public void LoadSingleFile(FileBrowser.OnSuccess onSuccess, FileBrowser.OnCancel onCancel,
        string title, string loadButton, string[] filter ){
        StartCoroutine(LoadSingleFileCoroutine(onSuccess, onCancel, title, loadButton, filter));
    }

    /// <summary>
    /// Shows the file browser and waits for the user to select a file or cancel
    /// <param name="onSuccess">The callback function for a successful load</param>
    /// <param name="onCancel">The callback function for a successful load</param>
    /// <param name="title">The text displayed at the top of the window</param>
    /// <param name="loadButton">The text displayed on the load button</param>
    /// <param name="filter">The list of file types to display</param>
    /// </summary>
    IEnumerator LoadSingleFileCoroutine(FileBrowser.OnSuccess onSuccess, FileBrowser.OnCancel onCancel,
        string title, string loadButton, string[] filter)
    {
        FileBrowser.SetFilters(false, filter);
        FileBrowser.ShowLoadDialog(onSuccess, onCancel, FileBrowser.PickMode.Files, false, null, null, title, loadButton);
        yield return new WaitWhile(() => FileBrowser.IsOpen);
    }
}
