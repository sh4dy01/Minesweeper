using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Managers;

public class DragProps : MonoBehaviour
{
    private Vector2 _mousePosition;
    private float _deltaX, _deltaY;
    private bool _isCarrying = false;
    
    [SerializeField] private Texture2D _cursorGrab;
    [SerializeField] private Texture2D _cursorHover;

    private void OnMouseEnter()
    {
        if (GameManager.Instance.IsPaused) return;
        
        Cursor.SetCursor(_cursorHover, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if(_isCarrying) Cursor.SetCursor(_cursorGrab, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsGameFinished || GameManager.Instance.IsPaused) return;
        _isCarrying = true;
        Cursor.SetCursor(_cursorGrab, Vector2.zero, CursorMode.Auto);
        _deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        _deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        if (GameManager.Instance.IsGameFinished || GameManager.Instance.IsPaused) return;
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(_mousePosition.x - _deltaX, _mousePosition.y - _deltaY + .1f, transform.position.z);
    }

    private void OnMouseUp()
    {
        if (GameManager.Instance.IsGameFinished || GameManager.Instance.IsPaused) return;
        _isCarrying = false;
        Cursor.SetCursor(_cursorHover, Vector2.zero, CursorMode.Auto);
        transform.position = new Vector3(_mousePosition.x - _deltaX, _mousePosition.y - _deltaY, transform.position.z);
    }
}
