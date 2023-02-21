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
    [SerializeField] private GameObject _bombContainer;
    [SerializeField] private GameObject _blockContainer;
    [SerializeField] private GameDifficultySo _gameMod;

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
        _grid = new Block[_gameMod._width, _gameMod._height];
        if (Camera.main != null) Camera.main.transform.position = new Vector3(_gameMod._width * 0.5f, _gameMod._height * 0.5f, -10);
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
        for (int x = 0; x < _gameMod._width; x++)
        {
            for (int y = 0; y < _gameMod._height; y++)
            {
                Block block = new Block();
                block.Position = new Vector3(x, y);
                _grid[y, x] = block;
            }
        }
    }

    private void SetBomb()
    {
        int bombPlaced = 0;
        
        while (bombPlaced < _gameMod._bombQuantity)
        {
            int x = Random.Range(0, _gameMod._width);
            int y = Random.Range(0, _gameMod._height);
            Debug.Log(_grid[x, y].IsBomb);
            if (_grid[x, y].IsBomb) continue;
            
            _grid[x, y].SetBomb(true);

            Vector3Int bombPos = new Vector3Int(x, y);
            foreach (var position in _neighbourPositions)
            {
                Vector3Int neighbor = bombPos + position;
                if (neighbor.x >= _gameMod._width || neighbor.y >= _gameMod._height || neighbor.x < 0 || neighbor.y < 0)
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
            Transform parent = block.IsBomb ? _bombContainer.transform : _blockContainer.transform;
            GameObject blockObj = Instantiate(_baseBlock, block.Position, Quaternion.identity, parent);
            
            blockObj.name = block.IsBomb ? "Bomb" : "Empty";
            blockObj.GetComponent<Block>().Position = block.Position;
            blockObj.GetComponent<Block>().SetBomb(block.IsBomb);
            blockObj.GetComponent<Block>().SetBombCounter(block.Bombcounter);
        }
    }
}
