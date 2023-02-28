using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField] private TextMeshPro countDownText;
        [SerializeField] private AudioClip _alarmMusic;
        [SerializeField] private LightManager _lightManager;
        [SerializeField] [Range(0.1f, 0.8f)] private float alarmSwitchTrigger;

        private AudioSource _audioSource;
        private float _internalTimer;
        private int _alarmTrigger;
        private int _seconds;
        private int _minutes;
        private bool _isAlarmActivated;

        private void Awake()
        {
            _isAlarmActivated = false;
        }

        private void Start()
        {
            _internalTimer = GameManager.Instance.GameDifficulty.Countdown;
            _alarmTrigger = Mathf.CeilToInt(_internalTimer * alarmSwitchTrigger);
            
            Debug.Log("Will trigger at : " + _alarmTrigger);
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

            if (_minutes * 60 + _seconds <= _alarmTrigger)
            {
                if (!_isAlarmActivated)
                {
                    _isAlarmActivated = true;
                    _lightManager.ActivateAlarm();
                    MusicManager.Instance.ChangeMusic(_alarmMusic);
                }
            }

            if (_minutes <= 0 && _seconds <= 0)
            {
                GameManager.Instance.FinishTheGame(false);
            }
        }
    }
}
