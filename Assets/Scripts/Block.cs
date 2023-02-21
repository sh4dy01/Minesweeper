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

	[SerializeField] private Sprite[] _bombCounterSprites = new Sprite[8];

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
        Debug.Log(_isBomb);

        Sprite which = _bombSprite;
		if (!_isBomb)
        {
            which = _bombCounter == 0 ? _emptySprite : _bombCounterSprites[_bombCounter - 1];
		}
        _spriteRenderer.sprite = which;
    }
}
