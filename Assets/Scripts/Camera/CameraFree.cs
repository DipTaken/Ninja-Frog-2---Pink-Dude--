using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFree : MonoBehaviour
{
    CameraFollowPlayer cameraFollowPlayer;
    CameraZoom cameraZoom;
    private GameObject hudUI;
    private Vector3 mapCenter, mousePosition;
    [SerializeField]
    private float freeZoomSize = 9f;
    [SerializeField]
    private Vector2 offset;

    void Start()
    {
        // Initialize components and variables
        cameraFollowPlayer = GetComponent<CameraFollowPlayer>();
        cameraZoom = GetComponent<CameraZoom>();
        hudUI = GameObject.Find("HUD Canvas");

        mapCenter = new Vector3(offset.x, offset.y, transform.position.z);
        mousePosition = Vector3.zero;
    }
    void LateUpdate()
    {
        if (GameManager.instance.isGameOver || GameManagerUI.instance.isPaused) //unable to zoom out when paused or game over
        {
            ResetValues();
            return;
        }

        if (Input.GetMouseButton(1)) //RMB press down
        {
            ZoomOut();
        }
        if (Input.GetMouseButtonUp(1)) //RMB release
        {
            ResetValues();
        }
    }

    void ZoomOut()
    {
        cameraFollowPlayer.canMove = false; // prevent camera from following player
        cameraZoom.canZoom = false; // prevent zooming in/out
        hudUI.SetActive(false); // hide HUD
        transform.position = mapCenter; // center camera on map
        Camera.main.orthographicSize = freeZoomSize; // set camera zoom level
    }

    void ResetValues()
    {
        cameraFollowPlayer.canMove = true; // allow camera to follow player
        cameraZoom.canZoom = true; // allow zooming in/out
        hudUI.SetActive(true); // show HUD
        Camera.main.orthographicSize = cameraZoom.currentZoom; // reset camera zoom to current zoom level
        cameraFollowPlayer.FollowPlayer(); // return camera to following player
        mousePosition = Vector3.zero; // reset mouse position
    }
}