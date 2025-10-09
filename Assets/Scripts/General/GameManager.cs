using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Level level;
    private GameObject player1, player2;
    private Text timerText, timerTextShad;
    private Text menuText, menuTextShad;
    public Text pauseText, pauseTextShad;
    [SerializeField]
    private float time;
    public int lives;
    private Image[] lifeIcons;
    public bool isGameOver = false;
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
        
        if (level.name != SceneManager.GetActiveScene().name) //warning to remind me to assign the correct level
        {
            Debug.LogWarning("Level name does not match the scene name!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; //hide cursor
        LoadLevelInfo();
        FindComponents();
        DisplayBestTime();
    }

    void SaveData()
    {
        if (time < level.bestTime || level.bestTime == 0) //if the time is less than the best time or if there is no best time yet
        {
            level.bestTime = Mathf.RoundToInt(time); //save the time as the best time
        }
        SaveSystem.SaveGameData(level.levelName, level);
    }

    void LoadLevelInfo()
    {
        if (SaveSystem.LoadData(level.levelName))
        {
            Level tempLevel = SaveSystem.LoadData(level.levelName);
            level.bestTime = tempLevel.bestTime; //for display purposes only
            level = tempLevel; //load level
        }
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        CheckWin();
    }

    IEnumerator GameOver(bool didWin, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManagerUI.instance.PauseGame();

        if (didWin) //if the player has won the game
        {
            SoundManager.instance.win.Play();
            SaveData(); //save the level data if the player has won

            //check if the next level exists in the build settings
            //if so, enable the next level button in the UI
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                GameManagerUI.instance.nextButton.gameObject.SetActive(false);
            }
            else
            {
                GameManagerUI.instance.nextButton.gameObject.SetActive(true);
            }

            //set the menu text to display the win message
            menuText.text = "You made it!";
            menuTextShad.text = menuText.text;
            pauseText.text = "You beat the level in " + time.ToString("0") + " seconds!";
            pauseTextShad.text = pauseText.text;
        }
        else //if the player has died
        {
            SoundManager.instance.lose.Play();
            //set the menu text to display the game over message
            menuText.text = "Game Over";
            menuTextShad.text = menuText.text;
            pauseText.text = "You lost all of your lives!";
            pauseTextShad.text = pauseText.text;
        }

        isGameOver = true;
        StopAllCoroutines(); //only allow this to run once
    }

    //used for displaying the timer in the UI
    void Timer()
    {
        if (GameManagerUI.instance.isPaused)
        {
            return;
        }

        time += Time.deltaTime;
        timerText.text = time.ToString("0");
        timerTextShad.text = timerText.text;
    }

    public void UpdateLives(int amount, GameObject player)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (isGameOver) //if the game is already over, do not update lives
        {
            return;
        }
        if (playerManager.timeSinceHit < playerManager.hitCooldown || lives < 0) //if the player is still in cooldown or if lives are already below 0
        {
            return;
        }

        lives += amount; 

        // update life icons
        for (int i = lives; i < lifeIcons.Length; i++)
        {
            if (i < 0 || i > lifeIcons.Length - 1)
            {
                continue;
            }

            lifeIcons[i].enabled = false;
        }

        playerManager.Hit();
        if (lives <= 0) //if the player has no lives left
        {
            playerManager.Death();
            StartCoroutine(GameOver(false, 0.8f));
        }
    }

    void CheckWin()
    {
        if (isGameOver) //already game over, no need to check for win conditions
        {
            return;
        }
        
        //checks if both players are stepping on the win plates
        bool player1Complete = player1.GetComponent<PlayerChecks>().hasFinished;
        bool player2Complete = player2.GetComponent<PlayerChecks>().hasFinished;
        if (player1Complete && player2Complete)
        {
            //Debug.Log("Both players have finished the game!");
            StartCoroutine(GameOver(true, 0.1f)); //start the game over coroutine with a short delay
        }
    }

    void FindComponents()
    {
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        timerTextShad = GameObject.Find("TimerTextShad").GetComponent<Text>();
        menuText = GameObject.Find("Title Text").GetComponent<Text>();
        menuTextShad = GameObject.Find("Title Text Shad").GetComponent<Text>();
        pauseText = GameObject.Find("Subtitle Text").GetComponent<Text>();
        pauseTextShad = GameObject.Find("Subtitle Text Shad").GetComponent<Text>();
        player1 = GameObject.Find("Player 1");
        player2 = GameObject.Find("Player 2");
        lifeIcons = GameObject.Find("Life Icons").GetComponentsInChildren<Image>();
    }

    // Displays the best time for the level in the pause menu
    void DisplayBestTime()
    {
        if (level.bestTime > 0)
        {
            pauseText.text = "Best time: " + level.bestTime.ToString();
            pauseTextShad.text = pauseText.text;
        }
        else
        {
            pauseText.text = "Best time: N/A";
            pauseTextShad.text = pauseText.text;
        }
    }
}