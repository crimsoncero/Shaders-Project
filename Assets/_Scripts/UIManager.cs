using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button compareShaderButton;
    public Button raiseTerrainButton;
    public Button lowerTerrainButton;
    public Button[] colorButtons;
    
    [Header("UI Texts")]
    public TMP_Text compareShaderText;
    
    [Header("Player Controller Reference")]
    public PlayerController playerController; // Reference to your player controller that handles terrain behavior

    private void Start()
    {
        // Assign button click events
        compareShaderButton.onClick.AddListener(CompareTerrain);
        raiseTerrainButton.onClick.AddListener(() => playerController.OnBrushModeIncrease(default));
        lowerTerrainButton.onClick.AddListener(() => playerController.OnBrushModeDecrease(default));

        if (colorButtons.Length >= 6)
        {
            colorButtons[0].onClick.AddListener(() => playerController.OnBrushModeColor1(default));
            colorButtons[1].onClick.AddListener(() => playerController.OnBrushModeColor2(default));
            colorButtons[2].onClick.AddListener(() => playerController.OnBrushModeColor3(default));
            colorButtons[3].onClick.AddListener(() => playerController.OnBrushModeColor4(default));
        }
    }

    private void CompareTerrain()
    {
     //   float similarity = playerController.GetComponent<TerrainModifier>().CompareTerrain(); // Calls existing compute shader function
       // compareShaderText.text = "Similarity: " + (similarity * 100f).ToString("F2") + "%";
    }
}