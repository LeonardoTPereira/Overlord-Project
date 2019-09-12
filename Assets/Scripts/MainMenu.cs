using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject introScreen, mainScreen;
    public void IntroScreen()
    {
        mainScreen.SetActive(false);
        introScreen.SetActive(true);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level");
    }
    public void CreateLevels()
    {
        SceneManager.LoadScene("LevelGenerator");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
