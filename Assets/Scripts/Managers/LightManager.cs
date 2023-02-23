using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    private bool _lightsOn = true;
    [SerializeField] private Light2D[] _bgLights;
    [SerializeField] private Light2D _gridLight;
    [SerializeField] private float _bgLightsOffIntensity = 0.2f;
    [SerializeField] private float _gridLightsOffIntensity = 0.2f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TurnOffLights();
        }
    }

    private void SwitchLight()
    {
        _lightsOn = !_lightsOn;
        if (_lightsOn)
        {
            //TurnOnLights();
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
            light2D.intensity = _bgLightsOffIntensity;
        }
        
        _gridLight.intensity = _gridLightsOffIntensity;
    }
}
