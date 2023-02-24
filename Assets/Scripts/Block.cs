using Managers;
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
    [SerializeField] private float _radius = 10.0F;
    [SerializeField] private float _power = 10.0F;
    private Collider2D[] _colliders;

    private SpriteRenderer _spriteRenderer;

    public GameGrid.BlockInfo BlockInfo { get; set; }
    public bool Revealed { get; private set; }
	public bool Flagged => _flag.activeSelf;
    public void SetBomb(bool value) => _isBomb = value;
    public void SetBombAroundCounter(int value) => _bombAroundCounter = value;

    // Start is called before the first frame update
    private void Awake()
    {
        Revealed = false;
        _bombAroundCounter = 0;
        _isBomb = false;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.IsFinished) return;
        
        Cursor.SetCursor(_screwdriverCursor, Vector2.zero, CursorMode.ForceSoftware);
        
        //Left click to open block
        if (Input.GetMouseButtonDown(0) && !_flag.activeSelf)
        {
            if (!Revealed)
                GameManager.Instance.GameGrid.RevealBlock(BlockInfo);
            else if (BlockInfo.BombCounter != 0)
                GameManager.Instance.GameGrid.RevealAround(BlockInfo);
        }
        //Right click to flag a block
        else if (Input.GetMouseButtonDown(1))
        {
            if (!_flag.activeSelf && (GameManager.Instance.BombCounter <= 0 || Revealed)) return;

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

    public void RevealThisBlock()
    {
        // This block is flagged.
        if (_flag.activeSelf) return;

        Revealed = true;

		Sprite which = _bombSprite;

		if (!_isBomb)
		{
			which = _bombAroundCounter == 0 ? _emptySprite : _bombCounterSprites[_bombAroundCounter - 1];
		}
		else
		{
			GameManager.Instance.DecreaseBombCounter();
        }

        _spriteRenderer.sprite = which;
	}

    public void Explosion()
    {
        _colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
        foreach (Collider2D hit in _colliders)
        {
            Rigidbody2D rb = hit.gameObject.GetComponent<Rigidbody2D>();
            if (hit.gameObject.CompareTag("Block"))
            {
                Block b = hit.gameObject.GetComponent<Block>();
                if (!b.Revealed) rb.bodyType = RigidbodyType2D.Dynamic;
            }

            if (rb == null) continue;
            
            Vector2 distanceToVector = hit.transform.position - transform.position;
            
            if (!(distanceToVector.magnitude > 0)) continue;
            
            float explosionForce = _power*50 / distanceToVector.magnitude;
            rb.AddForce(distanceToVector.normalized * explosionForce);
        }
    }
}
