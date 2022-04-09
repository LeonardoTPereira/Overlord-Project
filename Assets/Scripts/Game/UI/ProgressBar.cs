using System;
using Game.Events;
using Game.LevelGenerator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [field: SerializeField] public Image Bar { get; set; }
        [field: SerializeField] public TextMeshProUGUI ProgressText { get; set; }
        private string OldText { get; set; }

        private void Start()
        {
            OldText = ProgressText.text;
            ProgressText.text = "Generating Content";
        }

        private void OnEnable()
        {
            LevelGenerator.LevelGenerator.NewEaGenerationEventHandler += UpdateProgressBar;
        }

        private void OnDisable()
        {
            LevelGenerator.LevelGenerator.NewEaGenerationEventHandler -= UpdateProgressBar;
        }

        private void UpdateProgressBar(object sender, NewEAGenerationEventArgs eventArgs)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => Bar.fillAmount = eventArgs.CompletionRate);
            UnityMainThreadDispatcher.Instance().Enqueue(ApplyOldTextWhenFinished);
        }

        private void ApplyOldTextWhenFinished()
        {
            if (Bar.fillAmount >= 1.0f)
            {
                ProgressText.text = OldText;
            }
        }
    }
}