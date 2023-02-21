using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private bool _isBomb;
    [SerializeField] private int _bombAroundCounter;
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Sprite _bombSprite;
    
    private SpriteRenderer _spriteRenderer;

    public Vector3 Position { get; set; }

    public bool IsBomb => _isBomb;
    public int BombAroundCounter => _bombAroundCounter;
    public void SetBomb(bool value) => _isBomb = value;
    public void SetBombAroundCounter(int value) => _bombAroundCounter = value;

    // Start is called before the first frame update
    private void Awake()
    {
        _bombAroundCounter = 0;
        _isBomb = false;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        _spriteRenderer.sprite = _isBomb ? _bombSprite : _emptySprite;
        if (_isBomb)
        {
            GameManager.Instance.DecreaseBombCounter();
        }
    }
}
