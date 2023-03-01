using ScriptableObjects.script;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;
using Random = System.Random;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameDifficultySo _difficulty;

        private UIManager _uiManager;
        private int _maxBombCounter;
        
        public bool IsPaused { get; private set; }
        public bool IsGameFinished { get; private set; }
        public int BombCounter { get; private set; }
        public bool IsSeedSet { get; private set; }
        public int Seed { get; private set; }

        public GameDifficultySo GameDifficulty => _difficulty;
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
            
			if (_difficulty == null)
            {
                Debug.LogError("Difficulty isn't set !");
                return;
            }

            IsPaused = false;
            IsGameFinished = false;
            _maxBombCounter = _difficulty.BombQuantity;
            BombCounter = _maxBombCounter;
            _uiManager.UpdateBombText(BombCounter);
        }

        public void SetGameSeed(string seedText)
        {
            IsSeedSet = true;
            Seed = Mathf.Abs(int.Parse(seedText));
        }

        private void ResetGameSeed() => IsSeedSet = false;

        // Called when the player places a flag. Updates the remaining bombs counter on screen.
        public void DecreaseBombCounter()
        {
            if (BombCounter <= 0) return;
            BombCounter--;
            _uiManager.UpdateBombText(BombCounter);
        }

		// Called when the player removes a flag. Updates the remaining bombs counter on screen.
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

        // Mark the game as finished. ALso makes a call to the UI manager to
        // display the win or lose UI depending on the "win" parameter.
        public void FinishTheGame(bool win)
        {
            if (IsGameFinished) return;

            IsGameFinished = true;

            if (win) _uiManager.ShowWinUI();
            else _uiManager.ShowLoseUI();
        }

        public void Reset()
        {
            ResetGameSeed();
        }

        public void SwitchPauseState()
        {
            IsPaused = !IsPaused;
        }
    }
}