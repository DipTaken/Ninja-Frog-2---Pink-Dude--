using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private Slider volumeSlider, FPSSlider;
    private Button fullscreenButton, resolutionButtonUp, resolutionButtonDown, resetSettingsButton;
    private Text FPSText, FPSTextShad, fullscreenText, resolutionText;
    public int maxResolutionX;

    void ResetSettings()
    {
        SettingsLoadSave.ResetSettings();
        Awake(); // Reinitialize settings after reset
    }
    // Start is called before the first frame update
    void Awake()
    {
        FindGameObjects();
        AddListeners();
        InitializeUI();
    }

    void Start()
    {
        //adds a listener to all sliders in the scene to play a tick sound when the value changes
        Slider[] slidersInScene = FindObjectsOfType<Slider>();
        foreach (Slider slider in slidersInScene)
        {
            slider.onValueChanged.AddListener(delegate { PlayTickSound(); });
        }
    }

    void FindGameObjects()
    {
        //grabs all UI elements in scene
        volumeSlider = GameObject.Find("Volume Slider").GetComponent<Slider>();
        FPSSlider = GameObject.Find("FPS Slider").GetComponent<Slider>();
        fullscreenButton = GameObject.Find("Fullscreen Button").GetComponent<Button>();
        resolutionButtonUp = GameObject.Find("Forward Arrow").GetComponent<Button>();
        resetSettingsButton = GameObject.Find("Reset Settings Button").GetComponent<Button>();
        resolutionButtonDown = GameObject.Find("Back Arrow").GetComponent<Button>();
        FPSText = GameObject.Find("FPS Text").GetComponent<Text>();
        FPSTextShad = GameObject.Find("FPS Text Shad").GetComponent<Text>();
        fullscreenText = GameObject.Find("Fullscreen Text").GetComponent<Text>();
        resolutionText = GameObject.Find("Resolution Text").GetComponent<Text>();
    }

    void InitializeUI()
    {
        // Load settings from Settings (playerprefs)
        volumeSlider.value = Settings.volume * 50;

        switch (Settings.FPS)
        {
            case -1:
                FPSSlider.value = 0; // VSync
                break;
            case 30:
                FPSSlider.value = 1; // 30 FPS
                break;
            case 60:
                FPSSlider.value = 2; // 60 FPS
                break;
            case 120:
                FPSSlider.value = 3; // 120 FPS
                break;
            case 240:
                FPSSlider.value = 4; // 240 FPS
                break;
            default:
                FPSSlider.value = 2; // Default to 60 FPS
                break;
        }
        FPSText.text = Settings.FPS == -1 ? "VSync" : "FPS: " + Settings.FPS;

        resolutionText.text = "Resolution:\n" + Settings.resolution.x + "x" + Settings.resolution.y;

        fullscreenText.text = Settings.isFullscreen == 1 ? "Fullscreen" : "Windowed";

        //grabs all resolutions from the OS settings
        Resolution[] resolutions = Screen.resolutions;
        
        // Print the resolutions  
        // foreach (var res in resolutions)
        // {
        //     Debug.Log(res.width + "x" + res.height);
        // }

        //sets the maximum resolution X value
        maxResolutionX = resolutions[resolutions.Length - 1].width;
    }

    void AddListeners()
    {
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
        FPSSlider.onValueChanged.AddListener(ChangeFPS);
        fullscreenButton.onClick.AddListener(FullscreenToggle);
        resolutionButtonUp.onClick.AddListener(delegate { ChangeResolution(1); });
        resolutionButtonDown.onClick.AddListener(delegate { ChangeResolution(-1); });
        resetSettingsButton.onClick.AddListener(ResetSettings);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Settings.volume);
        // Debug.Log(Settings.FPS);
        // Debug.Log(Settings.isFullscreen);
        // Debug.Log(Settings.resolution.x + "x" + Settings.resolution.y);
        //Debug.Log(Settings.resolutionIndex);
    }

    void PlayTickSound()
    {
        SoundManager.instance.sliderTick.Play();
    }

    void ChangeVolume(float value)
    {
        //changes global volume based on the slider value
        Settings.volume = value / 50;
        AudioListener.volume = Settings.volume;
    }

    void ChangeFPS(float value)
    {
        //changes the max FPS based on the slider value
        switch (value)
        {
            case 0:
                Settings.FPS = -1;
                break;
            case 1:
                Settings.FPS = 30;
                break;
            case 2:
                Settings.FPS = 60;
                break;
            case 3:
                Settings.FPS = 120;
                break;
            case 4:
                Settings.FPS = 240;
                break;
            default:
                Settings.FPS = 60;
                break;
        }

        //if the slider is 0, set max FPS to vSync, otherwise, set it to the target FPS
        if (Settings.FPS == -1)
        {
            Settings.isVSync = 1;
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            Settings.isVSync = 0;
            QualitySettings.vSyncCount = 0;
        }
        Application.targetFrameRate = Settings.FPS; //set the target frame rate

        FPSText.text = Settings.FPS == -1 ? "VSync" : "FPS: " + Settings.FPS.ToString(); //update text
        FPSTextShad.text = FPSText.text; 

    }

    void FullscreenToggle()
    {
        //toggles fullscreen mode and updates the UI text accordingly
        if (Settings.isFullscreen == 1)
        {
            Settings.isFullscreen = 0;
            fullscreenText.text = "Windowed";
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Settings.isFullscreen = 1;
            fullscreenText.text = "Fullscreen";
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }

    void ChangeResolution(int change)
    {
        //checks if the change would go out of bounds of the resolution index
        if (Settings.resolutionIndex + change < 0 || Settings.resolutionIndex + change > 4)
        {   
            return;
        }
        //checks if going up in resolution would exceed the maximum resolution X value
        else if (Settings.resolution.x >= maxResolutionX)
        {
            if (change < 0)
            {
                Settings.resolutionIndex += change;
            }
        }
        //increase or decrease the resolution index based on the change value
        else
        {
            Settings.resolutionIndex += change;
        }
        
        //sets the resolution based on the index
        switch (Settings.resolutionIndex)
        {
            case 0:
                Settings.resolution.x = 1280;
                Settings.resolution.y = 720;
                break;
            case 1:
                Settings.resolution.x = 1600;
                Settings.resolution.y = 900;
                break;
            case 2:
                Settings.resolution.x = 1920;
                Settings.resolution.y = 1080;
                break;
            case 3:
                Settings.resolution.x = 2560;
                Settings.resolution.y = 1440;
                break;
            case 4:
                Settings.resolution.x = 3840;
                Settings.resolution.y = 2160;
                break;
            default:
                break;
        }
        Screen.SetResolution((int)Settings.resolution.x, (int)Settings.resolution.y, Settings.isFullscreen == 1); //set the screen resolution and fullscreen mode
        
        resolutionText.text = "Resolution:\n" + Settings.resolution.x + "x" + Settings.resolution.y; //update UI text
    }
}
