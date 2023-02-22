using ScriptableObjects.script;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GameObject baseBlock;
    [SerializeField] private GameObject bombContainer;
    [SerializeField] private GameObject blockContainer;
    [SerializeField] private GameDifficultySo gameMod;
    [SerializeField] private AudioClip _explodeSFX;
    [SerializeField] private AudioClip _breakSFX;
  
    private int _flagCounter;

    public class BlockInfo
    {
        public Vector3Int Position { get; private set; }
        public bool IsBomb { get; private set; }
        public int BombCounter { get; private set; }
        
        // Block position in game grid.
        public int X { get => Position.x; }
		public int Y { get => Position.y; }

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
    private Block[,] _blocks;

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
        _grid = new BlockInfo[gameMod.Width, gameMod.Height];
        _blocks = new Block[gameMod.Width, gameMod.Height];
        if (Camera.main == null) return;
        
        var main = Camera.main;
        main.transform.position = new Vector3(gameMod.Width * 0.5f, gameMod.Height * 0.5f, -10);
        main.orthographicSize = (gameMod.Height / 2) + 2;
    }

    private void Start()
    {
        GameManager.Instance.InitBombCounter(gameMod.BombQuantity);

        CreateGrid();
        SetBomb();
        SetBlock();
    }

    private void CreateGrid()
    {
        for (int x = 0; x < gameMod.Width; x++)
        {
            for (int y = 0; y < gameMod.Height; y++)
            {
                BlockInfo info = new();
                info.Init(new Vector3Int(x, y, 0));
                _grid[x, y] = info;
            }
        }
    }

    private void SetBomb()
    {
        int bombPlaced = 0;
        
        while (bombPlaced < gameMod.BombQuantity)
        {
            int x = Random.Range(0, gameMod.Width);
            int y = Random.Range(0, gameMod.Height);
            
            if (_grid[x, y].IsBomb) continue;

            BlockInfo info = _grid[x, y];
            
            info.SetBomb();
            Vector3Int bombPos = info.Position;
            foreach (var position in _neighbourPositions)
            {
                Vector3Int neighbor = bombPos + position;
                if (neighbor.x >= gameMod.Width || neighbor.y >= gameMod.Height || neighbor.x < 0 || neighbor.y < 0)
                    continue;
                
                    
                _grid[neighbor.x, neighbor.y].IncrementBombCounter();
            }
                
            bombPlaced++;
        }
    }

    private void SetBlock()
    {
        foreach (BlockInfo info in _grid)
        {
            Transform parent = info.IsBomb ? bombContainer.transform : blockContainer.transform;
            GameObject blockObj = Instantiate(baseBlock, info.Position, Quaternion.identity, parent);
            Block infoComponent = blockObj.GetComponent<Block>();

            _blocks[info.X, info.Y] = infoComponent;
            infoComponent.BlockInfo = info;
            blockObj.name = info.IsBomb ? "Bomb" : "Empty";
            blockObj.GetComponent<AudioSource>().clip = info.IsBomb ? _explodeSFX : _breakSFX;
            infoComponent.Position = info.Position;
            infoComponent.SetBomb(info.IsBomb);
            infoComponent.SetBombAroundCounter(info.BombCounter);
        }
    }

	public void RevealBlock(BlockInfo info)
    {
        RevealBlock(info.X, info.Y);
    }

	public void RevealBlock(int x, int y)
    {
        Block b = _blocks[x, y];
        BlockInfo info = _grid[x, y];

        // Already revealed.
        if (b.Revealed) return;

        b.RevealThisBlock();

        if (info.IsBomb)
        {
            b.Explosion();
        }
        else
        {
            // Propagate.
            if (info.BombCounter != 0) return;

			foreach (Vector3Int position in _neighbourPositions)
			{
				Vector3Int neighbor = info.Position + position;
				if (neighbor.x >= gameMod.Width || neighbor.y >= gameMod.Height || neighbor.x < 0 || neighbor.y < 0)
					continue;

				RevealBlock(neighbor.x, neighbor.y);
			}
		}
    }
}
