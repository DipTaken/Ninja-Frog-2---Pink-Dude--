using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAnimation : MonoBehaviour
{
    Animator animator;
    MovingObject movingObject;
    void Start()
    {
        //grab components
        animator = GetComponent<Animator>();
        movingObject = GetComponent<MovingObject>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //update animation based on movement direction/speed
        if (movingObject.xSpeed > 0 || movingObject.ySpeed > 0)
        {
            animator.SetFloat("Speed", 1f);
            animator.SetBool("Stop", false);
        }
        else if (movingObject.xSpeed < 0 || movingObject.ySpeed < 0)
        {
            animator.SetFloat("Speed", -1f);
            animator.SetBool("Stop", false);
        }
        else
        {
            animator.SetBool("Stop", true);
        }
    }
}
