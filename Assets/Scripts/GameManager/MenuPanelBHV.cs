using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelBHV : MonoBehaviour, IMenuPanel
{
    [SerializeField]
    protected GameObject previousPanel, nextPanel;
    public Text txt;
    public Button but;
    public GameObject menu;
    public void GoToNext()
    {
        nextPanel.SetActive(true);
        gameObject.SetActive(false);
        menu.SetActive(false);
    }
    public void GoToPrevious()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
