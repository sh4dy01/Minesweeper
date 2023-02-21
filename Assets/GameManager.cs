using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _bombCounter;
    private UIManager _uiManager;

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
        _bombCounter = bombs;
        _uiManager.UpdateBombText(_bombCounter);
    } 
    public void DecreaseBombCounter()
    {
        _bombCounter--;
        _uiManager.UpdateBombText(_bombCounter);
    }
}
