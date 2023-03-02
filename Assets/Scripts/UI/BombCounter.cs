using TMPro;
using UnityEngine;

public class BombCounter : MonoBehaviour
{
    [SerializeField] private TextMeshPro _bombCounterText;

    public void UpdateCounter(int bombCounter)
    {
        _bombCounterText.text = bombCounter.ToString("00");
    }
}
