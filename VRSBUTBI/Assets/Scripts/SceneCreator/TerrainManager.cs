using UnityEngine;
using UnityEditor;
using System.IO;
using System;

/// <summary>
/// Spawns the terrain on simulation start
/// </summary>
[Serializable]
public class TerrainManager : MonoBehaviour
{
    public static TerrainManager Instance;

    void Start()
    {   
        // A terrain object with 200 width, 1 height, 200 length.
        GameObject terrain = createTerrain(500, 1, 500);
    }

    /// <summary>
    /// Returns a new terrain object given the the width, height, length.
    /// Note: Since there are no textures in the project yet, this method
    /// doesn't add any textures to the terrain.
    /// </summary>
    public GameObject createTerrain(float width, float height, float length)
    {
        // Create a new terrain data.
        TerrainData terrainData = new TerrainData();  
        // Set terrain width, height, length.
        terrainData.size = new Vector3(width, height, length); 

        // Load the concrete texture
        Texture2D texture = Resources.Load<Texture2D>("Concrete");

        // Set the splat prototypes to use the concrete texture
        SplatPrototype[] splats = new SplatPrototype[1];
        splats[0] = new SplatPrototype();
        splats[0].texture = texture;
        splats[0].tileSize = new Vector2(50, 50);
        splats[0].tileOffset = Vector2.zero;
        terrainData.splatPrototypes = splats;

        // Create a new material with the concrete texture
        Material terrainMaterial = new Material(Shader.Find("Nature/Terrain/Diffuse"));
        terrainMaterial.mainTexture = texture;

        // Create a new terrain object
        Terrain terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
        terrain.materialTemplate = terrainMaterial;

        // Return the terrain object
        return terrain.gameObject;  
    }

}
