using UnityEngine;
using System.IO;

/// <summary>
/// This class is responsible for parsing a file loaded by the SimFileHandler
/// </summary>
public class FileParser : MonoBehaviour
{
    private void Start()
    {
        // Subscribe to the FileLoaded event of the SimFileHandler
        SimFileHandler.FileLoaded += ParseFile;
    }

    /// <summary>
    /// This method is called when the SimFileHandler's FileLoaded event is raised. It reads the contents of the file and logs it to the console.
    /// </summary>
    /// <param name="filePath">The path of the file that was loaded by the SimFileHandler</param>
    private void ParseFile(string filePath)
    {
        // Read the contents of the file
        string fileText = File.ReadAllText(filePath);

        // Log the contents of the file to the console
        Debug.Log("Contents of the file: " + fileText);
    }
}
