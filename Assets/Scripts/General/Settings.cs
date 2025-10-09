using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores all settings
public class Settings
{
    //This class holds all the settings values that can be modified in the game.
    public static float volume;
    public static int FPS;
    public static int isVSync;
    public static int isFullscreen;
    public static Vector2 resolution;
    public static int resolutionIndex;
}

public static class SettingsLoadSave
{
    //This runs before any scene is loaded, ensuring settings are applied immediately, not just when the settings scene is loaded.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Awake()
    {
        LoadSettings();
    }

    //Saves all the values to PlayerPrefs
    public static void SaveSettings()
    {
        //saves all settings to PlayerPrefs
        PlayerPrefs.SetFloat("Volume", Settings.volume);
        PlayerPrefs.SetInt("FPS", Settings.FPS);
        PlayerPrefs.SetInt("VSync", Settings.isVSync);
        PlayerPrefs.SetInt("Fullscreen", Settings.isFullscreen);
        PlayerPrefs.SetInt("XResolution", (int)Settings.resolution.x);
        PlayerPrefs.SetInt("YResolution", (int)Settings.resolution.y);
        PlayerPrefs.SetInt("ResolutionIndex", Settings.resolutionIndex);
        PlayerPrefs.Save();
    }

    //Loads all the values from PlayerPrefs and applies them to the game
    public static void LoadSettings()
    {
        Settings.volume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = Settings.volume;

        Settings.FPS = PlayerPrefs.GetInt("FPS", 60);
        Application.targetFrameRate = Settings.FPS;

        Settings.isVSync = PlayerPrefs.GetInt("VSync", 1);
        QualitySettings.vSyncCount = Settings.isVSync;

        Settings.isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1);
        if (Settings.isFullscreen == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        Settings.resolution.x = PlayerPrefs.GetInt("XResolution", 1920);
        Settings.resolution.y = PlayerPrefs.GetInt("YResolution", 1080);
        Screen.SetResolution((int)Settings.resolution.x, (int)Settings.resolution.y, Settings.isFullscreen == 1);

        Settings.resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 2);
    }

    public static void ResetSettings()
    {
        PlayerPrefs.DeleteAll();
        LoadSettings();
    }
}
