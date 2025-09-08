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
        public static event Action<IPlayerProfile> ProfileSelected;

        [field: SerializeField, MustBeAssigned] private PlayerDataController playerDataController;
        [field: SerializeField, MustBeAssigned] private DungeonDataController dungeonDataController;
        [field: SerializeField, MustBeAssigned] private GeneratorSettings generatorSettings;

        private IPlayerProfileCalculator _profileCalculator = new YeeProfileCalculator();        // Change it with another player profile calculator if needed

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
            var profile = _profileCalculator.CreateProfileFromNarrative(e);
            ProfileSelected?.Invoke(profile);
        }

        private void OnFormAnswered(object sender, FormAnsweredEventArgs e)
        {            
            var profile = _profileCalculator.CreateProfileFromFormAnswers(
                e.AnswerValue,
                generatorSettings);

            profile.IsFixedFromExperiment = sender.GetType() == typeof(RealTimeLevelSelectManager);
            ProfileSelected?.Invoke(profile);
        }

        private void OnProfileTested(object sender, ProfileTesterEventArgs e)
        {
            foreach (var formAnsweredArgs in e.Answers)
            {
                var profile = _profileCalculator.CreateProfileFromFormAnswers(
                    formAnsweredArgs.AnswerValue,
                    generatorSettings);

                ProfileSelected?.Invoke(profile);
            }
        }

        private void OnAllLevelsCompleted(object sender, EventArgs e)
        {
            var profile = _profileCalculator.CreateProfileFromGameplay(
                playerDataController.CurrentPlayer,
                dungeonDataController.CurrentDungeon);

            ProfileSelected?.Invoke(profile);
        }
    }
}