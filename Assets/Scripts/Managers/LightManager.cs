using System.Collections;
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
        [SerializeField] private GameObject _alarmButton;
        [SerializeField] private GameObject _lightSwitch;

        [Header("Alarm")] 
        [SerializeField] private Color _alarmColor;
        [SerializeField] private float _activateAlarmDelay;
    
        private bool _isLightsOn;
        private bool _isAlarm;
        private List<float> _bgLightsOnIntensity;
        private List<Color> _bgLightsColor;
        private float _gridLightOnIntensity;
        private Color _gridLightColor;

        private void Awake()
        {
            _isAlarm = false;
            _isLightsOn = true;
            _bgLightsOnIntensity = new List<float>();
            _bgLightsColor = new List<Color>();
            _gridLightOnIntensity = _gridLight.intensity;
            _gridLightColor = _gridLight.color;
        
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
            
            switch (_isLightsOn)
            {
                case true:
                    TurnOnLights();
                    break;
                default:
                    TurnOffLights();
                    break;
            }
        }

        private void TurnOffLights()
        {
            foreach (var light2D in _bgLights)
            {
                LightOff(light2D);
            }
        
            SetLightIntensity(_gridLight, _gridLightsOffIntensity);
            LightOn(_mouseLight);
            LightOff(_gridLight);
        }

        private void TurnOnLights()
        {
            foreach (var light2D in _bgLights)
            {
                LightOn(light2D);
            }

            SetLightIntensity(_gridLight, _gridLightOnIntensity);
            LightOn(_gridLight);
            LightOff(_mouseLight);
        }

        public void ActivateAlarm()
        {
            Color color = _alarmColor;
            color.r = 0.01f;
            
            LightOn(_mouseLight);
            SetLightColor(_gridLight, color);
            
            foreach (var bgLight in _bgLights)
            {
                SetLightColor(bgLight, _alarmColor);
            }
            
            HideButton(_lightSwitch);

            StartCoroutine(ShowAlarmButtonAfterSeconds());
        }
    
        public void DisableAlarm()
        {
            SetLightColor(_gridLight, _gridLightColor);

            for (int i = 0; i < _bgLightsColor.Count; i++)
            {
                SetLightColor(_bgLights[i], _bgLightsColor[i]);
            }

            HideButton(_alarmButton);
        }

        private IEnumerator ShowAlarmButtonAfterSeconds()
        {
            yield return new WaitForSeconds(_activateAlarmDelay);

            ShowButton(_alarmButton);
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
