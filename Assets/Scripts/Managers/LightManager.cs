using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light2D[] _bgLights;
    [SerializeField] private Light2D _gridLight;
    [SerializeField] private float _bgLightsOffIntensity = 0.2f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TurnOffLights();
        }
    }

    public void TurnOffLights()
    {
        _gridLight.intensity = 0.2f;
    }
}
