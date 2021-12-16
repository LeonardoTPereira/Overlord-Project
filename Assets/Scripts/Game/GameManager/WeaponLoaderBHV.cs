using Game.EnemyGenerator;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponLoaderBHV : MonoBehaviour, IMenuPanel
{
    ProjectileTypeSO projectileSO;
    [SerializeField]
    GameObject previousPanel;
    [SerializeField]
    Button button;
    [SerializeField]
    SceneReference levelToLoad;
    public static event LoadWeaponButtonEvent LoadWeaponButtonEventHandler;

    protected void OnEnable()
    {
        button.interactable = false;
        WeaponSelectionButtonBHV.SelectWeaponButtonEvent += PrepareWeapon;
    }

    protected void OnDisable()
    {
        WeaponSelectionButtonBHV.SelectWeaponButtonEvent -= PrepareWeapon;
    }

    protected void PrepareWeapon(object sender, LoadWeaponButtonEventArgs eventArgs)
    {
        projectileSO = eventArgs.ProjectileSO;
        button.interactable = true;
    }

    public void GoToNext()
    {
        LoadWeaponButtonEventHandler(this, new LoadWeaponButtonEventArgs(projectileSO));
        SceneManager.LoadScene(levelToLoad.SceneName);
        gameObject.SetActive(false);
    }

    public void GoToPrevious()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
