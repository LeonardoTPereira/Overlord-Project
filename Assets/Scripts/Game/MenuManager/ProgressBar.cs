using Overlord.Events;
using Overlord.LevelGenerator;
using Overlord.LevelGenerator.Events;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MenuManager
{
    public class ProgressBar : MonoBehaviour
    {
        [field: SerializeField] public Image Bar { get; set; }
        [field: SerializeField] public TextMeshProUGUI ProgressText { get; set; }
        private string OldText { get; set; }

        private void Start()
        {
            OldText = ProgressText.text;
            if (GameManager.GameManagerSingleton.Instance.IsInPortuguese)
                ProgressText.text = "Gerando o Calabouço";
            else
                ProgressText.text = "Generating Content";
        }

        private void OnEnable()
        {
            GeneticAlgorithmManager.NewEaGenerationEventHandler += UpdateProgressBar;
        }

        private void OnDisable()
        {
            GeneticAlgorithmManager.NewEaGenerationEventHandler -= UpdateProgressBar;
        }

        private void UpdateProgressBar(object sender, NewEAGenerationEventArgs eventArgs)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(UpdateProgressBarAndText(eventArgs.CompletionRate, eventArgs.HasFinished));
            UnityMainThreadDispatcher.Instance().Enqueue(ApplyOldTextWhenFinished(eventArgs.HasFinished));
        }

        private IEnumerator UpdateProgressBarAndText(float completionRate, bool hasFinished)
        {
            if (completionRate > 1.0f && !hasFinished)
            {
                if (GameManager.GameManagerSingleton.Instance.IsInPortuguese)
                    ProgressText.text = "Gerando Níveis Melhorados";
                else
                    ProgressText.text = "Creating Better Levels";

                Bar.fillAmount = completionRate % 1.0f;
            }
            else
            {
                Bar.fillAmount = completionRate;
            }
            yield return null;
        }

        private IEnumerator ApplyOldTextWhenFinished(bool hasFinished)
        {
            if (hasFinished)
            {
                ProgressText.text = OldText;
            }

            yield return null;
        }
    }
}