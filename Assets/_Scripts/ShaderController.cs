using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [SerializeField] private Material comparisonMaterial;
    [SerializeField] private TerrainModifier terrainModifier;
    [SerializeField] private Texture2D targetHeightMap;
    [SerializeField] private Texture2D targetColorMap;

    private void Start()
    {
        if (comparisonMaterial == null || terrainModifier == null)
        {
            Debug.LogError("Missing references in ShaderController!");
            return;
        }

        // Assign textures from TerrainModifier to the shader
        comparisonMaterial.SetTexture("_Player_HeightTex", terrainModifier.GetHeightTexture());
        comparisonMaterial.SetTexture("_Player_ColorTex", terrainModifier.GetColorTexture());

        // Assign reference textures
        comparisonMaterial.SetTexture("_Target_HeightTex", targetHeightMap);
        comparisonMaterial.SetTexture("_Target_ColorTex", targetColorMap);
    }
}
