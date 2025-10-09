using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    GameObject fireCollider;
    [SerializeField]
    private float offTime, onTime, variation;

    // This script controls a fire trap that alternates between being on and off.
    // The on and off times are randomized within a specified variation range.
    // The fireCollider is activated when the trap is on, allowing it to deal damage

    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("TurnOn", Random.Range(offTime - variation, offTime + variation));
    }

    void TurnOn()
    {
        animator.SetBool("On", true);
        fireCollider.SetActive(true);
        Invoke("TurnOff", Random.Range(onTime - variation, onTime + variation));
    }

    void TurnOff()
    {
        animator.SetBool("On", false);
        fireCollider.SetActive(false);
        Invoke("TurnOn", Random.Range(offTime - variation, offTime + variation));
    }
}