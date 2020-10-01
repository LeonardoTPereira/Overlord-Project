using EnemyGenerator;
using System.Collections;
using System.Collections.Generic;
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
    protected string nextPanel;
    public delegate void LoadWeaponButtonEvent(ProjectileTypeSO projectileSO);
    public static event LoadWeaponButtonEvent loadWeaponButtonEvent;

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
        loadWeaponButtonEvent(projectileSO);
        SceneManager.LoadScene(nextPanel);
        gameObject.SetActive(false);
    }

    public void GoToPrevious()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
