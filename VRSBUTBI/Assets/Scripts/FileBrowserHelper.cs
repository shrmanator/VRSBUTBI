using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;


public class FileBrowserHelper : MonoBehaviour{
    public void LoadSingleFile(FileBrowser.OnSuccess onSuccess, FileBrowser.OnCancel onCancel,
        string title, string loadButton, string[] filter ){
        StartCoroutine(LoadSingleFileCoroutine(onSuccess, onCancel, title, loadButton, filter));
    }

    IEnumerator LoadSingleFileCoroutine(FileBrowser.OnSuccess onSuccess, FileBrowser.OnCancel onCancel,
        string title, string loadButton, string[] filter)
    {
        UnityEngine.Debug.Log("Running coroutine");
        FileBrowser.SetFilters(false, filter);
        FileBrowser.ShowLoadDialog(onSuccess, onCancel, FileBrowser.PickMode.Files, false, null, null, title, loadButton);
        yield return new WaitWhile(() => FileBrowser.IsOpen);
    }
}
