using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 minConstraint, maxConstraint;
    [SerializeField]
    private float rotationSpeed;
    public float xSpeed, ySpeed;
    [SerializeField]
    private bool switchX, switchY;
    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<Rigidbody2D>() == null) // Check if Rigidbody2D component exists, if not, add it
        {
            this.gameObject.AddComponent<Rigidbody2D>();
        }
        rb = GetComponent<Rigidbody2D>(); 

        // Set the Rigidbody2D properties   
        rb.velocity = new Vector2(xSpeed, ySpeed);
        rb.bodyType = RigidbodyType2D.Kinematic;

        switchX = xSpeed > 0 ? true : false;
        switchY = ySpeed > 0 ? true : false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {       
        XMovement(); 
        YMovement();

        if (rotationSpeed != 0) //This reduces jitter when rotationSpeed is 0 by reducing calls to transform (it messes with the physics?)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    //below are the methods for movement
    // XMovement and YMovement are used to move the object within min and max constraints
    // switchX and switchY allow for proper directional movement
    // object will switch direction when it reaches the min or max constraint

    void XMovement()
    {
        if (!switchX && rb.position.x < minConstraint.x)
        {
            switchX = true;
            xSpeed = -xSpeed;
        }
        else if (switchX && rb.position.x > maxConstraint.x)
        {
            switchX = false;
            xSpeed = -xSpeed;
        }
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);   
    }

    void YMovement()
    {
        if (!switchY && rb.position.y < minConstraint.y)
        {
            switchY = true;
            ySpeed = -ySpeed;
        }
        else if (switchY && rb.position.y > maxConstraint.y)
        {
            switchY = false;
            ySpeed = -ySpeed;
        }
        rb.velocity = new Vector2(rb.velocity.x, ySpeed);
    }
}
