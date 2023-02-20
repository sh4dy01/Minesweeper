using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private bool _isBomb = false;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Sprite _bombSprite;
    
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        
    }

    public void SetBomb(bool value) => _isBomb = value;
    
}
