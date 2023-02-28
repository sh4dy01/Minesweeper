using Managers;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshPro countDownText;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private int timeToChangeMusic;
    [SerializeField] private LightManager _lightManager;
    
    private AudioSource _audioSource;
    private float _internalTimer;
    private int _seconds;
    private int _minutes;

    private bool _isMusicChanged = false;

    private void Start()
    {
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

        if (_minutes * 60 + _seconds <= timeToChangeMusic)
        {
            if (!_isMusicChanged)
            {
                _isMusicChanged = true;
                _lightManager.ActivateAlarm();
                ChangeMusic();
            }
        }

        if (_minutes <= 0 && _seconds <= 0)
        {
            GameManager.Instance.FinishTheGame(false);
        }
    }

    private void ChangeMusic()
    {
        MusicManager.Instance.ChangeMusic(_audioClip);
    }
}
