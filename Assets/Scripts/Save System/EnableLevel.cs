using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableLevel : MonoBehaviour
{
    Button button;

    // This script enables or disables the level button depending if the previous level has been completed or not.

    void Start()
    {
        button = GetComponent<Button>();

        Level thisLevel = SaveSystem.LoadData(this.name);
        Level previousLevel = SaveSystem.LoadData("Level " + (thisLevel.levelNumber - 1).ToString());

        if (thisLevel.levelNumber == 1 || previousLevel.bestTime > 0)
        {
            button.interactable = true; // Enable the button if it's the first level or the previous level has a best time
        }
        else
        {
            button.interactable = false; // Disable the button if the previous level has no best time
        }
    }
}