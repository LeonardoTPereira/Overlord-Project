using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransitionPanelBHV : MonoBehaviour, IMenuPanel
{
    [SerializeField]
    protected GameObject previousPanel;
    [SerializeField]
    protected string nextPanel;
    public void GoToNext()
    {
        SceneManager.LoadScene(nextPanel);
    }    
    public void GoToPrevious()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
