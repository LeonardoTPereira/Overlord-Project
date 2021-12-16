using UnityEngine;
using UnityEngine.UI;

public class LevelLoaderBHV : MonoBehaviour, IMenuPanel
{
    string levelFile;
    [SerializeField]
    GameObject previousPanel, nextPanel;
    [SerializeField]
    Button button;
    public static event LevelLoadEvent loadLevelButtonEventHandler;

    protected void OnEnable()
    {
        button.interactable = false;
        LevelSelectButtonBHV.selectLevelButtonEventHandler += PrepareLevel;
    }

    protected void OnDisable()
    {
        LevelSelectButtonBHV.selectLevelButtonEventHandler -= PrepareLevel;
    }

    protected void PrepareLevel(object sender, LevelSelectEventArgs args)
    {
        levelFile = args.LevelSO.fileName;
        button.interactable = true;
    }

    public void GoToNext()
    {
        loadLevelButtonEventHandler(this, new LevelLoadEventArgs(levelFile));
        nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void GoToPrevious()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
