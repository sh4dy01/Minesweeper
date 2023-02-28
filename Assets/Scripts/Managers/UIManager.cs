using Managers;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private BombCounter bombCount;
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _loseUI;

    private float _timer;

    private void Awake()
    {
        _timer = 0;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (!(_timer >= 1)) return;
        
        _timer = 0;
    }

    public void UpdateBombText(int bombCounter)
    {
        bombCount.UpdateCounter(bombCounter);
    }

    public void ShowWinUI()
    {
        _winUI.SetActive(true);
    }

    public void ShowLoseUI()
    {
        _loseUI.SetActive(true);
    }

    public void LoadGameScene()
    {
        SceneLoader.LoadGameScene();
    }
    
    public void LoadLobbyScene()
    {
        SceneLoader.LoadLobbyScene();
    }
}
