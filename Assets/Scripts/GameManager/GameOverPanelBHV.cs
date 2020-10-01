using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelBHV : MonoBehaviour, IMenuPanel
{
    [SerializeField]
    GameObject nextPanel;
    [SerializeField]
    GameObject WeaponSelectPanel;
    [SerializeField]
    bool hasWon;

    public void GoToNext()
    {
        nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void GoToPrevious()
    {
        PlayerProfile.instance.OnMapComplete();
        WeaponSelectPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
