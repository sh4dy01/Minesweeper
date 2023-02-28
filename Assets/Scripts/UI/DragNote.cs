using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DragNote : MonoBehaviour
{
    private Vector2 mousePosition;
    private float deltaX, deltaY;
    private int _numPages = 3;
    private bool _isGrab = false;
    
    [SerializeField] private Texture2D _cursorGrab;
    [SerializeField] private GameObject _paper;

    private void OnMouseDown()
    {
        Cursor.SetCursor(_cursorGrab, Vector2.zero, CursorMode.ForceSoftware);
        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (_numPages > 0 && !_isGrab)
        {
            Instantiate(_paper, new Vector3(mousePosition.x - deltaX, mousePosition.y - deltaY + .1f, transform.position.z), quaternion.identity);
            _numPages--;
            _isGrab = true;
        }
        else if (_numPages <= 0)
        {
            transform.position = new Vector3(mousePosition.x - deltaX, mousePosition.y - deltaY + .1f, transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        _isGrab = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if(_numPages <= 0)
            transform.position = new Vector3(mousePosition.x - deltaX, mousePosition.y - deltaY - .1f, transform.position.z);
    }
}
