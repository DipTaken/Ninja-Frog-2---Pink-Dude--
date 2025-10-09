using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private float parallaxEffectX, parallaxEffectY;
    // Start is called before the first frame update

    // Parallax effect for background
    void LateUpdate()
    {
        // slightly move less than the camera to create a parallax effect
        transform.position = new Vector3(
            Camera.main.transform.position.x - (Camera.main.transform.position.x * parallaxEffectX),
            Camera.main.transform.position.y - (Camera.main.transform.position.y  * parallaxEffectY),
            transform.position.z
        );
    }
}
