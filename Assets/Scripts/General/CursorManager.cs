using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursorTexture, cursorHoldTexture;
    private Vector2 hotspot = new Vector2(32, 32);
    // Update is called once per frame
    // Dymanic cursor change based on if the mouse button is held down
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(cursorHoldTexture, hotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }
    }
}
