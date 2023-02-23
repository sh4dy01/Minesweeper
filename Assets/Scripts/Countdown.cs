using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshPro countDownText;
    [SerializeField] private AudioClip _audioClip;
    private AudioSource _audioSource;
    private float _internalTimer;
    private int _seconds;
    private int _minutes;

    private bool _isMusicChanged = false;

    private void Start()
    {
        _audioSource = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
        _internalTimer = GameManager.Instance.GameDifficulty.Countdown;
    }

    private void Update()
    {
        if (_internalTimer > 0)
        {
            _internalTimer -= Time.deltaTime;
            _seconds = Mathf.CeilToInt(_internalTimer);
            _minutes = _seconds / 60;
            _seconds = _seconds % 60;
        }

        countDownText.text = _minutes.ToString("00") + " : " + _seconds.ToString("00");

        if (_minutes <= 1 && _seconds <= 35)
        {
            if (!_isMusicChanged)
            {
                _isMusicChanged = true;
                ChangeMusic();
            }
        }
    }

    private void ChangeMusic()
    {
        _audioSource.clip = _audioClip;
        _audioSource.volume = 0.5f;
        _audioSource.pitch = 1;
        _audioSource.Play();
    }
}
