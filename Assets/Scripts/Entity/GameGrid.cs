using System.Collections.Generic;
using Managers;
using ScriptableObjects.script;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using SRandom = System.Random;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GameObject _baseBlock;
    [SerializeField] private GameObject _borderBlock;

    [SerializeField] private GameObject _bombContainer;
    [SerializeField] private GameObject _blockContainer;
    [SerializeField] private AudioClip _explodeSfx;
    [SerializeField] private AudioClip _breakSfx;
	[SerializeField] private GameObject _explosionParticles;

	private AudioSource _blockAudioSource;
    
    private GameDifficultySo _gameMod;
    private int _flagCounter;
	private int _numBlocks;
	private float _gameScale;
	private Vector2Int _gridSize;
	private SRandom _random;

    private bool _firstClickOccurred;

	// Shaking
	private Vector3 _originalPosition;
    private float _shakeIntensity;

    // Block data structure
    public class BlockInfo
    {
        public Vector3 WorldPosition { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        public bool IsBomb { get; private set; }
        public int NumBombsAround { get; private set; }
        public float BlockScale { get; private set; }

        public bool Revealed { get; set; }
		public bool Flagged { get; set; }

		// Block position in game grid.
		public int GridX => GridPosition.x;
        public int GridY => GridPosition.y;

        public void IncrementBombCounter() => NumBombsAround++;
        public void SetBomb(bool b = true) => IsBomb = b;

		public void Init(Vector2Int position, Vector3 worldPosition)
        {
            IsBomb = false;
			NumBombsAround = 0;
            GridPosition = position;
            WorldPosition = worldPosition;

            Revealed = false;
			Flagged = false;

		}
    }
    
    private BlockInfo[,] _grid;
    private Block[,] _blocks;
    private List<GameObject> _borderBlockList;

    private readonly Vector2Int[] _neighbourPositions = 
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.up + Vector2Int.right,
        Vector2Int.up + Vector2Int.left,
        Vector2Int.down + Vector2Int.right,
        Vector2Int.down + Vector2Int.left
    };

    private void Awake()
    {
        GameManager.Instance.InitializeGame();
        _random = GameManager.Instance.IsSeedSet ? new SRandom(GameManager.Instance.Seed) : new SRandom();
        
        _gameMod = GameManager.Instance.GameDifficulty;
        _gridSize = new Vector2Int(_gameMod.Width+2,_gameMod.Height+2);
        _grid = new BlockInfo[_gameMod.Width, _gameMod.Height];
        _blocks = new Block[_gameMod.Width, _gameMod.Height];
        _borderBlockList = new List<GameObject>();

        _blockAudioSource = GetComponent<AudioSource>();
        _blockAudioSource.clip = _breakSfx;

		_shakeIntensity = 0.0F;
		_numBlocks = _gameMod.Width * _gameMod.Height - _gameMod.BombQuantity;

		_firstClickOccurred = false;

		if (Camera.main == null) return;

		var main = Camera.main;
		if (main == null) return;
		
		//get camera size
		float height = 2f * main.orthographicSize;
		float width = height * main.aspect * 0.8f;
		float border = 1f;
		
		//test height and width and take the smaller scale to avoid out of camera blocks
		_gameScale = (height - border) / (_gridSize.y);
		float tempGameScale = (width - border) / (_gridSize.x);
		if (_gameScale > tempGameScale) _gameScale = tempGameScale;
        
        //main.transform.position = new Vector3(_gameMod.Width * 0.5f, _gameMod.Height * 0.5f, -10);
        //main.orthographicSize = main.orthographicSize * 0.75f;

        _originalPosition = transform.position;
	}

    private void Start()
    {
	    CreateGrid();
	}

	private void Update()
	{
        _shakeIntensity *= 0.85F;
        if (_shakeIntensity < 0.0F)
        {
            _shakeIntensity = 0.0F;
		}

        Vector3 randomOffset = new Vector3(Random.Range(-1.0F, 1.0F), Random.Range(-1.0F, 1.0F), 0.0F) * _shakeIntensity;
        transform.SetPositionAndRotation(randomOffset, Quaternion.identity);
	}

	private void CreateGrid()
    {
	    float halfWidth = (_gameMod.Width+1.5f) * 0.5f;
	    float halfHeight = (_gameMod.Height+0.5f) * 0.5f;

	    for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
	            if (x == 0 || x == _gridSize.x-1 || y == 0 || y == _gridSize.y-1)
	            {
		            var block = Instantiate(_borderBlock,
			            new Vector3((x - halfWidth) * _gameScale, (y - halfHeight) * _gameScale, 0),
			            Quaternion.identity, this.transform);
		            block.transform.localScale = new Vector3(_gameScale,_gameScale,0);
		            _borderBlockList.Add(block);
	            }
	            else
	            {
		            BlockInfo info = new();
		            info.Init(new Vector2Int(x-1, y-1), new Vector3((x - halfWidth) * _gameScale, (y - halfHeight) * _gameScale, 0));
		            _grid[x-1, y-1] = info;
		            
		            // Create game block.
		            Transform parent = info.IsBomb ? _bombContainer.transform : _blockContainer.transform;
		            GameObject blockObj = Instantiate(_baseBlock, info.WorldPosition, Quaternion.identity, parent);

		            // Link block info by reference.
		            Block blockComponent = blockObj.GetComponent<Block>();
		            blockComponent.BlockInfo = info;
		            blockComponent.transform.localScale = new Vector3(_gameScale, _gameScale, 0);
		            _blocks[info.GridX, info.GridY] = blockComponent;
	            }
            }


        }
    }

    // Layout bombs on the grid. This is called after the first click, to prevent the player from
    // dying on the first hit.
    private void PlaceBombs(Vector2Int firstClickPos)
    {
		int bombPlaced = 0;
        
        while (bombPlaced < _gameMod.BombQuantity)
        {
            int x = _random.Next(0, _gameMod.Width);
            int y = _random.Next(0, _gameMod.Height);
			BlockInfo info = _grid[x, y];

			if (info.IsBomb) continue;
            if (Mathf.Abs(x - firstClickPos.x) <= 1 && Mathf.Abs(y - firstClickPos.y) <= 1) continue;
            
            info.SetBomb();
            Vector2Int bombPos = info.GridPosition;
            foreach (var position in _neighbourPositions)
            {
                Vector2Int neighbor = bombPos + position;
                if (neighbor.x >= _gameMod.Width || neighbor.y >= _gameMod.Height || neighbor.x < 0 || neighbor.y < 0)
                    continue;
                    
                _grid[neighbor.x, neighbor.y].IncrementBombCounter();
            }

            bombPlaced++;
        }
	}

	public void RevealBlock(BlockInfo info)
    {
		RevealBlock(info.GridX, info.GridY);

		_blockAudioSource.Play();
	}

    // Reveals a clicked block, and recursively reveals the empty blocks around it.
    // This also checks if one of the blocks is a mine. If so, it will cause the game to end.
    private void RevealBlock(int bx, int by)
    {
		// Stack used to hold the positions of blocks to reveal.
		Stack<(int, int)> positions = new Stack<(int, int)>(256);
        positions.Push((bx, by));

        // Reveal blocks in stack until there's no more.
        while (positions.Count != 0)
        {
            (int, int) pos = positions.Pop();
            int x = pos.Item1;
            int y = pos.Item2;

            Block b = _blocks[x, y];
            BlockInfo info = b.BlockInfo;

			// Already revealed.
			if (info.Revealed) continue;

			info.Revealed = true;

            // First click.
            if (!_firstClickOccurred)
            {
                _firstClickOccurred = true;
                PlaceBombs(new Vector2Int(x, y));
            }

            b.RevealThisBlock();

            // Add to shake intensity.
            // With recursion, the effect will add up, shaking more vigorously the more tiles are revealed at one time.
            _shakeIntensity += 0.02F;
            if (_shakeIntensity > 10.0F) _shakeIntensity = 10.0F;

            if (info.IsBomb)
            {
                GameManager.Instance.FinishTheGame(false);
                _blockAudioSource.clip = _explodeSfx;
                b.Explosion();

                foreach (var block in _borderBlockList) block.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                // Play particles effect.
                Instantiate(_explosionParticles, info.WorldPosition + new Vector3(0.5F, 0.5F, -1.0F), Quaternion.identity);
            }
            else
            {
				// Win detection.
				_numBlocks--;
				if (_numBlocks == 0)
				{
					GameManager.Instance.FinishTheGame(true);
                    return;
				}

				// Propagate.
                if (info.NumBombsAround == 0)
                            {
                    foreach (Vector2Int position in _neighbourPositions)
                    {
                        int nx = x + position.x;
                        int ny = y + position.y;

                        // Out of bounds.
                        if (nx >= _gameMod.Width || ny >= _gameMod.Height || nx < 0 || ny < 0)
                            continue;

                        // Revealed.
                        if (_grid[nx, ny].Revealed)
                            continue;

                        positions.Push((nx, ny));
                    }
                }
            }
        }

        _firstClickOccurred = true;
	}

    // Called when the player presses a number. It will try to reveal the squares around it, if there
    // are enough flags.
    public void RevealAround(BlockInfo info)
    {
        if (RevealAround(info.GridX, info.GridY, info.NumBombsAround))
        {
            _blockAudioSource.Play();
        }
    }

	private bool RevealAround(int x, int y, int requiredFlags)
    {
        // Get number of surrounding flags.
        int numFlags = 0;
		foreach (Vector3Int position in _neighbourPositions)
		{
			Vector3Int neighbor = new Vector3Int(x, y) + position;
			if (neighbor.x >= _gameMod.Width || neighbor.y >= _gameMod.Height || neighbor.x < 0 || neighbor.y < 0)
				continue;

			if (_grid[neighbor.x, neighbor.y].Flagged)
            {
                numFlags++;
            }
		}

        // Not enough.
        if (numFlags < requiredFlags) return false;

        bool blocksFound = false;
		foreach (Vector3Int position in _neighbourPositions)
		{
			Vector3Int neighbor = new Vector3Int(x, y) + position;
			if (neighbor.x >= _gameMod.Width || neighbor.y >= _gameMod.Height || neighbor.x < 0 || neighbor.y < 0)
				continue;

			if (!_grid[neighbor.x, neighbor.y].Flagged && !_grid[neighbor.x, neighbor.y].Revealed)
			{
                RevealBlock(neighbor.x, neighbor.y);
                blocksFound = true;
			}
		}

        return blocksFound;
	}
}
