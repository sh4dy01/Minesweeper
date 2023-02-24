using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] private LightManager _lightManager;

    private void OnMouseDown()
    {
        _lightManager.SwitchLight();
    }
}
