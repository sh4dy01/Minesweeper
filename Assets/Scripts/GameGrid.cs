using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GameObject _block;
    
    [SerializeField] private int bombAmount;
    [SerializeField] private int width;
    [SerializeField] private int height;

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
        _grid = new Block[width, height];
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        SetBlock();
    }

    void CreateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _grid[x, y] = _block.GetComponent<Block>();
                _grid[x, y].Position = new Vector3(x, y);
            }
        }
        
        SetBomb();
    }

    void SetBomb()
    {
        int bombPlaced = 0;
        
        while (bombPlaced < bombAmount)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            
            if (!_grid[x, y])
            {
                Vector3Int bombPos = new Vector3Int(x, y);
                foreach (var position in _neighbourPositions)
                {
                    Vector3Int neighbor = bombPos + position;
                    if (neighbor.x > width || neighbor.y > height || neighbor.x < 0 || neighbor.y < 0)
                    {
                        continue;
                    }
                    
                    _grid[neighbor.x, neighbor.y].IncrementBombCounter();
                }
                
                _grid[x, y].SetBomb();
                bombPlaced++;
            }
        }
    }

    void SetBlock()
    {
        foreach (var block in _grid)
        {
            GameObject blockObj = _block;
            blockObj.transform.position = block.Position;
            Instantiate(_block, blockObj.transform.position, Quaternion.identity);
        }
    }
}
