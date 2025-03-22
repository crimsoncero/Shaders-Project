using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrushModeEnum
{
    HeightIncrease,
    HeightDecrease,
    ColorOne,
    ColorTwo,
    ColorThree,
    ColorFour,
    ColorFive,
    ColorSix,
}

public class TerrainModifier : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private LayerMask _terrainLayer;
    [Header("Brush Settings")]
    [SerializeField] private int _brushRadius = 10;
    [SerializeField, Range(0, 100)] private int _heightChange = 5;
    [Header("Colors")] 
    [SerializeField] private Color _colorOne = Color.white;
    [SerializeField] private Color _colorTwo = Color.green;
    [SerializeField] private Color _colorThree = Color.blue;
    [SerializeField] private Color _colorFour = Color.red;
    [SerializeField] private Color _colorFive = Color.yellow;
    [SerializeField] private Color _colorSix = Color.cyan;
    
    
    
    private Texture2D _heightTexture;
    private Texture2D _colorTexture;



    [field: SerializeField] public BrushModeEnum BrushMode { get; set; } = BrushModeEnum.HeightIncrease;

    private void Start()
    {
        _material = new Material(_material);
        _meshRenderer.material = _material;
        
        
        var originalHeight = (Texture2D)_material.GetTexture("_HeightTex");
        var originalColor = (Texture2D)_material.GetTexture("_ColorTex");

        _heightTexture = new Texture2D(originalHeight.width, originalHeight.height, TextureFormat.RGBA32, false);
        _colorTexture = new Texture2D(originalColor.width, originalColor.height, TextureFormat.RGBA32, false);

        _heightTexture.SetPixels32(originalHeight.GetPixels32());
        _colorTexture.SetPixels32(originalColor.GetPixels32());

        _heightTexture.Apply();
        _colorTexture.Apply();

        _material.SetTexture("_HeightTex", _heightTexture);
        _material.SetTexture("_ColorTex", _colorTexture);

    }

    public void OnPaint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _terrainLayer))
        {
            Vector2 uv = hit.textureCoord;

            switch (BrushMode)
            {
                case BrushModeEnum.HeightIncrease:
                    OnHeightChange(uv, true);
                    break;
                case BrushModeEnum.HeightDecrease:
                    OnHeightChange(uv, false);
                    break;
                case BrushModeEnum.ColorOne:
                    OnColorChange(uv, _colorOne);
                    break;
                case BrushModeEnum.ColorTwo:
                    OnColorChange(uv, _colorTwo);
                    break;
                case BrushModeEnum.ColorThree:
                    OnColorChange(uv, _colorThree);
                    break;
                case BrushModeEnum.ColorFour:
                    OnColorChange(uv, _colorFour);
                    break;
                case BrushModeEnum.ColorFive:
                    OnColorChange(uv, _colorFive);
                    break;
                case BrushModeEnum.ColorSix:
                    OnColorChange(uv, _colorSix);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
        else
        {
            return;
        }
    }

    private void OnHeightChange(Vector2 uvPos, bool isIncrease)
    {
        Vector2 pixelPos = UVToPixel(uvPos, _heightTexture);

        float change = Mathf.InverseLerp(0, 100, _heightChange);
        change = isIncrease ? change : -change;
        
       
        for (int i = -_brushRadius / 2; i < _brushRadius / 2; i++)
        {
            // Skip if out of bounds
            if(i < 0 || i > _heightTexture.width) continue;
            
            for (int j = -_brushRadius; j < _brushRadius; j++)
            {
                // Skip if out of bounds
                if(j < 0 || j > _heightTexture.height) continue;
                
                

                
                var pixel = _heightTexture.GetPixel((int)pixelPos.x + i, (int)pixelPos.y + j);
                pixel.r = Mathf.Clamp(pixel.r + change, 0, 1);
                pixel.g = Mathf.Clamp(pixel.g + change, 0, 1);
                pixel.b = Mathf.Clamp(pixel.b + change, 0, 1);
                _heightTexture.SetPixel((int)pixelPos.x + i, (int)pixelPos.y + j, pixel);
            }
        }

        _heightTexture.Apply();

    }

    private void OnColorChange(Vector2 uvPos, Color color)
    {
        Vector2 pixelPos = UVToPixel(uvPos, _colorTexture);
       
        for (int i = -_brushRadius; i < _brushRadius; i++)
        {
            // Skip if out of bounds
            if(i < 0 || i > _colorTexture.width) continue;
            
            for (int j = -_brushRadius; j < _brushRadius; j++)
            {
                // Skip if out of bounds
                if(j < 0 || j > _colorTexture.height) continue;

                

                _colorTexture.SetPixel((int)pixelPos.x + i, (int)pixelPos.y + j, color);
            }
        }

        _colorTexture.Apply();
    }
    
    private Vector2 UVToPixel(Vector2 uvPos, Texture2D texture)
    {
        Vector2 v = Vector2.zero;

        v.x = Mathf.Lerp(0, texture.width, uvPos.x);
        v.y = Mathf.Lerp(0, texture.height, uvPos.y);

        return v;
    }
    
    // Use this methods to connect stuff:
    
    /// <summary>
    /// Reset the height and color texture to a baseline form
    /// </summary>
    public void ResetTextures()
    {
        var originalHeight = (Texture2D)_material.GetTexture("_HeightTex");
        var originalColor = (Texture2D)_material.GetTexture("_ColorTex");
        _heightTexture = new Texture2D(originalHeight.width, originalHeight.height, TextureFormat.RGBA32, false);
        _colorTexture = new Texture2D(originalColor.width, originalColor.height, TextureFormat.RGBA32, false);
        Color32 pixel = new Color32(127, 127, 127, 255);

        for (int i = 0; i < _heightTexture.width; i++)
        {
            for (int j = 0; j < _heightTexture.height; j++)
            {
                _heightTexture.SetPixel(i, j, pixel);
            }
        }
        
        for (int i = 0; i < _colorTexture.width; i++)
        {
            for (int j = 0; j < _colorTexture.height; j++)
            {
                _colorTexture.SetPixel(i, j, pixel);
            }
        }

        _heightTexture.Apply();
        _colorTexture.Apply();

        _material.SetTexture("_HeightTex", _heightTexture);
        _material.SetTexture("_ColorTex", _colorTexture);
    }

    // Getter for the height texture currently used
    public Texture2D GetHeightTexture()
    {
        return _heightTexture;
    }
    
    // Getter for the color texture currently used
    public Texture2D GetColorTexture()
    {
        return _colorTexture;
    }

    // Get the color matching the number
    public Color GetColor(int colorNumber)
    {
        if (colorNumber < 1 || colorNumber > 6)
        {
            Debug.LogError("Out of bounds color number");
            throw new ArgumentOutOfRangeException("colorNumber");
        }

        switch (colorNumber)
        {
            case 1:
                return _colorOne;
            case 2:
                return _colorTwo;
            case 3:
                return _colorThree;
            case 4:
                return _colorFour;
            case 5:
                return _colorFive;
            case 6:
                return _colorSix;
        }

        return _colorOne;
    }

    // Use this to change the size of the brush (in pixels
    public void ChangeBrushSize(int brushSize)
    {
        _brushRadius = brushSize;
    }

    // Use this to change how much the height increases/decreases, goes from 0 - 100 
    public void ChangeHeightChange(int heightChange)
    {
        _heightChange = Mathf.Clamp(heightChange, 0, 100);
    }
    
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "ResetTexture"))
        {
            ResetTextures();
        }
    }
}
