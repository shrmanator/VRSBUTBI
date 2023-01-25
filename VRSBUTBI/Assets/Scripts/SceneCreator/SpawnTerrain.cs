using UnityEngine;
using UnityEditor;
using System.IO;
 

public class SpawnTerrain : MonoBehaviour
{
    void Start()
    {
         // create a new terrain data
        TerrainData _terrainData = new TerrainData();  
        // set terrain width, height, length
        _terrainData.size = new Vector3(20, 1, 20); 
        // create a terrain with the terrain data
        GameObject _terrain = Terrain.CreateTerrainGameObject(_terrainData);    
    }

    void Update()
    {
    
    }
}
