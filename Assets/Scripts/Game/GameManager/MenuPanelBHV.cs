using UnityEngine;
using UnityEngine.UI;

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
