using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostFormMenuBHV : MonoBehaviour, IMenuPanel
{
    [SerializeField]
    GameObject nextPanel;
    [SerializeField]
    TextMeshProUGUI postFormText;
    [SerializeField]
    Button playMoreButton;
    LevelConfigSO nextLevel;
    //TODO FIX THE GAMBIARRA
    public delegate void PostFormButtonEvent(string fileName, int difficulty);
    public static event PostFormButtonEvent postFormButtonEvent;
    private const string noMoreLevelsText = "Você jogou todos os níveis.\n Incrível!\n" +
        "Infelizmente não temos mais níveis para jogar\n" +
        "Mas agradecemos muito a sua colaboração neste experimento\n" +
        "Para sair, é só fechar a janela!";
    private bool hasMoreLevels;

    private void OnEnable()
    {
        
        hasMoreLevels = GameManager.instance.HasMoreLevels();
        if (!hasMoreLevels)
        {
            postFormText.text = noMoreLevelsText;
            playMoreButton.interactable = false;
        }
    }
    public void GoToNext()
    {
        if (hasMoreLevels)
        {
            nextLevel = GameManager.instance.PickNextLevel();
            postFormButtonEvent(nextLevel.fileName, nextLevel.enemy);
            nextPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void GoToPrevious()
    {
        Debug.LogError("There is no Panel to Go Back To!");
    }
}
