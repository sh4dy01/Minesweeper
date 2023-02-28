using TMPro;
using UnityEngine;

public class BombCounter : MonoBehaviour
{
    [SerializeField] private TextMeshPro bombCounterText;

    public void UpdateCounter(int bombCounter)
    {
        bombCounterText.text = bombCounter.ToString("00");
    }
}
