using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    PlayerChecks playerChecks;
    public float timeSinceHit, hitCooldown;
    private bool playedDeathSound = false;
    private Vector3 respawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        //grab components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerChecks = GetComponent<PlayerChecks>();
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceHit += Time.deltaTime;
    }

    public void Death()
    {
        if (!playedDeathSound)
        {
            // death sequence
            Debug.Log("Player has died.");
            animator.SetTrigger("Death");
            rb.constraints |= RigidbodyConstraints2D.FreezePositionX; //bitwise operations to "add" the constraints, or else only one can be active at a time
            rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
            SoundManager.instance.death.Play();
            playedDeathSound = true;
        }
    }

    public void Hit()
    {
        Debug.Log("Player has been hit.");
        animator.SetTrigger("Hit");
        SoundManager.instance.hit.Play();

        //launch the player when hit
        Vector2 launchDirection = new Vector2(transform.localScale.x * 3, 5);
        rb.velocity = launchDirection;
        timeSinceHit = 0f; //reset the hit cooldown timer
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Spike") //spike hit
        {
            GameManager.instance.UpdateLives(-1, this.gameObject);
        }
        if (other.gameObject.tag == "SpikeRespawn") //spike hit, but also respawn
        {
            GameManager.instance.UpdateLives(-1, this.gameObject);
            StartCoroutine(Respawn());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spike") //spike hit
        {
            GameManager.instance.UpdateLives(-1, this.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Respawn" && playerChecks.TrueGroundDistance() < 0.1f) //if the player is on the ground and in a respawn area
        {
            respawnPosition = new Vector3(other.transform.position.x, transform.position.y, transform.position.z); // set respawn pos
            //Debug.Log("Respawn position set: " + respawnPosition);
        }
    }
    
    IEnumerator Respawn()
    {
        void DisableAnimations()
        {
            animator.SetBool("Running", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("Sliding", false);
            animator.SetBool("DoubleJump", false);
        }

        if (GameManager.instance.lives > 0)
        {
            //disable moving and collisions
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<PlayerMovement>().enabled = false;

            yield return new WaitForSeconds(0.55f);
            //reset player position and velocity
            DisableAnimations();
            transform.position = respawnPosition;
            rb.velocity = Vector2.zero;
            SoundManager.instance.respawn.Play();

            yield return new WaitForSeconds(1f);
            //re-enable moving and collisions
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<Collider2D>().isTrigger = false;
            GetComponent<PlayerMovement>().enabled = true;
        }
    }
}