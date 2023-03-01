using System;
using Managers;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    
    private GameGrid _grid;

    private void Awake()
    {
        _grid = FindObjectOfType<GameGrid>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        if (GameManager.Instance.IsPaused)
        {
            ResumeTheGame();
        }
        else
        {
            PauseTheGame();
        }
    }

    public void ResumeTheGame()
    {
        _pauseMenu.SetActive(false);
        GameManager.Instance.SwitchPauseState();
        MusicManager.Instance.ResumeMusic();
        Time.timeScale = 1;
        _grid.gameObject.SetActive(true);
    }
    
    private void PauseTheGame()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
        MusicManager.Instance.PauseMusic();
        GameManager.Instance.SwitchPauseState();
        _grid.gameObject.SetActive(false);
    }
}
