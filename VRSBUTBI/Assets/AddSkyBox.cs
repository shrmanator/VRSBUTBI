using UnityEngine;

public class AddSkybox : MonoBehaviour
{
    void Start()
    {
        // Step 1: Create a Material
        Material skyboxMaterial = new Material(Shader.Find("Skybox/6 Sided"));

        Texture sunTexture = (Texture)Resources.Load("Built-In Textures/Sun");

        if (sunTexture != null)
        {
            // Step 2: Set Properties for the skybox material
            // skyboxMaterial.SetTexture("_FrontTex", ...);
            // skyboxMaterial.SetTexture("_BackTex", ...);
            // skyboxMaterial.SetTexture("_LeftTex", ...);
            // skyboxMaterial.SetTexture("_RightTex", ...);
            skyboxMaterial.SetTexture("_UpTex", sunTexture);
            skyboxMaterial.SetTexture("_DownTex", sunTexture);
        }
        else
        {
            Debug.LogError("Could not find texture Built-In Textures/Sun in the Resources folder");
        }

        // skyboxMaterial.SetColor("_Tint", ...);
        // skyboxMaterial.SetFloat("_Exposure", ...);

        // Step 4: Assign the skybox material to RenderSettings
        RenderSettings.skybox = skyboxMaterial;
    }
}
