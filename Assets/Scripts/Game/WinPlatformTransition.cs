using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPlatformTransition : MonoBehaviour
{
    [SerializeField]
    private GameObject star, waitingText;
    // Start is called before the first frame update

    //This script shows a waiting message when only one player is on the win platform.
    //Creates a better UX because some users may be lost on the first level.

    void Start()
    {
        star.SetActive(true);
        waitingText.SetActive(false);
    }

    void LateUpdate()
    {
        if (GameManager.instance.isGameOver)
        {
            star.SetActive(false);
            waitingText.SetActive(false);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Player1" && this.gameObject.tag == "Win1") || (other.gameObject.tag == "Player2" && this.gameObject.tag == "Win2"))
        {
            star.SetActive(false);
            waitingText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Player1" && this.gameObject.tag == "Win1") || (other.gameObject.tag == "Player2" && this.gameObject.tag == "Win2"))
        {
            star.SetActive(true);
            waitingText.SetActive(false);
        }
    }
}
