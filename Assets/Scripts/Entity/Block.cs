using Managers;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Prefab properties.
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Sprite _bombSprite;
    [SerializeField] private GameObject _flag;
    [SerializeField] private Texture2D _cursorReveal;
    [SerializeField] private Sprite[] _bombCounterSprites = new Sprite[8];
    [SerializeField] private float _radius = 10.0F;
    [SerializeField] private float _power = 10.0F;

    private Collider2D[] _colliders;
    private SpriteRenderer _spriteRenderer;

    // Reference to block's data in the game's logic.
    public GameGrid.BlockInfo BlockInfo { get; set; }

    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.IsGameFinished) return;
        
        Cursor.SetCursor(_cursorReveal, Vector2.zero, CursorMode.ForceSoftware);
        if (!BlockInfo.Revealed || BlockInfo.NumBombsAround > 0)
        {
            _spriteRenderer.color = new Color(0.7F, 0.7F, 0.7F);
        }
        
        // Left click to open block
        if (Input.GetMouseButtonDown(0) && !_flag.activeSelf)
        {
            if (!BlockInfo.Revealed)
                GameManager.Instance.GameGrid.RevealBlock(BlockInfo);
            else if (BlockInfo.NumBombsAround != 0)
                GameManager.Instance.GameGrid.RevealAround(BlockInfo);
        }
        // Right click to flag a block
        else if (Input.GetMouseButtonDown(1))
        {
            if (!_flag.activeSelf && (GameManager.Instance.BombCounter <= 0 || BlockInfo.Revealed)) return;

            // Update flag status.
            _flag.SetActive(!_flag.activeSelf);
            BlockInfo.Flagged = _flag.activeSelf;

			if (_flag.activeSelf)
            {
                if (GameManager.Instance.BombCounter <= 0) return;
                GameManager.Instance.DecreaseBombCounter();
            }
            else
            {
                GameManager.Instance.IncreaseBombCounter();
            }
        }
    }

    private void OnMouseExit()
    {
		_spriteRenderer.color = new Color(1, 1, 1);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void RevealThisBlock()
    {
        // This block is flagged.
        if (_flag.activeSelf) return;

		BlockInfo.Revealed = true;

		Sprite which = _bombSprite;

		if (!BlockInfo.IsBomb)
		{
			which = BlockInfo.NumBombsAround == 0 ? _emptySprite : _bombCounterSprites[BlockInfo.NumBombsAround - 1];
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
                if (!b.BlockInfo.Revealed) rb.bodyType = RigidbodyType2D.Dynamic;
            }

            if (rb == null) continue;
            
            Vector2 distanceToVector = hit.transform.position - transform.position;
            
            if (!(distanceToVector.magnitude > 0)) continue;
            
            float explosionForce = _power*50 / distanceToVector.magnitude;
            rb.AddForce(distanceToVector.normalized * explosionForce);
        }
    }
}
