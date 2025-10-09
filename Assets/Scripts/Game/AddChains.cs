using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChains : MonoBehaviour
{
    [SerializeField]
    private GameObject platform;
    [SerializeField]
    private GameObject chainPrefab;
    private MovingObject movingObject;
    private int distanceX, distanceY;
    private Vector2 minChainPosition, maxChainPosition;
    // Start is called before the first frame update
    void Start()
    {
        movingObject = platform.GetComponent<MovingObject>();

        //get the min and max constraints of the moving platform
        minChainPosition = new Vector2(movingObject.minConstraint.x, movingObject.minConstraint.y);
        maxChainPosition = new Vector2(movingObject.maxConstraint.x, movingObject.maxConstraint.y);

        //calculate the delta for x and y axis
        distanceX = (int)(maxChainPosition.x - minChainPosition.x);
        distanceY = (int)(maxChainPosition.y - minChainPosition.y);

        //place chains along the platform path
        if (distanceX > 0)
        {
            PlaceChains(distanceX);
        }
        else if (distanceY > 0)
        {
            PlaceChains(distanceY);
        }

    }

    //Place chains along the platform path evenly spaced apart by 1 unit
    void PlaceChains(int distance)
    {
        for (int i = 0; i <= distance; i++)
        {
            GameObject chain = Instantiate(chainPrefab, this.transform);

            if (distanceX > 0) //place along x axis
            {
                chain.transform.position = new Vector2(minChainPosition.x + i, platform.transform.position.y);
            }
            else if (distanceY > 0) //place along y axis
            {
                chain.transform.position = new Vector2(platform.transform.position.x, minChainPosition.y + i);
            }
            chain.name = "Chain" + i;
        }
    }
}