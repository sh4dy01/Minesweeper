using Managers;
using ScriptableObjects.script;
using TMPro;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _errorText;

    private int? _customWidth;
    private int? _customHeight;
    private int? _customBombQuantity;
    private string _errorMessage;
    
    public void SetDifficulty(GameDifficultySo _gameDifficulty)
    {
        GameManager.Instance.SetDifficulty(_gameDifficulty);
        Debug.Log("Set diff");

        if (!_gameDifficulty.IsCustom)
            SceneLoader.LoadGameScene();
    }

    public void SetCustomWidth(string width)
    {
        _customWidth = int.Parse(width);
    }
        
    public void SetCustomHeight(string height)
    {
        _customHeight = int.Parse(height);
    }
        
    public void SetCustomBombQuantity(string bombQuantity)
    {
        _customBombQuantity = int.Parse(bombQuantity);
    }

    public void ApplyCustomValues()
    {
        Debug.Log("Applied values");
        GameDifficultySo difficultySo = GameManager.Instance.GameDifficulty;
        
        if (_customHeight == null || _customWidth == null || _customBombQuantity == null || _customHeight < 1 || _customWidth < 1 ||_customBombQuantity < 1 || _customHeight > 50 || _customWidth > 50)
        {
            _errorMessage = "One of the values is incorrect";
            PrintErrorMessage();
            return;
        }

        if (_customBombQuantity > (_customHeight * _customWidth) / 3)
        {
            _errorMessage = "There are too many bombs";
            PrintErrorMessage();
            return;
        }
        
        difficultySo.SetHeight((int)_customHeight);
        difficultySo.SetWidth((int)_customWidth);
        difficultySo.SetBombQuantity((int)_customBombQuantity);
        SceneLoader.LoadGameScene();
    }
    
    private void PrintErrorMessage()
    {
        _errorText.text = _errorMessage;
    }
}
