using System.Collections;
using System.Collections.Generic;
using Dummiesman;
using System.IO;
using UnityEngine;

public class ImportObjDemo : MonoBehaviour
{
    string objPath = string.Empty;
    string error = string.Empty;

    //creates debugger/placeholder GUI
    void OnGUI() {
        objPath = GUI.TextField(new Rect(0, 0, 256, 32), objPath);

        GUI.Label(new Rect(0, 0, 256, 32), "Obj Path:");
        if(GUI.Button(new Rect(256, 32, 64, 32), "Load File"))
        {
            //check that file exists
            if (!File.Exists(objPath))
            {
                error = "File doesn't exist.";
            }else{
                //calls plugin to create object
                GameObject loadedObject = new OBJLoader().Load(objPath);
                error = string.Empty;
            }
        }
        
        //handles error
        if(!string.IsNullOrWhiteSpace(error))
        {
            GUI.color = Color.red;
            GUI.Box(new Rect(0, 64, 256 + 64, 32), error);
            GUI.color = Color.white;
        }
    }

    
}
