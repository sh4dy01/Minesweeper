using Managers;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] private bool _isAlarm;
    [SerializeField] private LightManager _lightManager;
    
    private void OnMouseDown()
    {
        if (_isAlarm)
            _lightManager.DisableAlarm();
        else
            _lightManager.SwitchLight();
    }
}
