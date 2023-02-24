using ScriptableObjects.script;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameDifficultySo _difficulty;

    private GameGrid _gameGrid;
    private UIManager _uiManager;
    private int _maxBombCounter;
    private bool _isFinished;

    public bool IsFinished => _isFinished;
    public int BombCounter { get; private set; }
    public GameDifficultySo GameDifficulty => _difficulty;
    public GameGrid GameGrid { get => _gameGrid; }

    #region Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    #endregion

    public void InitializeGame()
    {
        _gameGrid = FindObjectOfType<GameGrid>();
        _uiManager = FindObjectOfType<UIManager>();

        _isFinished = false;
        _maxBombCounter = _difficulty.BombQuantity;
        BombCounter = _maxBombCounter;
        _uiManager.UpdateBombText(BombCounter);
    } 

    public void DecreaseBombCounter()
    {
        if (BombCounter <= 0) return;
        BombCounter--;
        _uiManager.UpdateBombText(BombCounter);
    }

    public void IncreaseBombCounter()
    {
        if (BombCounter >= _maxBombCounter) return;
        BombCounter++;
        _uiManager.UpdateBombText(BombCounter);
    }

    public void SetDifficulty(GameDifficultySo difficulty)
    {
        _difficulty = difficulty;
    }

    public void FinishTheGame()
    {
        _isFinished = true;
        
    }
}
