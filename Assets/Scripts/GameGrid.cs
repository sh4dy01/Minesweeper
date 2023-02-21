using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GameObject baseBlock;
    [SerializeField] private GameObject bombContainer;
    [SerializeField] private GameObject blockContainer;
    [SerializeField] private int bombAmount;
    [SerializeField] private int width;
    [SerializeField] private int height;
    
    private struct BlockInfo
    {
        public Vector3Int Position { get; private set; }
        public bool IsBomb { get; private set; }
        public int BombCounter { get; private set; }
        
        public void IncrementBombCounter() => BombCounter++;
        public void SetBomb() => IsBomb = true;
        
        public void Init(Vector3Int position)
        {
            IsBomb = false;
            BombCounter = 0;
            Position = position;
        }
    }
    
    private BlockInfo[,] _grid;

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
        _grid = new BlockInfo[width, height];
        if (Camera.main != null) Camera.main.transform.position = new Vector3(width * 0.5f, height * 0.5f, -10);
    }

    private void Start()
    {
        CreateGrid();
        SetBomb();
        SetBlock();
    }

    private void CreateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                BlockInfo info = new();
                info.Init(new Vector3Int(x, y, 0));
                _grid[y, x] = info;
            }
        }
    }

    private void SetBomb()
    {
        int bombPlaced = 0;
        
        while (bombPlaced < bombAmount)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            
            if (_grid[x, y].IsBomb) continue;

            BlockInfo info = _grid[x, y];
            
            info.SetBomb();
            Vector3Int bombPos = info.Position;
            foreach (var position in _neighbourPositions)
            {
                Vector3Int neighbor = bombPos + position;
                if (neighbor.x >= width || neighbor.y >= height || neighbor.x < 0 || neighbor.y < 0)
                    continue;
                    
                _grid[neighbor.x, neighbor.y].IncrementBombCounter();
            }
                
            bombPlaced++;
        }
    }

    private void SetBlock()
    {
        foreach (var info in _grid)
        {
            Transform parent = info.IsBomb ? bombContainer.transform : blockContainer.transform;
            GameObject blockObj = Instantiate(baseBlock, info.Position, Quaternion.identity, parent);
            Block infoComponent = blockObj.GetComponent<Block>();
            
            blockObj.name = info.IsBomb ? "Bomb" : "Empty";
            infoComponent.Position = info.Position;
            infoComponent.SetBomb(info.IsBomb);
            infoComponent.SetBombCounter(info.BombCounter);
        }
    }
}
