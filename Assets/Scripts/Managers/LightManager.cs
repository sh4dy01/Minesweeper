using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Managers
{
    public class LightManager : MonoBehaviour
    {
        [Header("Lights")]
        [SerializeField] private List<Light2D> _bgLights;
        [SerializeField] private Light2D _gridLight;
        [SerializeField] private Light2D _mouseLight;
        [SerializeField] private float _bgLightsOffIntensity;
        [SerializeField] private float _gridLightsOffIntensity;
        
        [Header("Buttons")]
        [SerializeField] private GameObject _lightSwitch;

        [Header("Alarm")] 
        [SerializeField] private Color _alarmColor;
        
        [Header("Sound")]
        [SerializeField] private AudioClip _switchOnSound;
        [SerializeField] private AudioClip _switchOffSound;
        
        private bool _isLightsOn;
        private bool _isAlarm;
        private List<float> _bgLightsOnIntensity;
        private List<Color> _bgLightsColor;
        private float _gridLightOnIntensity;
        private Color _gridLightColor;
        private AudioSource _audioSource;
        
        public bool IsLightsOn => _isLightsOn;
        public bool IsAlarmActivated => _isAlarm;

        public event Action OnSwitchLightsOn;

        private void Awake()
        {
            _isAlarm = false;
            _isLightsOn = true;
            
            _bgLightsOnIntensity = new List<float>();
            _bgLightsColor = new List<Color>();
            
            _gridLightOnIntensity = _gridLight.intensity;
            _gridLightColor = _gridLight.color;

            _audioSource = GetComponent<AudioSource>();
        
            foreach (var bgLight in _bgLights)
            {
                _bgLightsOnIntensity.Add(bgLight.intensity);
                _bgLightsColor.Add(bgLight.color);
            }
        }

        public void SwitchLight()
        {
            if (_isAlarm) return;

            _isLightsOn = !_isLightsOn;
            
            if (_isLightsOn)
                TurnOnLights();
            else
                TurnOffLights();
        }

        private void TurnOffLights()
        {
            ChangeSoundAndPlay(_switchOffSound);

            foreach (var light2D in _bgLights)
            {
                LightOff(light2D);
            }
        
            SetLightIntensity(_gridLight, _gridLightsOffIntensity);
            LightOn(_mouseLight);
            LightOff(_gridLight);
            ShowButton(_lightSwitch);
        }

        private void TurnOnLights()
        {
            OnSwitchLightsOn?.Invoke();
            
            ChangeSoundAndPlay(_switchOnSound);
            
            foreach (var light2D in _bgLights)
            {
                LightOn(light2D);
            }

            SetLightIntensity(_gridLight, _gridLightOnIntensity);
            LightOn(_gridLight);
            LightOff(_mouseLight);
            HideButton(_lightSwitch);
        }

        public void ActivateAlarm()
        {
            _isAlarm = true;

            MusicManager.Instance.SetAlarmMusic();
            
            Color color = _alarmColor;
            color.r = 0.05f;
            
            LightOn(_mouseLight);
            SetLightColor(_gridLight, color);
            
            foreach (var bgLight in _bgLights)
            {
                SetLightColor(bgLight, _alarmColor);
            }
            
            HideButton(_lightSwitch);
        }

        private void ChangeSoundAndPlay(AudioClip sound)
        {
            _audioSource.clip = sound;
            _audioSource.Play();
        }

        /// -------- UTILITY -------- ///
        private static void SetLightColor(Light2D light, Color color) => light.color = color;
        private static void SetLightIntensity(Light2D light, float intensity) => light.intensity = intensity;
        private static void LightOff(Light2D light) => light.enabled = false;
        private static void LightOn(Light2D light) => light.enabled = true;
        private static void HideButton(GameObject button) => button.SetActive(false);
        private static void ShowButton(GameObject button) => button.SetActive(true);
    }
}
