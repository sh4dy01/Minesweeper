using System.Linq;
using ScriptableObjects.script;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameDifficultySo _difficulty;
        [SerializeField] private Canvas _winUI;

        private GameGrid _gameGrid;
        private UIManager _uiManager;
        private int _maxBombCounter;
        private bool _isFinished;

        private int? _customWidth;
        private int? _customHeight;
        private int? _customBombQuantity;
        private string _errorMessage;

        public bool IsFinished { get; private set; }
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
            IsFinished = false;
            _maxBombCounter = _difficulty.BombQuantity;
            BombCounter = _maxBombCounter;
            _uiManager.UpdateBombText(BombCounter);
        }

        public void SetCustomWidth(string width)
        {
            if (width.Any(c => c is < '0' or > '9')) return;
            _customWidth = System.Convert.ToInt16(width);
        }
        
        public void SetCustomHeight(string height)
        {
            if (height.Any(c => c is < '0' or > '9')) return;
            _customHeight = System.Convert.ToInt16(height);
        }
        
        public void SetCustomBombQuantity(string bombQuantity)
        {
            if (bombQuantity.Any(c => c is < '0' or > '9')) return;
            _customBombQuantity = System.Convert.ToInt16(bombQuantity);
        }

        public void ApplyCustomValues(GameDifficultySo gameDifficultySo)
        {
            if (_customHeight == null || _customWidth == null || _customBombQuantity == null || _customHeight < 1 || _customWidth < 1 ||_customBombQuantity < 1 || _customHeight > 50 || _customWidth > 50)
            {
                _errorMessage = "One of the value is incorrect";
                return;
            }

            if (_customBombQuantity > (_customHeight * _customWidth) / 3)
            {
                _errorMessage = "There are to many bombs";
                return;
            }
            gameDifficultySo.SetHeight((int)_customHeight);
            gameDifficultySo.SetWidth((int)_customWidth);
            gameDifficultySo.SetBombQuantity((int)_customBombQuantity);
            GameScene();
        }

        public void PrintErrorMessage(TextMeshProUGUI text)
        {
            text.text = _errorMessage;
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
            _difficulty = difficulty;
        }

        public void FinishTheGame(bool win)
        {
            _isFinished = true;

            if (win) _winUI.enabled = true;
        }

        #region SceneModify
        
        public void GameScene()
        {
            SceneManager.LoadScene("game");
        }

        public void LobbyScene()
        {
            SceneManager.LoadScene("Lobby");
        }
        
        #endregion
    }
}