using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private TerrainComparison terrainComparison;
    [SerializeField] private TMP_Dropdown difficultyDropdown;

    private void Start()
    {
        difficultyDropdown.onValueChanged.AddListener(SetDifficulty);
    }

    private void SetDifficulty(int index)
    {
        switch (index)
        {
            case 0: // Easy
                terrainComparison.SetResolutionFactor(0.5f);
                break;
            case 1: // Medium
                terrainComparison.SetResolutionFactor(1.0f);
                break;
            case 2: // Hard
                terrainComparison.SetResolutionFactor(1.5f);
                break;
        }
    }
}
