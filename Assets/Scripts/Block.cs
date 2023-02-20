using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private bool _isBomb = false;
    [SerializeField] private int _bombCounter = 0;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Sprite _bombSprite;
    private Vector3 _position;
    public Vector3 Position
    {
        get => _position;
        set => _position = value;
    }

    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isBomb)
            {
                _spriteRenderer.sprite = _bombSprite;
            }
            else
            {
                _spriteRenderer.sprite = _emptySprite;
            }
        }
    }

    public void SetBomb() => _isBomb = true;
    public void IncrementBombCounter() => _bombCounter++;
}
