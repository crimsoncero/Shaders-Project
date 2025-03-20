using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrushModeEnum
{
    HeightIncrease,
    HeightDecrease,
    Color,
}

public class TerrainModifier : MonoBehaviour
{
    [SerializeField] private Material _material;

    [SerializeField] private int _brushRadius = 10;
    [SerializeField] private float _heightChange = 0.2f;

    private Texture2D _heightTexture;
    private Texture2D _colorTexture;
    
    
    
    [field: SerializeField] public BrushModeEnum BrushMode { get; set; } = BrushModeEnum.HeightIncrease;

    private void Start()
    {
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
        
        if (Physics.Raycast(ray, out hit))
        {
            Vector2 uv = hit.textureCoord;
            
            switch (BrushMode)
            {
                case BrushModeEnum.HeightIncrease:
                    OnHeightChange(uv,true);
                    break;
                case BrushModeEnum.HeightDecrease:
                    OnHeightChange(uv,false);
                    break;
                case BrushModeEnum.Color:
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
        
        float change = Mathf.Lerp(0, 255, _heightChange);
        change = isIncrease ? change : -change;


        for (int i = -_brushRadius / 2; i < _brushRadius / 2; i++)
        {
            for (int j = -_brushRadius / 2; j < _brushRadius / 2; j++)
            {
                var pixel = _heightTexture.GetPixel((int)pixelPos.x + i, (int)pixelPos.y + j);
                pixel.r = Mathf.Clamp(pixel.r + change, 0, 255);
                pixel.g = Mathf.Clamp(pixel.g + change, 0, 255);
                pixel.b = Mathf.Clamp(pixel.b + change, 0, 255);
                _heightTexture.SetPixel((int)pixelPos.x + i, (int)pixelPos.y + j, pixel);
            }
        }
        _heightTexture.Apply();

    }

    private Vector2 UVToPixel(Vector2 uvPos, Texture2D texture)
    {
        Vector2 v = Vector2.zero;
        
        v.x = Mathf.Lerp(0, texture.width, uvPos.x);
        v.y = Mathf.Lerp(0, texture.height, uvPos.y);

        return v;
    }
    
}
