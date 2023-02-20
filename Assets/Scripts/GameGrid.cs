using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private Block _block;
    
    [SerializeField] private int bombAmount;
    [SerializeField] private int width;
    [SerializeField] private int height;

    private bool[,] _grid;

    private void Awake()
    {
        _grid = new bool[width, height];
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
                _grid[x, y] = false;
            }
        }
        
        SetBomb();
    }

    void SetBomb()
    {
        for (int i = 0; i < bombAmount; i++)
        {
            while (true)
            {
                int x = Random.Range(0, 10);
                int y = Random.Range(0, 10);

                if (_grid[x, y] == false) _grid[x, y] = true;
                else
                    continue;
                break;
            }
        }
    }

    void SetBlock()
    {
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                Block block = _block;
                block.SetBomb(_grid[x,y]);
                Instantiate(block.gameObject, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }
}
