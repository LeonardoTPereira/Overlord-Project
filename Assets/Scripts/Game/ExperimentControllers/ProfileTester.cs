using System.Collections.Generic;
using System.Linq;
using Game.Events;
using Game.LevelSelection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.ExperimentControllers
{
    public class ProfileTester : MonoBehaviour
    {
        [field: SerializeField] public SelectedLevels Selected { get; set; }
        public static event ProfileTesterEvent PreTestFormQuestionAnsweredEventHandler;

        public void StartGeneration(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            var profileAnswers = new List<FormAnsweredEventArgs>();
            foreach (var levelItem in Selected.Levels)
            {
                Selected.SelectLevel(levelItem);
                var profileWeights = new List<int>();
                if (levelItem is not RealTimeLevelData realTimeLevelData) continue;
                profileWeights.Add(realTimeLevelData.AchievementWeight);
                profileWeights.Add(realTimeLevelData.CreativityWeight);
                profileWeights.Add(realTimeLevelData.ImmersionWeight);
                profileWeights.Add(realTimeLevelData.MasteryWeight);
                profileAnswers.Add(new FormAnsweredEventArgs(-1, profileWeights));
            }
            PreTestFormQuestionAnsweredEventHandler?.Invoke(this, new ProfileTesterEventArgs(profileAnswers));
            SceneManager.LoadScene("ContentGenerator");
        }
    }
}