using ScriptableObjects.script;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager _uiManager;
    private int _maxBombCounter;
    [SerializeField] private GameDifficultySo _difficulty;

    public int BombCounter { get; private set; }
    public GameDifficultySo GameDifficulty => _difficulty;

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

    public void InitBombCounter(int bombs)
    {
        _uiManager = FindObjectOfType<UIManager>();
        _maxBombCounter = bombs;
        BombCounter = bombs;
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
}
