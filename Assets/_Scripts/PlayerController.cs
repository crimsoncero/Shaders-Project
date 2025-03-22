using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public enum VerticalEnum
{
    None,
    Up,
    Down,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private TerrainModifier _terrainModifier;
    [FormerlySerializedAs("moveSpeed")] [SerializeField] private float _moveSpeed = 7.5f;
    [FormerlySerializedAs("lookSpeed")] [SerializeField] private float _lookSpeed = 2.5f;
    
    
    [FormerlySerializedAs("isControllingCamera")] [SerializeField] private bool _isControllingCamera = false;
    private Vector3 _moveDirection = Vector3.zero;
    private VerticalEnum _verticalMovement = VerticalEnum.None; 

    private void Update()
    {
        Vector3 v = transform.rotation * _moveDirection;
        transform.position += v * _moveSpeed * Time.deltaTime;

        switch (_verticalMovement)
        {
            case VerticalEnum.Up:
                transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
                break;
            case VerticalEnum.Down:
                transform.position += Vector3.down * _moveSpeed * Time.deltaTime;
                break;
            case VerticalEnum.None:
            default:
                break;
        }
        
    }

    public void OnPaint(InputAction.CallbackContext context)
    {
        _terrainModifier.OnPaint();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveVec = context.ReadValue<Vector2>();
        _moveDirection.x = moveVec.x;
        _moveDirection.z = moveVec.y;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (_isControllingCamera)
        {
            Vector2 lookVec = context.ReadValue<Vector2>();
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y += lookVec.x * _lookSpeed;
            eulerAngles.x -= lookVec.y * _lookSpeed;
            
            transform.eulerAngles = eulerAngles;
        }
    }

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _verticalMovement = VerticalEnum.Up;
        }
        else
        {
            _verticalMovement = VerticalEnum.None;
        }
    }

    public void OnMoveDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _verticalMovement = VerticalEnum.Down;
        }
        else
        {
            _verticalMovement = VerticalEnum.None;
        }
    }
    
    public void OnControlCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isControllingCamera = true;
        }
        else
        {
            _isControllingCamera = false;
        }
    }

    public void OnBrushModeIncrease(InputAction.CallbackContext context)
    {
        _terrainModifier.BrushMode = BrushModeEnum.HeightIncrease;
    }
    
    public void OnBrushModeDecrease(InputAction.CallbackContext context)
    {
        _terrainModifier.BrushMode = BrushModeEnum.HeightDecrease;
    }
    
    public void OnBrushModeColor1(InputAction.CallbackContext context)
    {
        _terrainModifier.BrushMode = BrushModeEnum.ColorOne;
    }
    
    public void OnBrushModeColor2(InputAction.CallbackContext context)
    {
        _terrainModifier.BrushMode = BrushModeEnum.ColorTwo;
    }
    public void OnBrushModeColor3(InputAction.CallbackContext context)
    {
        _terrainModifier.BrushMode = BrushModeEnum.ColorThree;
    }
    public void OnBrushModeColor4(InputAction.CallbackContext context)
    {
        _terrainModifier.BrushMode = BrushModeEnum.ColorFour;
    }
    public void OnBrushModeColor5(InputAction.CallbackContext context)
    {
        _terrainModifier.BrushMode = BrushModeEnum.ColorFive;
    }
    public void OnBrushModeColor6(InputAction.CallbackContext context)
    {
        _terrainModifier.BrushMode = BrushModeEnum.ColorSix;
    }
}
