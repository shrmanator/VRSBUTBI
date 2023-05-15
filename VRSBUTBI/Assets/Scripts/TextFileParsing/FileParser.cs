using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;


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

// Used by ObjectManager
    public delegate void CreateCommandReceivedEventHandler(object[] newObject);
    public static event CreateCommandReceivedEventHandler CreateCommandReceived;

//    public delegate void setobjCommandReceivedEventHandler(object[] data);
//    public static event setobjCommandReceivedEventHandler setobjCommandReceived;

//    public delegate void moveCommandReceivedEventHandler(string objectName3, string pathName, float duration1, float startPosition);
//    public static event moveCommandReceivedEventHandler moveCommandReceived;

//    public delegate void DestroyCommandReceivedEventHandler(string objectName);
//    public static event DestroyCommandReceivedEventHandler DestroyCommandReceived;

//    public delegate void DYNCommandReceivedEventHandler(object[] objectName);
//    public static event DYNCommandReceivedEventHandler DYNjCommandReceived;

    List<object[]> createCommands;
    List<object[]> moveCommands;
    List<object[]> setobjCommands;
    List<object[]> DynCommands;
    List<object[]> pathCommands;
    List<object[]> commands;
    
    bool isCreatingObject;

    // whitespace delimeters to use for parsing
    // prevents error if there's a blank line at the end of the file
    char[] whitespace = {' ', '\n', '\t', '\r', };


    private void Start()
    {
        // Subscribe to the TextFileLoaded event of the SimFileHandler
        SimFileHandler.TextFileLoaded += ParseFile;
        // Subscribe to the ObjectCreated event of the ObjectCreator
        //ObjectManager.ObjectCreated += OnObjectCreated;

    }

    private void ParseFile(string filePath)
    {
        // Read the contents of the file
        string fileText = File.ReadAllText(filePath);

        // Split the contents of the file into individual lines
        string[] lines = fileText.Split('\n');

        // Create a list to hold the commands
        createCommands = new List<object[]>();
        moveCommands = new List<object[]>();
        setobjCommands = new List<object[]>();
        DynCommands = new List<object[]>();
        pathCommands = new List<object[]>();
        commands = new List<object[]>();

        isCreatingObject = false;

        StartCoroutine(ParseFileCoroutine(lines));
    }

    private IEnumerator ParseFileCoroutine(string[] lines)
    {
        // Parse each line into a command and add it to the list
        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line)) // Skip empty lines
            {
                // Split the line into its components
                string[] parts = line.Split(whitespace);
                string cmd = parts[0];

                // Parse the components and add them to the list of commands
                switch (cmd)
                {
                    case "CREATE":
                    //handles object creation during file parsing to prevent issues during playback
                        //Check for valid input (OBJ Type, OBJ name, x, y, z)
                        string objectName1 = parts[1];
                        string masterName = parts[2];
                        float x = float.Parse(parts[3]);
                        float y = float.Parse(parts[4]);
                        float z = float.Parse(parts[5]);
                        commands.Add(new object[] {cmd, objectName1, masterName, x, y, z });
                        object[] newObject = new object[] { objectName1, masterName, x, y, z };
                        createCommands.Add(newObject);
                        //isCreatingObject = true;
                        //CreateCommandReceived?.Invoke(newObject);
                        break;
                    case "SETOBJCELL":
                        //Check for valid input (Core, width lenght, value, unit)
                        string objectName2 = parts[1];
                        string cellName = parts[2];
                        //string formula = parts[3];
                        float x1 = float.Parse(parts[3]);
                        float y1 = float.Parse(parts[4]);
                        float z1 = float.Parse(parts[5]);
                        commands.Add(new object[] {cmd, objectName2, cellName, x1, y1, z1 });
                        setobjCommands.Add(new object[] {objectName2, cellName, x1, y1, z1 });
                        break;
                    case "MOVE":
                        string objectName3 = parts[1];
                       // string pathName1 = parts[2];
                        float duration1 = float.Parse(parts[2]);
//                        float startPosition = parts.Length > 2 ? float.Parse(parts[3].Substring(12)) : 0;
                        commands.Add(new object[] {cmd, objectName3, duration1});
                        moveCommands.Add(new object[] { objectName3, duration1});
                        break;
                    case "PATH":
                        string objectName4 = parts[1];
                        string pathName2 = parts[2];
                        commands.Add(new object[] { cmd, objectName4, pathName2 });
                        pathCommands.Add(new object[] { cmd, objectName4, pathName2 });
                        break;
                    case "DESTROY":
                        string objToDestory = parts[1];
                        //DestroyCommandReceived?.Invoke(objToDestory);
                        commands.Add(new object[] {cmd, objToDestory });
                        break;
                    case "DYNUPDATECELL":
                        string objToUpdate = parts[1];
                        string cellToUpdate = parts[2];
                        float duration2 = float.Parse(parts[3]);
                        float x2 = float.Parse(parts[4].ToString());
                        float y2 = float.Parse(parts[5].ToString());
                        float z2 = float.Parse(parts[6].ToString());
                        commands.Add(new object[] {cmd, objToUpdate, cellToUpdate, duration2, x2, y2, z2 });
                        DynCommands.Add(new object[] { objToUpdate, cellToUpdate, duration2, x2, y2, z2 });
                        break;
                    case "TIME":
                        commands.Add(new object[] {cmd, parts[1]});
                        break;
                    default:
                        Debug.LogWarning("Unrecognized command: " + parts[0]);
                        break;
                }
            }
            // pauses the loop while isCreatingObject is true
            yield return new WaitWhile(() => isCreatingObject);
        }
        ScenePlayer.Player.SetScene(commands, createCommands);

        /*foreach (object[] command in commands)
        {
            foreach (object item in command)
                print(item);
        }*/
        // Raise the CommandReceived event and pass the list of commands
        //CommandReceived?.Invoke(commands);
    }

    /// <summary>
    /// Callback for ObjectCreated event
    /// </summary>
    /*private void OnObjectCreated(){
        isCreatingObject = false;
    }*/

}
