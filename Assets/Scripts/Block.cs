using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private bool _isBomb;
    [SerializeField] private int _bombCounter;
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Sprite _bombSprite;
    [SerializeField] private GameObject _flag;
    [SerializeField] private Texture2D _screwdriverCursor;
    
    private SpriteRenderer _spriteRenderer;

    public Vector3 Position { get; set; }

    public bool IsBomb => _isBomb;
    public int Bombcounter => _bombCounter;
    public void SetBomb(bool value) => _isBomb = value;
    public void IncrementBombCounter() => _bombCounter++;
    public void SetBombCounter(int value) => _bombCounter = value;

    // Start is called before the first frame update
    private void Awake()
    {
        _bombCounter = 0;
        _isBomb = false;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        Cursor.SetCursor(_screwdriverCursor, Vector2.zero, CursorMode.ForceSoftware);
        
        //Left click to open block
        if (Input.GetMouseButtonDown(0) && !_flag.activeSelf)
        {
            _spriteRenderer.sprite = _isBomb ? _bombSprite : _emptySprite;
        }
        
        //Right click to place a flag
        if (_spriteRenderer.sprite == _emptySprite || _spriteRenderer.sprite == _bombSprite)
        {
            if(_flag.activeSelf) _flag.SetActive(false); 
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            _flag.SetActive(!_flag.activeSelf);
        }
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
