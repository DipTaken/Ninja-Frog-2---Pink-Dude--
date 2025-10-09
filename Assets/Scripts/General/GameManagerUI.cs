using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    public static GameManagerUI instance;

    private GameObject menuUI, settingsUI;
    private Button menuButton, restartButton, settingsButton, backSettingsButton;
    public Button nextButton;
    [SerializeField]
    private float gameSpeed;
    public bool isPaused;
    public bool canPause;

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

        FindGameObjects();
        AddListeners();
    }
    // Start is called before the first frame update
    void Start()
    {
        //default values
        menuUI.SetActive(false);
        settingsUI.SetActive(false);
        canPause = false;
        isPaused = false;
        Time.timeScale = gameSpeed;
        Invoke("EnablePausing", 0.7f); //pause delay to allow scene transition to complete
    }

    void EnablePausing()
    {
        canPause = true;
    }

    void FindGameObjects()
    {
        //grabs all UI elements in scene
        menuUI = GameObject.Find("PAUSE MENU");
        settingsUI = GameObject.Find("SETTINGS MENU");
        menuButton = GameObject.Find("Menu Button").GetComponent<Button>();
        restartButton = GameObject.Find("Restart Button").GetComponent<Button>();
        nextButton = GameObject.Find("Next Level Button").GetComponent<Button>();
        settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
        backSettingsButton = GameObject.Find("Back Settings Button").GetComponent<Button>();
    }

    void AddListeners()
    {
        // Add listeners to buttons
        menuButton.onClick.AddListener(delegate { ButtonManager.instance.SwitchScene("Menu"); });
        restartButton.onClick.AddListener(ButtonManager.instance.RestartGame);
        nextButton.onClick.AddListener(ButtonManager.instance.NextLevel);
        settingsButton.onClick.AddListener(ToggleSettings);
        backSettingsButton.onClick.AddListener(ToggleSettings);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //if escape is pressed
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (GameManager.instance.isGameOver || !canPause) // do not pause if the game is over or if pausing is not allowed
        {
            return;
        }

        if (Time.timeScale == gameSpeed) // if the game is running, pause it
        {
            Camera.main.GetComponent<CameraZoom>().canZoom = false; // disable camera zoom
            Time.timeScale = 0f; // pause the game
            Cursor.visible = true;  // show the cursor
            SoundManager.instance.BGM.volume *= 0.3f; // lower bgm
            menuUI.SetActive(true); // Show the pause menu
            nextButton.gameObject.SetActive(false); // hide next button
            isPaused = true; 

        }
        else // if the game is paused, resume it
        {
            Camera.main.GetComponent<CameraZoom>().canZoom = true; // enable camera zoom
            Time.timeScale = gameSpeed; // resume the game
            Cursor.visible = false; // hide the cursor
            SoundManager.instance.BGM.volume /= 0.3f; // restore bgm volume
            menuUI.SetActive(false); // hide the pause menu
            settingsUI.SetActive(false); // hide settings menu
            isPaused = false;
        }   
    }
    
    public void ToggleSettings()
    {
        // toggles the settings menu on and off
        if (settingsUI.activeSelf)
        {
            settingsUI.SetActive(false);
            menuUI.SetActive(true);
        }
        else
        {
            settingsUI.SetActive(true);
            menuUI.SetActive(false);
        }
    }
}
