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
            var hasMoreLevels = !GameManagerSingleton.Instance.IsLastQuestLine;
            if (hasMoreLevels) return;
            postFormText.text = noMoreLevelsText;
            playMoreButton.interactable = false;
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
}
