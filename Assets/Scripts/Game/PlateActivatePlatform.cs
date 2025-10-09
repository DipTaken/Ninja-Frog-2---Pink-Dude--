using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateActivatePlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject platformToActivate;
    MovingObject moveScript;
    private Vector2 initalSpeed;
    private bool player1On = false, player2On = false;
    // Start is called before the first frame update
    void Start()
    {
        //grab platform details and set initial speed to 0
        moveScript = platformToActivate.GetComponent<MovingObject>();
        initalSpeed = new Vector2(moveScript.xSpeed, moveScript.ySpeed);
        moveScript.xSpeed = 0;
        moveScript.ySpeed = 0;
    }

    // Move the platform when a player steps on the plate
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Player1" && !player2On) || (other.gameObject.tag == "Player2" && !player1On))
        {
            moveScript.xSpeed = initalSpeed.x;
            moveScript.ySpeed = initalSpeed.y;
        }
        
        //handles cases when both players are on the plate
        if (other.gameObject.tag == "Player1")
        {
            player1On = true;
        }
        else if (other.gameObject.tag == "Player2")
        {
            player2On = true;
        }
    }


    // Stop the platform when a player exits the plate
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            player1On = false;
        }
        else if (other.gameObject.tag == "Player2")
        {
            player2On = false;
        }

        if ((other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2") && !player1On && !player2On)
        {
            initalSpeed = new Vector2(moveScript.xSpeed, moveScript.ySpeed); //store speed (because it can now be negative)
            moveScript.xSpeed = 0;
            moveScript.ySpeed = 0;
        }
    }
}