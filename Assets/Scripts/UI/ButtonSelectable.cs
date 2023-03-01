using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectable : MonoBehaviour
{
    [SerializeField] private Image _borderRight;
    [SerializeField] private Image _borderLeft;
    
    private Button _button;

    private void Awake()
    {
        _borderLeft.enabled = false;
        _borderRight.enabled = false;
        _button = GetComponent<Button>();
    }
    
    public void HideBorders()
    {
        _borderRight.enabled = false;
        _borderLeft.enabled = false;
    }
    
    public void ShowBorders()
    {
        _borderRight.enabled = true;
        _borderLeft.enabled = true;
    }
}
