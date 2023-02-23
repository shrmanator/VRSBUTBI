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
            if (!string.IsNullOrEmpty(line)) {

                // Split the line into separate commands
                string[] components = line.Split(' ');

                // Parse the components and add them to the list of commands
                switch (components[0])
                {
                    case "CREATE":
                        commands.Add(new object[] { components[1], components[2], int.Parse(components[3]), int.Parse(components[4]) });
                        break;
                    case "SETOBJCELL":
                        commands.Add(new object[] { components[1], components[2], int.Parse(components[3]), int.Parse(components[4]) });
                        break;
                    default:
                        Debug.LogWarning("Unrecognized command: " + components[0]);
                        break;
                }
            }
        }
        // Raise the CommandReceived event and pass the list of commands
        CommandReceived?.Invoke(commands);
    }
}
