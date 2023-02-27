using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DragTimer : MonoBehaviour
{
    private Vector2 mousePosition;
    private float deltaX, deltaY;
    
    [SerializeField] private Texture2D _cursorGrab;

    private void OnMouseDown()
    {
        Cursor.SetCursor(_cursorGrab, Vector2.zero, CursorMode.ForceSoftware);
        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x - deltaX, mousePosition.y - deltaY + .1f, transform.position.z);
        Debug.Log(transform.position);
    }

    private void OnMouseUp()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        transform.position = new Vector3(mousePosition.x - deltaX, mousePosition.y - deltaY - .1f, transform.position.z);
    }
}
