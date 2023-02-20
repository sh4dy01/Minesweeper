using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GameObject _baseBlock;
    [SerializeField] private int _bombAmount;
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    private Block[,] _grid;
    
    private readonly Vector3Int[] _neighbourPositions = 
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.up + Vector3Int.right,
        Vector3Int.up + Vector3Int.left,
        Vector3Int.down + Vector3Int.right,
        Vector3Int.down + Vector3Int.left
    };

    private void Awake()
    {
        _grid = new Block[_width, _height];
    }

    // Start is called before the first frame update
    private void Start()
    {
        CreateGrid();
        SetBomb();
        SetBlock();
    }

    private void CreateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Block block = gameObject.AddComponent<Block>();
                block.Position = new Vector3(x, y);
                _grid[y, x] = block;
            }
        }
    }

    private void SetBomb()
    {
        int bombPlaced = 0;
        
        while (bombPlaced < _bombAmount)
        {
            int x = Random.Range(0, _width);
            int y = Random.Range(0, _height);
            Debug.Log(_grid[x, y].IsBomb);
            if (_grid[x, y].IsBomb) continue;
            
            _grid[x, y].SetBomb(true);

            Vector3Int bombPos = new Vector3Int(x, y);
            foreach (var position in _neighbourPositions)
            {
                Vector3Int neighbor = bombPos + position;
                if (neighbor.x >= _width || neighbor.y >= _height || neighbor.x <= 0 || neighbor.y <= 0)
                {
                    continue;
                }
                    
                _grid[neighbor.x, neighbor.y].IncrementBombCounter();
            }
                
            bombPlaced++;
        }
    }

    private void SetBlock()
    {
        foreach (var block in _grid)
        {
            GameObject blockObj = Instantiate(_baseBlock, block.Position, Quaternion.identity, transform);
            blockObj.name = block.IsBomb ? "Bomb" : "Empty";
            blockObj.GetComponent<Block>().Position = block.Position;
            blockObj.GetComponent<Block>().SetBomb(block.IsBomb);
        }
    }
}
