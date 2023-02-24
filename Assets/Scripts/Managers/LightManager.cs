using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    private bool _lightsOn = true;
    [SerializeField] private List<Light2D> _bgLights;
    [SerializeField] private Light2D _gridLight;
    [SerializeField] private Light2D _mouseLight;
    [SerializeField] private float _bgLightsOffIntensity;
    [SerializeField] private float _gridLightsOffIntensity;
    
    private List<float> _bgLightsOnIntensity;
    private float _gridLightOnIntensity;

    private void Awake()
    {
        _bgLightsOnIntensity = new List<float>();
        _gridLightOnIntensity = _gridLight.intensity;
        
        foreach (var bgLight in _bgLights)
        {
            _bgLightsOnIntensity.Add(bgLight.intensity);
        }
    }

    public void SwitchLight()
    {
        _lightsOn = !_lightsOn;
        if (_lightsOn)
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
}
