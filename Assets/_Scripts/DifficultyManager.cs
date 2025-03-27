using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private ShaderController _shaderController;
    [SerializeField] private TMP_Dropdown _difficultyDropdown;

    private void Start()
    {
        _difficultyDropdown.onValueChanged.AddListener(SetDifficulty);
    }

    private void SetDifficulty(int index)
    {
        switch (index)
        {
            case 0: // Easy
                _shaderController.SetResolutionFactor(1.5f);
                break;
            case 1: // Medium
                _shaderController.SetResolutionFactor(1.0f);
                break;
            case 2: // Hard
                _shaderController.SetResolutionFactor(0.5f);
                break;
        }
    }
}
