using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [SerializeField] private Material _comparisonMaterial;
    [SerializeField] private TerrainModifier _terrainModifier;
    [SerializeField] private Texture2D _targetHeightMap;
    [SerializeField] private Texture2D _targetColorMap;

    private void Start()
    {
        if (_comparisonMaterial == null || _terrainModifier == null)
        {
            Debug.LogError("Missing references in ShaderController!");
            return;
        }

        // Assign textures from TerrainModifier to the shader
        _comparisonMaterial.SetTexture("_Player_HeightTex", _terrainModifier.GetHeightTexture());
        _comparisonMaterial.SetTexture("_Player_ColorTex", _terrainModifier.GetColorTexture());

        // Assign reference textures
        _comparisonMaterial.SetTexture("_Target_HeightTex", _targetHeightMap);
        _comparisonMaterial.SetTexture("_Target_ColorTex", _targetColorMap);
    }

    
    public float CompareTerrains()
    {
        Texture2D playerHeightMap = (Texture2D)_comparisonMaterial.GetTexture("_Player_HeightTex");
        Texture2D playerColorMap = (Texture2D)_comparisonMaterial.GetTexture("_Player_ColorTex");

        if (playerHeightMap == null || playerColorMap == null) return 0f;

        int width = playerHeightMap.width;
        int height = playerHeightMap.height;
        float totalScore = 0f;
        int totalPixels = width * height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float playerHeight = playerHeightMap.GetPixel(x, y).r;
                float targetHeight = _targetHeightMap.GetPixel(x, y).r;
                float heightDiff = Mathf.Abs(playerHeight - targetHeight);

                Color playerColor = playerColorMap.GetPixel(x, y);
                Color targetColor = _targetColorMap.GetPixel(x, y);
                float colorDiff = Mathf.Abs(playerColor.r - targetColor.r) +
                                  Mathf.Abs(playerColor.g - targetColor.g) +
                                  Mathf.Abs(playerColor.b - targetColor.b);

                float heightScore = Mathf.Clamp01(1 - (heightDiff / _comparisonMaterial.GetFloat("_Comparison_Threshold")));
                float colorScore = Mathf.Clamp01(1 - (colorDiff / _comparisonMaterial.GetFloat("_Comparison_Threshold")));

                float pixelScore = ((heightScore + colorScore) / 2) * _comparisonMaterial.GetFloat("_Resolution_Factor");

                totalScore += pixelScore;
            }
        }

        return totalScore / totalPixels;
    }

    public void SetTexture()
    {
        _comparisonMaterial.SetTexture("_Player_HeightTex", _terrainModifier.GetHeightTexture());
        _comparisonMaterial.SetTexture("_Player_ColorTex", _terrainModifier.GetColorTexture());
    }

    public void SetResolutionFactor(float factor)
    {
        _comparisonMaterial.SetFloat("_Resolution_Factor", factor);
    }
}
