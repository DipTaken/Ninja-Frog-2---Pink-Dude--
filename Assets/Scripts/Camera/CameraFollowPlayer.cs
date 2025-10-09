using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private GameObject player1, player2;
    private Vector3 player1Position, player2Position;
    private bool isTransitioning = false;
    public bool canMove = true;
    private float lerpSpeed = 5f;
    private float exLerpSpeed = 4f;
    private float baseCameraSize;
    // Start is called before the first frame update
    void Start()
    {
        //finding players and setting default values
        player1 = GameObject.Find("Player 1");
        player2 = GameObject.Find("Player 2");
        baseCameraSize = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //update player positions
        player1Position = player1.transform.position + new Vector3(0, 0, -10);
        player2Position = player2.transform.position + new Vector3(0, 0, -10);

        if (!isTransitioning && canMove)
        {
            FollowPlayer();
        }
        else if (canMove)
        {
            SmoothSwitchPlayerTransition();
        }

        if (Input.GetKeyDown("left shift") && !GameManagerUI.instance.isPaused)
        {
            SwitchPlayer();
        }
    }

    // Follow player positions based on which player is active
    public void FollowPlayer()
    {
        if (player1.GetComponent<PlayerMovement>().isActive)
        {
            transform.position = player1Position;
        }
        else
        {
            transform.position = player2Position;
        }
    }

    // Switch active player and smoothly transition camera position
    void SwitchPlayer()
    {
        if (player1.GetComponent<PlayerMovement>().isActive)
        {
            player1.GetComponent<PlayerMovement>().isActive = false;
            player2.GetComponent<PlayerMovement>().isActive = true;
        }
        else
        {
            player1.GetComponent<PlayerMovement>().isActive = true;
            player2.GetComponent<PlayerMovement>().isActive = false;
        }

        if (canMove) //prevent jitter when zoomed out
        {
            SmoothSwitchPlayerTransition();
        }
    }

    void SmoothSwitchPlayerTransition()
    {
        //start transition
        isTransitioning = true;
        exLerpSpeed = 0;

        //Calculate target position based on active player
        Vector3 targetPosition = player1.GetComponent<PlayerMovement>().isActive ? player1Position : player2Position;
        float distanceDelta = Mathf.Abs((transform.position - targetPosition).magnitude); //calculate distance from camera to target player

        //Move camera towards target position smoothly (lerping)
        if (distanceDelta > 1E-2f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * (lerpSpeed + exLerpSpeed));
        }
        else //reset valiues when close enough to target position
        {
            transform.position = targetPosition;
            exLerpSpeed = 0;
            isTransitioning = false;
        }

        //Make transition speed increase over time and make it end faster
        if (distanceDelta > 1f)
        {
            exLerpSpeed += Time.deltaTime * 2f;
        }
        else if (distanceDelta > 1E-2f)
        {
            exLerpSpeed += Time.deltaTime * Mathf.Pow(exLerpSpeed, 2);
        }
    }
}
