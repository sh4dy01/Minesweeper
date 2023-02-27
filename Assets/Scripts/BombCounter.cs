using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;

public class BombCounter : MonoBehaviour
{
    [SerializeField] private TextMeshPro bombCounterText;
    private int _bombCounter;

    private void Start()
    {
        _bombCounter = GameManager.Instance.GameDifficulty.BombQuantity;
    }

    private void Update()
    {
        bombCounterText.text = _bombCounter.ToString("00");
    }
}
