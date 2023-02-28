using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Managers
{
    public class LightManager : MonoBehaviour
    {
        [SerializeField] private List<Light2D> _bgLights;
        [SerializeField] private Light2D _gridLight;
        [SerializeField] private Light2D _mouseLight;
        [SerializeField] private float _bgLightsOffIntensity;
        [SerializeField] private float _gridLightsOffIntensity;

        [Header("Alarm")] 
        [SerializeField] private Color _alarmColor;
    
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
            _isLightsOn = !_isLightsOn;
            if (_isLightsOn)
            {
                TurnOnLights();
            }
            else
            {
                TurnOffLights();
            }
        }

        private void TurnOffLights()
        {
            foreach (var light2D in _bgLights)
            {
                light2D.enabled = false;
            }
        
            _gridLight.intensity = _gridLightsOffIntensity;
            _mouseLight.enabled = true;
        }

        private void TurnOnLights()
        {
            foreach (var light2D in _bgLights)
            {
                light2D.enabled = true;
            }

            _gridLight.intensity = _gridLightOnIntensity;
            _mouseLight.enabled = false;
        }

        public void SwitchAlarm()
        {
            _isAlarm = !_isAlarm;

            if (_isAlarm)
                ActivateAlarm();
            else
                DisableAlarm();
        }
    
        private void ActivateAlarm()
        {
            _gridLight.color = _alarmColor;
        
            foreach (var bgLight in _bgLights)
            {
                bgLight.color = _alarmColor;
            }
        }
    
        private void DisableAlarm()
        {
            _gridLight.color = _gridLightColor;

            for (int i = 0; i < _bgLightsColor.Count; i++)
            {
                _bgLights[i].color = _bgLightsColor[i];
            }
        }
    }
}
