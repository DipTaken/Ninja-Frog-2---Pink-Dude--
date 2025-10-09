using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject topHalf, bottomHalf;
    Collider2D blockCollider;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public bool isBroken = false;
    [SerializeField]
    private float breakDelay, respawnDelay;
    public bool canBreakfromBelow = true;
    // Start is called before the first frame update
    void Start()
    {
        //grab components
        blockCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBroken)
        {
            PlayerCheck();
        }
    }

    public IEnumerator Break(float delay)
    {
        //break block and spawn halves which will fall (particles)
        isBroken = true;
        yield return new WaitForSeconds(delay);
        SoundManager.instance.blockBreak.Play();
        blockCollider.enabled = false; //hide block
        spriteRenderer.enabled = false;
        AddRigidbody(topHalf);
        AddRigidbody(bottomHalf);
        topHalf.GetComponent<Animator>().SetTrigger("Break"); //break anim
        bottomHalf.GetComponent<Animator>().SetTrigger("Break"); //break anim

        StartCoroutine(ResetBlock(3f)); //reset block after 3 seconds
    }

    void AddRigidbody(GameObject gameObject)
    {  
        //add Rigidbody2D to the block halves, set default values, and apply random force and rotation
        gameObject.AddComponent<Rigidbody2D>();
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        rb.mass = 1f;

        Vector2 force = new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f));
        rb.AddForce(force, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-1f, 1f), ForceMode2D.Impulse);
    }

    public void PlayerCheck()
    {
        // Check if player is touching the block from the top or bottom
        Vector2 rayOriginTop = new Vector2(blockCollider.bounds.min.x + 0.05f, blockCollider.bounds.max.y + 0.01f);
        Vector2 rayOriginBottom = new Vector2(blockCollider.bounds.min.x + 0.05f, blockCollider.bounds.min.y - 0.05f);

        float rayLength = blockCollider.bounds.size.x - 0.1f;
        RaycastHit2D topRay = Physics2D.Raycast(rayOriginTop, Vector2.right, rayLength, LayerMask.GetMask("Player"));
        RaycastHit2D bottomRay = Physics2D.Raycast(rayOriginBottom, Vector2.right, rayLength, LayerMask.GetMask("Player"));

        Debug.DrawRay(rayOriginTop, new Vector2(rayLength, 0), topRay ? Color.green : Color.red);
        Debug.DrawRay(rayOriginBottom, new Vector2(rayLength, 0), bottomRay ? Color.green : Color.red);

        if (!isBroken)
        {
            if (topRay)
            {
                StartCoroutine(Break(breakDelay)); //break with delay if player is on top
            }
            else if (bottomRay && canBreakfromBelow)
            {
                StartCoroutine(Break(0.05f)); //break immediately if player is below and canBreakfromBelow is true
            }
        }
    }

    IEnumerator ResetBlock(float delay)
    {
        //reset the block after a delay
        yield return new WaitForSeconds(delay);
        blockCollider.enabled = true; //show block
        spriteRenderer.enabled = true;
        Destroy(topHalf.GetComponent<Rigidbody2D>()); //get rid of rigidbodies
        Destroy(bottomHalf.GetComponent<Rigidbody2D>());
        topHalf.transform.position = new Vector2(transform.position.x, transform.position.y); //reset position
        bottomHalf.transform.position = new Vector2(transform.position.x, transform.position.y);
        topHalf.transform.rotation = Quaternion.identity; //reset rotation
        bottomHalf.transform.rotation = Quaternion.identity;
        
        //go back to frictionless material (to allow for sliding when initially broken)
        topHalf.GetComponent<BreakableBlockHalves>().SwitchMaterialFrictionless(); 
        bottomHalf.GetComponent<BreakableBlockHalves>().SwitchMaterialFrictionless();

        yield return new WaitForSeconds(0.5f);
        isBroken = false;
    }
}