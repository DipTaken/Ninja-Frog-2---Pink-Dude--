using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerChecks : MonoBehaviour
{
    public LayerMask ignoredLayers;
    Collider2D playerCollider;
    Rigidbody2D rb;
    [SerializeField]
    private float groundRayDist;
    [SerializeField]
    private float sideRayDist = 0.35f;
    public Vector2 groundVelocity;
    public bool hasFinished;
    private bool playedLandSound;

    void Start()
    {
        //default values and getting components
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        groundRayDist = playerCollider.bounds.size.x - 0.2f;
    }

    void LateUpdate()
    {
        //for debugging purposes
        isGrounded();
        TrueGroundDistance();
    }

    public bool isGrounded()
    {
        Vector2 rayOrigin = new Vector2(playerCollider.bounds.min.x + 0.1f, playerCollider.bounds.min.y - 0.021f); //send a ray from the bottom right of the player collider

        RaycastHit2D groundRay = Physics2D.Raycast(rayOrigin, Vector2.right, groundRayDist, ~ignoredLayers); //cast a ray from right to left

        float velocityDelta = Mathf.Abs(rb.velocity.y - groundVelocity.y);

        if (groundRay && velocityDelta < 1E-3f) //if the ray hit something and the vertical velocity is close to zero (on ground)
        {
            Debug.DrawRay(rayOrigin, Vector2.right * groundRayDist, Color.green);
            if (!playedLandSound)
            {
                SoundManager.instance.land.Play(); //play landing sound if not already played
                playedLandSound = true;
            }
        }
        else // if not on ground
        {
            Debug.DrawRay(rayOrigin, Vector2.right * groundRayDist, Color.red);
            groundVelocity = Vector2.zero; // reset ground velocity
            playedLandSound = false;
        }

        if (groundRay) // if the ray hit something
        {
            GetGroundVelocity(groundRay.collider);
            HasFinished(groundRay.collider);
        }

        return groundRay && velocityDelta < 1E-3f;
    }

    public bool WallDetected()
    {
        bool onGround = isGrounded();
        Vector2 rayOrigin = new Vector2(playerCollider.bounds.center.x, playerCollider.bounds.min.y); // send a ray from the bottom center of the player collider

        RaycastHit2D rightRay = Physics2D.Raycast(rayOrigin, Vector2.right, sideRayDist, 64); //extend the ray to the right
        RaycastHit2D leftRay = Physics2D.Raycast(rayOrigin, Vector2.left, sideRayDist, 64); //extend the ray to the left


        // debug
        if (rightRay && !onGround)
        {
            Debug.DrawRay(rayOrigin, Vector2.right * new Vector2(sideRayDist, 0), Color.green);
        }
        else
        {
            Debug.DrawRay(rayOrigin, Vector2.right * new Vector2(sideRayDist, 0), Color.red);
        }

        if (leftRay && !onGround)
        {
            Debug.DrawRay(rayOrigin, Vector2.left * new Vector2(sideRayDist, 0), Color.green);
        }
        else
        {
            Debug.DrawRay(rayOrigin, Vector2.left * new Vector2(sideRayDist, 0), Color.red);
        }

        float playerDirection = Mathf.Clamp(transform.localScale.x, -1f, 1f); //used to check if the player is facing the wall
        return (rightRay || leftRay) && !onGround && Input.GetAxisRaw("Horizontal") == playerDirection;
    }

    private float GroundRayDistance(Vector2 rayOrigin)
    {
        RaycastHit2D groundRay = Physics2D.Raycast(rayOrigin, Vector2.down, float.MaxValue, ~ignoredLayers); // cast a ray downwards from the specified origin
        float distance;

        if (groundRay.collider.GetComponent<TilemapCollider2D>() != null) // Check if the collider is a TilemapCollider2D
        {
            GridLayout gridLayout = groundRay.collider.transform.parent.GetComponentInParent<GridLayout>(); // Get the GridLayout component from the parent of the collider
            Vector3 tilePosition = gridLayout.WorldToLocal(groundRay.point); // Get the tile position

            distance = rayOrigin.y - tilePosition.y; // calculate delta from ray origin to the tile position
        }
        else //if the collider is not a TilemapCollider2D        
        {
            distance = rayOrigin.y - groundRay.point.y; // calculate delta from ray origin to the hit point
        }

        Debug.DrawRay(rayOrigin, Vector2.down * distance, Color.blue); // draw the ray for debugging
        return distance;
    }

    public float TrueGroundDistance()
    {
        Vector2 leftBound = new Vector2(playerCollider.bounds.min.x + 0.14f, playerCollider.bounds.min.y - 0.0211f); // left bound of the player collider
        Vector2 rightBound = new Vector2(playerCollider.bounds.max.x - 0.15f, playerCollider.bounds.min.y - 0.0211f); // right bound of the player collider
        float leftDist = GroundRayDistance(leftBound); // distance from the left bound to the ground
        float rightDist = GroundRayDistance(rightBound); // distance from the right bound to the ground

        return Mathf.Min(leftDist, rightDist); // return the shorter ray distance
    }

    void GetGroundVelocity(Collider2D ground)
    {
        ground.TryGetComponent(out Rigidbody2D groundRb); // try to get the Rigidbody2D component from the ground collider
        groundVelocity = groundRb != null ? groundRb.velocity : Vector2.zero; 
    }

    bool HasFinished(Collider2D other)
    {
        // checks if the player has finished the game by stepping on the win plate
        if (this.gameObject.tag == "Player1")
        {
            hasFinished = other.gameObject.tag == "Win1";
        }
        else if (this.gameObject.tag == "Player2")
        {
            hasFinished = other.gameObject.tag == "Win2";
        }
        return hasFinished;
    }
}