using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBallModifier : MonoBehaviour
{
    [SerializeField]
    private GameObject spikeBall;
    [SerializeField]
    private MovingObject thisMovingObject, ballMovingObject;
    [SerializeField]
    private int radius;

    //add-on to the moving script to allow for a spike ball orbiting around a point

    // Start is called before the first frame update
    void Start()
    {
        spikeBall.transform.position = thisMovingObject.transform.position + new Vector3(radius, 0, 0);
        thisMovingObject.minConstraint = new Vector2(transform.position.x, 0);
        thisMovingObject.maxConstraint = new Vector2(transform.position.x + radius, 0);
        ballMovingObject.minConstraint = new Vector2(transform.position.x, 0);
        ballMovingObject.maxConstraint = new Vector2(transform.position.x + radius, 0);
    }
}
