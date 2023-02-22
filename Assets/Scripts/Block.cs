using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private bool _isBomb;
    [SerializeField] private int _bombAroundCounter;
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Sprite _bombSprite;
    [SerializeField] private GameObject _flag;
    [SerializeField] private Texture2D _screwdriverCursor;
    [SerializeField] private Sprite[] _bombCounterSprites = new Sprite[8];
    
    private SpriteRenderer _spriteRenderer;

    public Vector3 Position { get; set; }

    public bool IsBomb => _isBomb;
    public void SetBomb(bool value) => _isBomb = value;
    public void SetBombAroundCounter(int value) => _bombAroundCounter = value;

    // Start is called before the first frame update
    private void Awake()
    {
        _bombAroundCounter = 0;
        _isBomb = false;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        Cursor.SetCursor(_screwdriverCursor, Vector2.zero, CursorMode.ForceSoftware);
        
        //Left click to open block
        if (Input.GetMouseButtonDown(0) && !_flag.activeSelf)
        {
            Sprite which = _bombSprite;
            
            if (!IsBomb)
            {
                which = _bombAroundCounter == 0 ? _emptySprite : _bombCounterSprites[_bombAroundCounter - 1];
            }
            else
            {
                //Loose
                //Explode
            }
            
            _spriteRenderer.sprite = which;
            GetComponent<Collider2D>().enabled = false;
        } 
        //Right click to flag a block
        else if (Input.GetMouseButtonDown(1))
        {
            if (!_flag.activeSelf && GameManager.Instance.BombCounter <= 0) return;

            _flag.SetActive(!_flag.activeSelf);
            switch (_flag.activeSelf)
            {
                case true:
                    if (GameManager.Instance.BombCounter <= 0) return;
                    GameManager.Instance.DecreaseBombCounter();
                    break;
                case false:
                    GameManager.Instance.IncreaseBombCounter();
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
