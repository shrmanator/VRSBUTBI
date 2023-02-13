using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

/// <summary>
/// This class reads and parses the commands in a text file,
/// and stores the data in a JSON format to be used by other scripts.
/// </summary>

public class TextParser : MonoBehaviour{

/// <summary>    
/// A class to store the data for each command.    
/// </summary>    

    private class CommandData    
    {
        public string Type;
        public string ObjectName;
        public string MasterName;
        public float X;
        public float Y;
    }

    // A list to store all the CommandData objects
    private List<CommandData> commandDataList = new List<CommandData>();

    // A public property to access the list of CommandData objects as a JSON string
    public string CommandDataJson { get; private set; }

    private void Start()
    {
        // Read the text file
        string[] lines = File.ReadAllLines("file.txt");
        // Parse the lines into CommandData objects
        foreach (string line in lines)
        {
            string[] words = line.Split(' ');
            if (words[0] == "CREATE")
            {
                CommandData cmd = new CommandData();
                cmd.Type = words[0];
                cmd.ObjectName = words[1];
                cmd.MasterName = words[2];
                cmd.X = float.Parse(words[3]);
                cmd.Y = float.Parse(words[4]);
                commandDataList.Add(cmd);
            }
        }
        // Serialize the list of CommandData objects to a JSON string
        CommandDataJson = JsonConvert.SerializeObject(commandDataList);
    }
}