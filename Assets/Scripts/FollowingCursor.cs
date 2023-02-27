using UnityEngine;

public class FollowingCursor : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        gameObject.transform.position = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
