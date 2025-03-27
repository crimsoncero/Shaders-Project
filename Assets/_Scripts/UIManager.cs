using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button raiseTerrainButton;
    public Button lowerTerrainButton;
    public Button[] colorButtons;
    
    [Header("UI Texts")]
    [SerializeField] private TMP_Text _scoreText;

    [Header("Player Controller Reference")]
    public PlayerController playerController; // Reference to your player controller that handles terrain behavior

    [SerializeField] private ShaderController _shaderController;
    private void Start()
    {
        // Assign button click events
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

    public void CompareAndDisplayScore()
    {
        float score = _shaderController.CompareTerrains();
        if (score > 1) score = 1;
        else if (score < 0) score = 0;
        _scoreText.text = "Score: " + (score * 100).ToString("F2") + "%";
    }
}