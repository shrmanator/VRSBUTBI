using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class textparser : MonoBehaviour
{
    // Start is called before the first frame update
    //"read in file and parse user's commands"
    void Start()
    {
        string fileName = "";
        using (StreamReader sr = File.OpenText(fileName))

        {

            string s = sr.ReadToEnd();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
