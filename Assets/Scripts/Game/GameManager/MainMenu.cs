﻿using Game.GameManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject introScreen, introScreen2, gameOverScreen, levelSelect, weaponSelect, preTestForm;

    public void IntroScreen()
    {
        introScreen.SetActive(true);
    }
    public void PlayGame()
    {
        GameManagerSingleton.instance.createMaps = true;
        SceneManager.LoadScene("LevelWithEnemies");
    }
    public void CreateLevels()
    {
        SceneManager.LoadScene("LevelGenerator");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
    public void PlayLoadedLevel()
    {
        GameManagerSingleton.instance.createMaps = false;
        SceneManager.LoadScene("LevelWithEnemies");
    }

    public void RetryLevel()
    {
        GameManagerSingleton.instance.createMaps = false;
        SceneManager.LoadScene("LevelWithEnemies");
    }

    public void DifficultySelect()
    {
        weaponSelect.SetActive(false);
        levelSelect.SetActive(true);
    }

    public void WeaponSelect()
    {
        introScreen2.SetActive(false);
        weaponSelect.SetActive(true);
    }

    public void SecondIntro()
    {
        introScreen.SetActive(false);
        introScreen2.SetActive(true);
    }
}
