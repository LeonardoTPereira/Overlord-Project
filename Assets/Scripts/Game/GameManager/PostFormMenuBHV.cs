using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostFormMenuBHV : MonoBehaviour, IMenuPanel
{
    [SerializeField]
    GameObject nextPanel;
    [SerializeField]
    TextMeshProUGUI postFormText;
    [SerializeField]
    Button playMoreButton;
    //TODO FIX THE GAMBIARRA
    public static event LevelLoadEvent postFormButtonEventHandler;
    private const string noMoreLevelsText = "Você jogou todos os níveis.\n Incrível!\n" +
        "Infelizmente não temos mais níveis para jogar\n" +
        "Mas agradecemos muito a sua colaboração neste experimento\n" +
        "Para sair, é só fechar a janela!";
    private bool hasMoreLevels;

    private void OnEnable()
    {
        /*hasMoreLevels = GameManager.instance.HasMoreLevels();
        if (!hasMoreLevels)
        {
            postFormText.text = noMoreLevelsText;
            playMoreButton.interactable = false;
        }*/
    }
    public void GoToNext()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("Overworld");
    }

    public void GoToPrevious()
    {
        Debug.LogError("There is no Panel to Go Back To!");
    }
}
