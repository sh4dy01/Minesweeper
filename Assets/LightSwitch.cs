using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] private bool _isAlarm;
    [SerializeField] private LightManager _lightManager;
    
    private void OnMouseDown()
    {
        if (_isAlarm)
            _lightManager.SwitchAlarm();
        else
            _lightManager.SwitchLight();
    }
}
