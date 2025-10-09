using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGameData(string levelName, Level level)
    {
        string json = JsonUtility.ToJson(level); //convert the level object to a json string
        PlayerPrefs.SetString(levelName, json); //save the json string to PlayerPrefs
        PlayerPrefs.Save(); //save the PlayerPrefs to disk
    }
    public static Level LoadData(string levelName)
    {
        Level data = ScriptableObject.CreateInstance<Level>(); //create a new instance of the Level scriptable object
        string json = PlayerPrefs.GetString(levelName); //get the json string from PlayerPrefs
        if (!string.IsNullOrEmpty(json)) //check if the json string is not empty
        {
            JsonUtility.FromJsonOverwrite(json, data); //convert the json string back to a Level object
            return data;
        }
        else
        {
            return null;
        }
    }
}
