using ScriptableObjects.script;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameDifficultySo difficulty;

        private UIManager _uiManager;
        private int _maxBombCounter;

        public bool IsFinished { get; private set; }
        public int BombCounter { get; private set; }
        public bool IsSeedSet { get; private set; }
        public int Seed { get; private set; }

        public GameDifficultySo GameDifficulty => difficulty;
        public GameGrid GameGrid { get; private set; }

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
            GameGrid = FindObjectOfType<GameGrid>();
            _uiManager = FindObjectOfType<UIManager>();

            IsFinished = false;
            _maxBombCounter = difficulty.BombQuantity;
            BombCounter = _maxBombCounter;
            _uiManager.UpdateBombText(BombCounter);
        }

        public void SetGameSeed(string seedText)
        {
            IsSeedSet = true;
            Seed = Mathf.Abs(int.Parse(seedText));
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
            this.difficulty = difficulty;
        }

        public void FinishTheGame()
        {
            IsFinished = true;
        }
    }
}
