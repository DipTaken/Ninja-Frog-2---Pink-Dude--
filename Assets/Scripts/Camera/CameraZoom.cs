using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    CameraFollowPlayer cameraFollowPlayer;
    [SerializeField]
    private float minZoom, maxZoom;
    private float zoomStep = 0.5f;
    private float zoomSpeed = 4f;
    private float exZoomSpeed = 2f;
    private float baseCameraSize;
    public float currentZoom;
    public bool canZoom = true;
    void Start()
    {
        // Initialize default values
        currentZoom = Camera.main.orthographicSize;
        baseCameraSize = currentZoom;
        cameraFollowPlayer = GetComponent<CameraFollowPlayer>();
    }

    void LateUpdate()
    {
        if (canZoom)
        {
            float scroll = -Input.mouseScrollDelta.y;
            currentZoom += scroll * zoomStep; // adjust zoom via mouse scroll
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom); // make sure zoom is within bounds
        }
    }

    void FixedUpdate()
    {
        float zoomDelta = Mathf.Abs(currentZoom - Camera.main.orthographicSize); // difference between current zoom and camera's orthographic zoom
        if (zoomDelta > 1E-4f && canZoom)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, currentZoom, Time.deltaTime * (zoomSpeed + exZoomSpeed)); // smoothly transition to the new zoom level
            exZoomSpeed += Time.deltaTime * 0.1f; // gradually increase the extra zoom speed
        }
        else
        {
            exZoomSpeed = 0;
            Camera.main.orthographicSize = currentZoom; // Ensure it snaps to the exact value
        }
    }
}