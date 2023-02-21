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


	[SerializeField] private Sprite[] _bombCounterSprites = new Sprite[8];
	[SerializeField] private GameObject _flag;

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
        //Left click to open block
        if (Input.GetMouseButtonDown(0))
        {
             Sprite which = _bombSprite;
            if (!_isBomb)
                {
                    which = _bombCounter == 0 ? _emptySprite : _bombCounterSprites[_bombCounter - 1];
                 }
            _spriteRenderer.sprite = which;
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
}
