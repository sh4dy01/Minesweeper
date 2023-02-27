using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bombText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private string baseTimerText;
    [SerializeField] private string baseBombText;
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _loseUI;

    private float _timer;

    private void Awake()
    {
        _timer = 0;
        timerText.text = baseTimerText + "00";
        bombText.text = baseBombText;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (!(_timer >= 1)) return;
        
        timerText.text = baseTimerText + Time.timeSinceLevelLoad.ToString("00");
        _timer = 0;
    }

    public void UpdateBombText(int bombCounter)
    {
        bombText.text = baseBombText + bombCounter.ToString("00");
    }

    public void ShowWinUI()
    {
        _winUI.SetActive(true);
    }

    public void ShowLoseUI()
    {
        _loseUI.SetActive(true);
    }

    public void GameScene()
    {
        SceneManager.LoadScene("game");
    }

    public void LobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}
