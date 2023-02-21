using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.script;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GameObject baseBlock;
    [SerializeField] private GameObject bombContainer;
    [SerializeField] private GameObject blockContainer;
    [SerializeField] private GameDifficultySo _gameMod;

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
        _grid = new BlockInfo[_gameMod.Width, _gameMod.Height];
        if (Camera.main != null) Camera.main.transform.position = new Vector3(_gameMod.Width * 0.5f, _gameMod.Height * 0.5f, -10);
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
        for (int x = 0; x < _gameMod.Width; x++)
        {
            for (int y = 0; y < _gameMod.Height; y++)
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
        
        while (bombPlaced < _gameMod.BombQuantity)
        {
            int x = Random.Range(0, _gameMod.Width);
            int y = Random.Range(0, _gameMod.Height);
            Debug.Log(_grid[x, y].IsBomb);
            if (_grid[x, y].IsBomb) continue;

            BlockInfo info = _grid[x, y];
            
            info.SetBomb();
            Vector3Int bombPos = info.Position;
            foreach (var position in _neighbourPositions)
            {
                Vector3Int neighbor = bombPos + position;
                if (neighbor.x >= _gameMod.Width || neighbor.y >= _gameMod.Height || neighbor.x < 0 || neighbor.y < 0)
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
