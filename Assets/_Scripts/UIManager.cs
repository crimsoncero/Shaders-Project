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

    private Button _activeButton;
    private Color defaultColor = Color.white;
    private Color activeColor = Color.green;

    private void Start()
    {
        // Assign button click events with name changes and highlighting
        raiseTerrainButton.onClick.AddListener(() => SetActiveButton(raiseTerrainButton, "Raise Terrain", () => playerController.OnBrushModeIncrease(default)));
        lowerTerrainButton.onClick.AddListener(() => SetActiveButton(lowerTerrainButton, "Lower Terrain", () => playerController.OnBrushModeDecrease(default)));

        if (colorButtons.Length >= 6)
        {
            colorButtons[0].onClick.AddListener(() => SetActiveButton(colorButtons[0], "Color 1", () => playerController.OnBrushModeColor1(default)));
            colorButtons[1].onClick.AddListener(() => SetActiveButton(colorButtons[1], "Color 2", () => playerController.OnBrushModeColor2(default)));
            colorButtons[2].onClick.AddListener(() => SetActiveButton(colorButtons[2], "Color 3", () => playerController.OnBrushModeColor3(default)));
            colorButtons[3].onClick.AddListener(() => SetActiveButton(colorButtons[3], "Color 4", () => playerController.OnBrushModeColor4(default)));
            colorButtons[4].onClick.AddListener(() => SetActiveButton(colorButtons[4], "Color 5", () => playerController.OnBrushModeColor5(default)));
            colorButtons[5].onClick.AddListener(() => SetActiveButton(colorButtons[5], "Color 6", () => playerController.OnBrushModeColor6(default)));
        }
    }

    private void SetActiveButton(Button button, string buttonText, System.Action action)
    {
        if (_activeButton != null)
        {
            //_activeButton.GetComponentInChildren<TMP_Text>().text = _activeButton.name;
            _activeButton.image.color = defaultColor;
        }

        _activeButton = button;
      //  _activeButton.GetComponentInChildren<TMP_Text>().text = buttonText;
        _activeButton.image.color = activeColor;

        action.Invoke();
    }

    public void CompareAndDisplayScore()
    {
        float score = _shaderController.CompareTerrains();
        score = Mathf.Clamp01(score); // Ensures the score is between 0 and 1
        _scoreText.text = "Score: " + (score * 100).ToString("F2") + "%";
    }
}