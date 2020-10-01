using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoaderBHV : MonoBehaviour, IMenuPanel
{
    string levelFile;
    int levelDifficulty;
    [SerializeField]
    GameObject previousPanel, nextPanel;
    [SerializeField]
    Button button;
    public delegate void LoadLevelButtonEvent(string fileName, int difficulty);
    public static event LoadLevelButtonEvent loadLevelButtonEvent;

    protected void OnEnable()
    {
        button.interactable = false;
        LevelSelectButtonBHV.selectLevelButtonEvent += PrepareLevel;
    }

    protected void OnDisable()
    {
        LevelSelectButtonBHV.selectLevelButtonEvent -= PrepareLevel;
    }

    protected void PrepareLevel(LevelConfigSO levelConfigSO)
    {
        levelFile = levelConfigSO.fileName;
        levelDifficulty = levelConfigSO.enemy;
        button.interactable = true;
    }

    public void GoToNext()
    {
        loadLevelButtonEvent(levelFile, levelDifficulty);
        nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void GoToPrevious()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
