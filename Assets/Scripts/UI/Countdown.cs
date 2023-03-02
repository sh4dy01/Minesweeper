using Managers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _countDownText;
        [SerializeField] private LightManager _lightManager;
        [SerializeField] [Range(0.1f, 0.25f)] private float _alarmSwitchTrigger;
        [SerializeField] [Range(0, 0.20f)] private float _minLightEventTrigger;
        [SerializeField] [Range(0.20f, 0.60f)] private float _maxLightEventTrigger;
        
        private AudioSource _audioSource;
        private float _internalTimer;
        private int _seconds;
        private int _minutes;
        private int _alarmTrigger;
        private bool _isAlarmActivated;
        private bool _isLightEventTriggered;
        private int _lightEventTrigger;
        
        private void Awake()
        {
            _isAlarmActivated = false;
        }

        private void Start()
        {
            _lightManager.OnSwitchLightsOn += SetLightTriggerEvent;
            _internalTimer = GameManager.Instance.GameDifficulty.Countdown;
            _alarmTrigger = Mathf.CeilToInt(_internalTimer * _alarmSwitchTrigger);
            SetLightTriggerEvent();
        }

        private void SetLightTriggerEvent()
        {
            int min = Mathf.CeilToInt(_internalTimer * _minLightEventTrigger);
            int max = Mathf.CeilToInt(_internalTimer * _maxLightEventTrigger);
            _lightEventTrigger = Mathf.CeilToInt(_internalTimer) - Random.Range(min, max);
        }

        private void Update()
        {
            if (_internalTimer <= 0 || GameManager.Instance.IsGameFinished) return;
            
            DecreaseTimer();

            if (!_isAlarmActivated)
            {
                if (_minutes * 60 + _seconds <= _alarmTrigger)
                {
                    _isAlarmActivated = true;
                    _lightManager.ActivateAlarm();
                } 
                else if (_lightManager.IsLightsOn && _minutes * 60 + _seconds <= _lightEventTrigger)
                {
                    _lightManager.SwitchLight();
                }
            }
            else if (_minutes <= 0 && _seconds <= 0)
            {
                GameManager.Instance.FinishTheGame(false);
            }
        }

        private void DecreaseTimer()
        {
            _internalTimer -= Time.deltaTime;
            _seconds = Mathf.CeilToInt(_internalTimer);
            _minutes = _seconds / 60;
            _seconds = _seconds % 60;
            
            _countDownText.text = _minutes.ToString("00") + " : " + _seconds.ToString("00");
        }
    }
}
