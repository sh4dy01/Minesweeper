using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] float minSize = 5;
    [SerializeField] float maxSize = 3;
    [SerializeField] float zoomSensitivity = 0.1f;
    
    [Header("Movement")]
    [SerializeField] float moveSensitivity = 0.5f;
    
    private Vector2 _xMinMax;
    private Vector2 _yMinMax;
    
    private Camera _camera;
    private float _zoomScale;
    
    private void Awake()
    {
        _camera = Camera.main;
        _zoomScale = 1;
    }

    private void Update ()
    {
        if (Input.GetAxis("Mouse ScrollWheel") == 0f) return;
        
        if (_camera.orthographicSize <= minSize && Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Zoom();
        }
        else if (_camera.orthographicSize >= maxSize && Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Zoom();
        }
    }

    private void Zoom()
    {
        float size = _camera.orthographicSize;
        size += Input.GetAxis("Mouse ScrollWheel") * -zoomSensitivity;
        size = Mathf.Clamp(size, maxSize, minSize);
        _camera.orthographicSize = size;
        
        _zoomScale += Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
    }
    
    private void LateUpdate()
    {
        if (!Input.GetMouseButton(2)) return;
        
        transform.position += new Vector3(
            Input.GetAxisRaw("Mouse X") * Time.deltaTime * moveSensitivity * _zoomScale, 
            Input.GetAxisRaw("Mouse Y") * Time.deltaTime * moveSensitivity * _zoomScale, 
            0f);

        Vector3 camPos = transform.position;
        float yBounds = minSize - _camera.orthographicSize;
        float xBounds = yBounds * _camera.aspect;
        if (camPos.y > yBounds) camPos.y = yBounds;
        if (camPos.y < -yBounds) camPos.y = -yBounds;
        if (camPos.x > xBounds) camPos.x = xBounds;
        if (camPos.x < -xBounds) camPos.x = -xBounds;
        transform.position = camPos;
    }
}
