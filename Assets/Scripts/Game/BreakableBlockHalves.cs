using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlockHalves : MonoBehaviour
{
    Collider2D halfCollider;
    [SerializeField]
    private PhysicsMaterial2D normalMaterial , frictionlessMaterial;
    void Start()
    {
        //get the collider component
        halfCollider = GetComponent<Collider2D>();
    }

    //Switch to a friction material when it collides with anything but the player
    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag != "Player")
        {
            SwitchMaterialNormal();
        }
    }

    void SwitchMaterialNormal()
    {
        halfCollider.sharedMaterial = normalMaterial; // switch to normal (has friction) material
    }

    public void SwitchMaterialFrictionless()
    {
        halfCollider.sharedMaterial = frictionlessMaterial; // switch to frictionless material
    }
    
}
