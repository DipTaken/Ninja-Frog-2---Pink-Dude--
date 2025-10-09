using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearData : MonoBehaviour
{
    [SerializeField]
    private Level[] levels;
    [SerializeField]
    private GameObject warningUI, selectUI;

    void Start()
    {
        warningUI.SetActive(false); // Hide the warning UI at the start
    }
    public void ClearSaveData() //attached to the "Clear Data" button family in the UI
    {
        // Clear all the data in the levels then reload the scene to display the new data
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].bestTime = 0;
            SaveSystem.SaveGameData(levels[i].levelName, levels[i]);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowWarning()
    {
        // Show the warning UI
        warningUI.SetActive(true);
        selectUI.SetActive(false);
    }

    public void HideWarning()
    {
        // Hide the warning UI
        warningUI.SetActive(false);
        selectUI.SetActive(true);
    }
}
