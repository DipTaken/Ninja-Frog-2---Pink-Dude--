using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

//Holds level data
public class Level : ScriptableObject
{
    public string levelName;
    public int levelNumber;
    public int bestTime;
}
