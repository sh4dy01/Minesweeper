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
        if (!Input.GetMouseButtonDown(0)) return;
        _spriteRenderer.sprite = _isBomb ? _bombSprite : _emptySprite;
    }
}
