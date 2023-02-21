using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bombText;
    [SerializeField] private TextMeshProUGUI timerText;

    private float timer = 0;
    
    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;

        if (!(timer >= 1)) return;
        
        timerText.text = Time.timeSinceLevelLoad.ToString("00");
        timer = 0;
    }
}
