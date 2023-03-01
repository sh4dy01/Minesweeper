using System.Collections;
using System.Collections.Generic;
using Managers;
using Unity.Mathematics;
using UnityEngine;

public class BlockNote : MonoBehaviour
{
    private Vector2 mousePosition;
    private float deltaX, deltaY;
    private int _numPages = 3;
    private bool _isGrab = false;
    
    [SerializeField] private GameObject _paper;

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsGameFinished || GameManager.Instance.IsPaused) return;

        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        if (GameManager.Instance.IsGameFinished || GameManager.Instance.IsPaused) return;

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
    }
}
