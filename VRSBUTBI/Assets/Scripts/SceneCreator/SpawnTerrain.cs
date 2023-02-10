using UnityEngine;
using UnityEditor;
using System.IO;
 
/// <summary>
/// Spawns the terrain on simulation start
/// </summary>
public class SpawnTerrain : MonoBehaviour
{
    void Start()
    {   
        // A terrain object with 200 width, 1 height, 200 length.
        GameObject terrain = createTerrain(200, 1, 200);
    }

    /// <summary>
    /// Returns a new terrain object given the the width, height, length. 
    /// </summary>
    public GameObject createTerrain(float width, float height, float length)
    {
        // Create a new terrain data.
        TerrainData terrainData = new TerrainData();  
        // Set terrain width, height, length.
        terrainData.size = new Vector3(width, height, length); 
        // Return a new terrain object with the terrain data.
        return Terrain.CreateTerrainGameObject(terrainData);    
    }
}
