using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelBHV : MonoBehaviour, IMenuPanel
{
    [SerializeField]
    protected GameObject previousPanel, nextPanel;
    public void GoToNext()
    {
        nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    public void GoToPrevious()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
