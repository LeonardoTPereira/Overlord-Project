using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransitionPanelBHV : MonoBehaviour, IMenuPanel
{
    [SerializeField]
    protected GameObject previousPanel;
    [SerializeField, Scene]
    protected string nextPanel;
    public void GoToNext()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(nextPanel);
    }
    public void GoToPrevious()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
