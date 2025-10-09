using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource BGM, buttonClick, sliderTick;
    public AudioSource jump, hit, respawn, land, death, win, lose;
    public AudioSource blockBreak;

    //holds all audio clips for other scripts to access

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}