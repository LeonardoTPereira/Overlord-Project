using Game.DataCollection;
using Game.Events;
using Game.ExperimentControllers;
using Game.LevelSelection;
using Game.NarrativeGenerator;
using MyBox;
using System;
using UnityEngine;

namespace Overlord.ProfileAnalyst
{
    public class PlayerProfileManager : MonoBehaviour
    {
        public static event Action<YeePlayerProfile> ProfileSelected;

        [field: SerializeField, MustBeAssigned] private PlayerDataController playerDataController;
        [field: SerializeField, MustBeAssigned] private DungeonDataController dungeonDataController;
        [field: SerializeField, MustBeAssigned] private GeneratorSettings generatorSettings;

        private void OnEnable()
        {
            NarrativeGenerator.NarrativeCreatorEventHandler += OnNarrativeCreated;
            FormBhv.PreTestFormQuestionAnsweredEventHandler += OnFormAnswered;
            RealTimeLevelSelectManager.PreTestFormQuestionAnsweredEventHandler += OnFormAnswered;
            ProfileTester.PreTestFormQuestionAnsweredEventHandler += OnProfileTested;
            LevelSelectManager.CompletedAllLevelsEventHandler += OnAllLevelsCompleted;
        }

        private void OnDisable()
        {
            NarrativeGenerator.NarrativeCreatorEventHandler -= OnNarrativeCreated;
            FormBhv.PreTestFormQuestionAnsweredEventHandler -= OnFormAnswered;
            RealTimeLevelSelectManager.PreTestFormQuestionAnsweredEventHandler -= OnFormAnswered;
            ProfileTester.PreTestFormQuestionAnsweredEventHandler -= OnProfileTested;
            LevelSelectManager.CompletedAllLevelsEventHandler -= OnAllLevelsCompleted;
        }

        private void OnNarrativeCreated(object sender, NarrativeCreatorEventArgs e)
        {
            var profile = YeeProfileCalculator.CreateProfile(e);
            ProfileSelected?.Invoke(profile);
        }

        private void OnFormAnswered(object sender, FormAnsweredEventArgs e)
        {            
            var profile = YeeProfileCalculator.CreateProfile(
                e.AnswerValue,
                generatorSettings.EnableRandomProfileToPlayer,
                generatorSettings.ProbabilityToGetTrueProfile);

            profile.IsFixedFromExperiment = sender.GetType() == typeof(RealTimeLevelSelectManager);
            ProfileSelected?.Invoke(profile);
        }

        private void OnProfileTested(object sender, ProfileTesterEventArgs e)
        {
            foreach (var formAnsweredArgs in e.Answers)
            {
                var profile = YeeProfileCalculator.CreateProfile(
                    formAnsweredArgs.AnswerValue,
                    generatorSettings.EnableRandomProfileToPlayer,
                    generatorSettings.ProbabilityToGetTrueProfile);

                ProfileSelected?.Invoke(profile);
            }
        }

        private void OnAllLevelsCompleted(object sender, EventArgs e)
        {
            var profile = YeeProfileCalculator.CreateProfile(
                playerDataController.CurrentPlayer,
                dungeonDataController.CurrentDungeon);

            ProfileSelected?.Invoke(profile);
        }
    }
}