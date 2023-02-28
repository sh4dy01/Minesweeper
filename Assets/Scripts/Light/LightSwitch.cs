using Managers;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] private LightManager _lightManager;

    private void OnMouseDown() => _lightManager.SwitchLight();
}
