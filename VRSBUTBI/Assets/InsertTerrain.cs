using UnityEngine;
using UnityEditor;
using System.IO;
 

public class InsertTerrain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //create a new terrain data
        TerrainData _terrainData = new TerrainData();
 
        //set terrain width, height, length
        _terrainData.size = new Vector3(20, 1, 20);

        //Create a terrain with the set terrain data
        GameObject _terrain = Terrain.CreateTerrainGameObject(_terrainData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
