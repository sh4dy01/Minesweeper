using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Managers;

public class DragProps : MonoBehaviour
{
    private Vector2 mousePosition;
    private float deltaX, deltaY;
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
        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        if (GameManager.Instance.IsGameFinished || GameManager.Instance.IsPaused) return;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x - deltaX, mousePosition.y - deltaY + .1f, transform.position.z);
    }

    private void OnMouseUp()
    {
        if (GameManager.Instance.IsGameFinished || GameManager.Instance.IsPaused) return;
        _isCarrying = false;
        Cursor.SetCursor(_cursorHover, Vector2.zero, CursorMode.Auto);
        transform.position = new Vector3(mousePosition.x - deltaX, mousePosition.y - deltaY, transform.position.z);
    }
}
