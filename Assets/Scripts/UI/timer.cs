using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class timer : MonoBehaviour
{
    [SerializeField] private TextMeshPro timerText;
    private float _seconds;
    private int _minutes;

    private void Awake()
    {
        _seconds = 0;
    }

    private void Update()
    {
        if (_seconds < 60)
        {
            _seconds += Time.deltaTime;

        }
        else
        {
            _seconds = 0;
            _minutes++;
        }
        
        timerText.text = _minutes.ToString("00") + " : " + _seconds.ToString("00");
    }
}
