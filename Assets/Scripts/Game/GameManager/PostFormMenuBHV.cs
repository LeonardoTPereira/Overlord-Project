using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.GameManager
{
    public class PostFormMenuBHV : MonoBehaviour, IMenuPanel
    {
        [SerializeField]
        GameObject nextPanel;
        [SerializeField]
        TextMeshProUGUI postFormText;
        [SerializeField]
        Button playMoreButton;
        //TODO FIX THE GAMBIARRA
        private const string noMoreLevelsText = "Você jogou todos os níveis.\n Incrível!\n" +
                                                "Infelizmente não temos mais níveis para jogar\n" +
                                                "Mas agradecemos muito a sua colaboração neste experimento\n" +
                                                "Para sair, é só fechar a janela!";

        private void OnEnable()
        {
            playMoreButton.interactable = false;
            var hasMoreLevels = !GameManagerSingleton.Instance.IsLastQuestLine;
            if (hasMoreLevels) return;
            postFormText.text = noMoreLevelsText;
            ExperimentController.ProfileSelectedEventHandler += NewProfileSelected;
        }

        private void OnDisable()
        {
            ExperimentController.ProfileSelectedEventHandler -= NewProfileSelected;
        }

        public void GoToNext()
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene("Overworld");
            GameManagerSingleton.Instance.StopMusic();
        }

        private void NewProfileSelected(object sender, EventArgs e)
        {
            playMoreButton.interactable = true;
        }


        public void GoToPrevious()
        {
            Debug.LogError("There is no Panel to Go Back To!");
        }
    }
}
