using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeactivateBlocks : MonoBehaviour
{
    [SerializeField]
    private GameObject[] group1, group2;
    private bool group1Active = true;

    void Start()
    {
        //all blocks in group1 are active and group2 is inactive at the start
        foreach (GameObject block in group1)
        {
            Activate(block);
        }
        foreach (GameObject block in group2)
        {
            Deactivate(block);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            if (group1Active)
            {
                // Deactivate all blocks in group1
                foreach (GameObject block in group1)
                {
                    Deactivate(block);
                }

                // Activate all blocks in group2
                foreach (GameObject block in group2)
                {
                    Activate(block);
                }
                group1Active = false;
            }
            else
            {
                // Activate all blocks in group1
                foreach (GameObject block in group1)
                {
                    Activate(block);
                }

                // Deactivate all blocks in group2
                foreach (GameObject block in group2)
                {
                    Deactivate(block);
                }
                group1Active = true;
            }
        }
    }

    void Deactivate(GameObject block)
    {
        // Disable the collider and change the color (opacity) to indicate deactivation
        block.GetComponent<Collider2D>().enabled = false;
        if (block.GetComponent<Tilemap>() != null)
        {
            Tilemap blockTilemap = block.GetComponent<Tilemap>();
            block.GetComponent<Tilemap>().color = new Color(blockTilemap.color.r, blockTilemap.color.g, blockTilemap.color.b, 0.5f); ;
        }
        else
        {
            SpriteRenderer blockRenderer = block.GetComponent<SpriteRenderer>();
            block.GetComponent<SpriteRenderer>().color = new Color(blockRenderer.color.r, blockRenderer.color.g, blockRenderer.color.b, 0.5f); ;
        }
    }
    void Activate(GameObject block)
    {
        // Enable the collider and reset the color (opacity) to indicate activation
        block.GetComponent<Collider2D>().enabled = true;
        //cases to handle tilemaps and sprite renderers
        if (block.GetComponent<Tilemap>() != null)
        {
            Tilemap blockTilemap = block.GetComponent<Tilemap>();
            block.GetComponent<Tilemap>().color = new Color(blockTilemap.color.r, blockTilemap.color.g, blockTilemap.color.b, 1f); ;
        }
        else
        {
            SpriteRenderer blockRenderer = block.GetComponent<SpriteRenderer>();
            block.GetComponent<SpriteRenderer>().color = new Color(blockRenderer.color.r, blockRenderer.color.g, blockRenderer.color.b, 1f); ;
        }
    }
}
