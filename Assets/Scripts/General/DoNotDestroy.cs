using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour
{
    public static DoNotDestroy instance;

    //used for the bgm in non-level scenes

    void Awake()
    {
        //ensure that only one instance of this object exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        //destroy this object if it's in a level scene
        if (SceneManager.GetActiveScene().name != "Menu" &&
            SceneManager.GetActiveScene().name != "Settings" &&
            SceneManager.GetActiveScene().name != "Level Selector" &&
            SceneManager.GetActiveScene().name != "Credits")
        {
            Destroy(gameObject);
        }
    }
}