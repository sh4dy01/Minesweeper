using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager _uiManager;
    private int _maxBombCounter;

    public int BombCounter { get; private set; }

    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            Instance._uiManager = FindObjectOfType<UIManager>();
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
}
