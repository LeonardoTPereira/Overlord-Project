using EnemyGenerator;
using MyBox;
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
    [SerializeField, Scene]
    protected string levelToLoad;
    public static event LoadWeaponButtonEvent loadWeaponButtonEventHandler;

    protected void OnEnable()
    {
        button.interactable = false;
        WeaponSelectionButtonBHV.selectWeaponButtonEvent += PrepareWeapon;
    }

    protected void OnDisable()
    {
        WeaponSelectionButtonBHV.selectWeaponButtonEvent -= PrepareWeapon;
    }

    protected void PrepareWeapon(ProjectileTypeSO projectileSO)
    {
        this.projectileSO = projectileSO;
        button.interactable = true;
    }

    public void GoToNext()
    {
        loadWeaponButtonEventHandler(this, new LoadWeaponButtonEventArgs(projectileSO));
        SceneManager.LoadScene(levelToLoad);
        gameObject.SetActive(false);
    }

    public void GoToPrevious()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
