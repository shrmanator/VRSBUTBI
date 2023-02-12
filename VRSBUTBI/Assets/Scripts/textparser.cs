using System.IO;
using UnityEngine;
using FileBrowser;

public class TextParser : MonoBehaviour
{
    // Reference to the ObjectCreator script
    public ObjectCreator objectCreator;

    // Reference to the MoveObject script
    public MoveObject moveObject;

    private void Start()
    {
        // Open a file browser to select a text file
        FileBrowser.OpenFilePanel("Select the file", "", "txt", (path) =>
        {
            // Read all the lines in the text file
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                // Split the line into words
                string[] words = line.Split(' ');
                if (words[0] == "CREATE")
                {
                    // Extract the x and y coordinates
                    float x = float.Parse(words[1]);
                    float y = float.Parse(words[2]);
                    // Call the Create method in the ObjectCreator script
                    objectCreator.Create(x, y);
                }
                else if (words[0] == "MOVE")
                {
                    // Extract the x and y coordinates
                    float x = float.Parse(words[1]);
                    float y = float.Parse(words[2]);
                    // Call the Move method in the MoveObject script
                    moveObject.Move(x, y);
                }
            }
        });
    }
}
