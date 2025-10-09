using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;
    [SerializeField]
    private Animator animator;
    private float transitionDuration = 0.34f;
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

        //finds all buttons in scene and adds button click sound
        Button[] buttonsInScene = FindObjectsOfType<Button>();
        foreach (Button button in buttonsInScene)
        {
            button.onClick.AddListener(PlayButtonClick);
        }
    }

    // to be attached to buttons - switches scene by calling the transition
    public void SwitchScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == "Settings")
        {
            SettingsLoadSave.SaveSettings();
        }
        StartCoroutine(LoadSceneAnimation(sceneName));
    }

    // scene fade in/out animation
    IEnumerator LoadSceneAnimation(string levelName) //https://www.youtube.com/watch?v=CE9VOZivb3I How to make AWESOME Scene Transitions in Unity! - Brackeys
    {
        Debug.Log("LoadLevel(\"" + levelName + "\")");
        Time.timeScale = 1f;
        animator.SetTrigger("Start"); //start fade in

        yield return new WaitForSeconds(transitionDuration); //wait until fade in is complete
        SceneManager.LoadScene(levelName); //load the scene
    }

    public void NextLevel()
    {
        //loads next level in the build order
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void QuitGame()
    {
        // This will quit the game in a build or stop playing in the editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the editor
        #else
            Application.Quit(); // Quit the application in a build
        #endif
    }

    public void RestartGame()
    {
        // Reload the current scene to restart the game
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void PlayButtonClick()
    {
        SoundManager.instance.buttonClick.Play();
    }
}