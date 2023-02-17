using UnityEngine;
using System.IO;


/// <summary>
/// This class is responsible for parsing an input file, organizing the data into a list of objects,
/// and saving the data to a JSON file. It subscribes to the FileLoaded event of the SimFileHandler
/// and raises the JsonFileCreated event when the JSON file is created.
/// </summary>
public class FileParser : MonoBehaviour
{
    // Define a class or struct to hold the data from the input file
    [System.Serializable]
    private class Data
    {
        public string name;
        public int score;
    }

    // Define a delegate and event for the JSON file created event
    public delegate void JsonFileCreatedHandler(string filePath);
    public static event JsonFileCreatedHandler JsonFileCreated;

    private void Start()
    {
        // Subscribe to the FileLoaded event of the SimFileHandler
        SimFileHandler.FileLoaded += ParseFile;
    }

    /// <summary>
    /// This method is called when the SimFileHandler's FileLoaded event is raised. It parses the
    /// contents of the file, saves the data to a JSON file, and raises the JsonFileCreated event.
    /// </summary>
    /// <param name="filePath">The path of the file that was loaded by the SimFileHandler</param>
    private void ParseFile(string filePath)
    {
        // Read the contents of the file
        string[] lines = File.ReadAllLines(filePath);

        // Parse the input file and organize the information into a list of Data objects
        List<Data> dataList = new List<Data>();
        foreach (string line in lines)
        {
            string[] fields = line.Split(',');
            Data data = new Data { name = fields[0], score = int.Parse(fields[1]) };
            dataList.Add(data);
        }

        // Convert the list of Data objects to a JSON string
        string json = JsonUtility.ToJson(dataList, prettyPrint: true);

        // Write the JSON string to a file
        string jsonFilePath = "data.json";
        File.WriteAllText(jsonFilePath, json);

        // Raise the JsonFileCreated event with the path of the created JSON file
        if (JsonFileCreated != null)
        {
            JsonFileCreated(jsonFilePath);
        }
    }
}
