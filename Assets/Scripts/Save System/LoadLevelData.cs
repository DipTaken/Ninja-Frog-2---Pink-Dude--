using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelData : MonoBehaviour
{
    [SerializeField]
    private Text buttonLevel, buttonText;
    [SerializeField]
    private Level level;
    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();

        // Set the button text and level name
        buttonLevel.text = level.levelName;
        if (level.bestTime == 0)
        {
            buttonText.text = "Best Time: N/A";
        }
        else
        {
            buttonText.text = "Best Time: " + level.bestTime.ToString() + " s";
        }
    }

    void LoadLevel()
    {
        // Load the level data from the save system, or create a new entry if it doesn't exist
        if (SaveSystem.LoadData(level.levelName) == null)
        {
            SaveSystem.SaveGameData(this.gameObject.name, level);
        }
        level = SaveSystem.LoadData(level.levelName);
    }
}