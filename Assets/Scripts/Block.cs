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
                which = _bombAroundCounter == 0 ? _emptySprite : _bombCounterSprites[_bombAroundCounter - 1];
            }
            else
            {
                GameManager.Instance.DecreaseBombCounter();
            }
            
            _spriteRenderer.sprite = which;
        } 
        //Right click to flag a block
        else if (Input.GetMouseButtonDown(1))
        {
            if (_spriteRenderer.sprite == _emptySprite || _spriteRenderer.sprite == _bombSprite)
            {
                if(_flag.activeSelf) _flag.SetActive(false); 
                return;
            }
            
            _flag.SetActive(!_flag.activeSelf);
        }
    }
}
