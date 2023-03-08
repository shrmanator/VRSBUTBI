using UnityEngine;
using System.IO;
using System.Collections.Generic;


/// <summary>
/// This class is responsible for parsing an input txt file and organizing that text into a list of commands.
/// 
/// It subscribes to the FileLoaded event of the SimFileHandler (see SimFileHandler.cs) and raises the
/// CommandReceivedEventHandler once a new txt file is loaded.
/// </summary>
public class FileParser : MonoBehaviour
{
    /// <summary>
    /// Delegate for handling the CommandReceived event, which is raised when a list of commands is received.
    /// </summary>
    /// <param name="commands">The list of commands received.</param>
    public delegate void CommandReceivedEventHandler(List<object[]> commands);

    /// <summary>
    /// Event that is raised when a list of commands is received.
    /// </summary>
    public static event CommandReceivedEventHandler CommandReceived;

    private void Start()
    {
        // Subscribe to the FileLoaded event of the SimFileHandler
        SimFileHandler.FileLoaded += ParseFile;
    }

    private void ParseFile(string filePath)
    {
        // Read the contents of the file
        string fileText = File.ReadAllText(filePath);

        // Split the contents of the file into individual lines
        string[] lines = fileText.Split('\n');

        // Create a list to hold the commands
        List<object[]> commands = new List<object[]>();

        // Parse each line into a command and add it to the list
        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line)) // Skip empty lines
            {
                // Split the line into its components
                string[] parts = line.Split(' ');
                string commands = components[0];

                // Parse the components and add them to the list of commands
                switch (commands)
                {
                    case "CREATE":
                        //Check for valid input (OBJ Type, OBJ name, x, y, z)
                        string objectName = parts[1];
                        string masterName = parts[2];
                        float x = float.Parse(parts[3]);
                        float y = float.Parse(parts[4]);
                        float z = float.Parse(parts[5]);
                        break;
                    case "SETOBJCELL":
                        //Check for valid input (Core, width lenght, value, unit)
                        string objName = parts[1];
                        string cellName = parts[2];
                        string formula = parts[3];
                        break;
                    case "MOVE":
                        string objectName = tokens[1];
                        string pathName = tokens[2];
                        float duration = float.Parse(tokens[3]);
                        float startPosition = tokens.Length > 4 ? float.Parse(tokens[4].Substring(12)) : 0;
                        break;
                    case "DESTROY":
                        string objToDestory = parts[1];
                        break;
                    case "DYNUPDATECELL"
                        string objToUpdate = parts[1];
                        string cellToUpdate = parts[2];
                        float duration = float.Parse(parts[3]);
                        float startVal = float.Parse(parts[4]);
                        float endVal = float.Parse(parts[5]);
                        string units = parts.Length > 6 ? parts[6] : null;
                        break;
                    default:
                        Debug.LogWarning("Unrecognized command: " + parts[0]);
                        break;
                }
            }
        }
        foreach (object[] command in commands)
        {
            foreach (object item in command)
                print(item);
        }
        // Raise the CommandReceived event and pass the list of commands
        CommandReceived?.Invoke(commands);
    }
}
