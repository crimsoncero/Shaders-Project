using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerrainComparison : MonoBehaviour
{
    [SerializeField] private TerrainModifier _terrainModifier;
    [SerializeField] private Texture2D _targetHeightMap;
    [SerializeField] private Texture2D _targetColorMap;
    [SerializeField] private float comparisonThreshold = 0.1f;
    [SerializeField] private float resolutionFactor = 1.0f;
    [SerializeField] private TMP_Text scoreText;

    public void CompareAndDisplayScore()
    {
        float score = CompareTerrains();
        if (score > 1) score = 1;
        else if (score < 0) score = 0;
        scoreText.text = "Score: " + (score * 100).ToString("F2") + "%";
    }

    private float CompareTerrains()
    {
        Texture2D playerHeightMap = _terrainModifier.GetHeightTexture();
        Texture2D playerColorMap = _terrainModifier.GetColorTexture();

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

                float heightScore = Mathf.Clamp01(1 - (heightDiff / comparisonThreshold));
                float colorScore = Mathf.Clamp01(1 - (colorDiff / comparisonThreshold));

                float pixelScore = ((heightScore + colorScore) / 2) * resolutionFactor;

                totalScore += pixelScore;
            }
        }

        return totalScore / totalPixels;
    }

    public void SetResolutionFactor(float factor)
    {
        resolutionFactor = factor;
    }
}
