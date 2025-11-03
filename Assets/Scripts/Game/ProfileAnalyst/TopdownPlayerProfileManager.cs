using Game.DataCollection;
using Game.Events;
using Game.ExperimentControllers;
using Game.GameManager;
using Game.LevelSelection;
using Game.NarrativeGenerator;
using MyBox;
using System;
using UnityEngine;

namespace Overlord.ProfileAnalyst
{
    public class TopdownPlayerProfileManager : MonoBehaviour
    {
        public static event Action<IPlayerProfile> ProfileSelected;

        [field: SerializeField, MustBeAssigned] private PlayerDataController _playerDataController;
        [field: SerializeField, MustBeAssigned] private DungeonDataController dungeonDataController;
        [field: SerializeField, MustBeAssigned] private GeneratorSettings generatorSettings;
        public static event ProfileSelectedEvent GameplayProfileSelectedEventHandler;

        private IPlayerProfileCalculator _profileCalculator = new YeeProfileCalculator();        // Change it with another player profile calculator if needed

        private void OnEnable()
        {
            Game.NarrativeGenerator.NarrativeGenerator.NarrativeCreatorEventHandler += SelectPlayerProfile;
            FormBhv.PreTestFormQuestionAnsweredEventHandler += SelectPlayerProfile;
            RealTimeLevelSelectManager.PreTestFormQuestionAnsweredEventHandler += SelectPlayerProfile;
            ProfileTester.PreTestFormQuestionAnsweredEventHandler += SelectPlayerProfile;
            LevelSelectManager.CompletedAllLevelsEventHandler += SelectPlayerProfile;
            ExperimentController.StartExperimentGeneratorEventHandler += SelectPlayerProfile;
        }

        private void OnDisable()
        {
            Game.NarrativeGenerator.NarrativeGenerator.NarrativeCreatorEventHandler -= SelectPlayerProfile;
            FormBhv.PreTestFormQuestionAnsweredEventHandler -= SelectPlayerProfile;
            RealTimeLevelSelectManager.PreTestFormQuestionAnsweredEventHandler -= SelectPlayerProfile;
            ProfileTester.PreTestFormQuestionAnsweredEventHandler -= SelectPlayerProfile;
            LevelSelectManager.CompletedAllLevelsEventHandler -= SelectPlayerProfile;
            ExperimentController.StartExperimentGeneratorEventHandler -= SelectPlayerProfile;

        }

        private void SelectPlayerProfile(object sender, NarrativeCreatorEventArgs e)
        {
            var profile = _profileCalculator.CreateProfileFromNarrative(e);
            ProfileSelected?.Invoke(profile);
        }

        private void SelectPlayerProfile(object sender, FormAnsweredEventArgs e)
        {
            var playerProfile = _profileCalculator.CreateProfileFromFormAnswers(e.AnswerValue, generatorSettings);
            playerProfile.IsFixedFromExperiment = sender.GetType() == typeof(RealTimeLevelSelectManager);

            ProfileSelected?.Invoke(playerProfile);
            //ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs((YeePlayerProfile)playerProfile));
        }

        private void SelectPlayerProfile(object sender, ProfileTesterEventArgs e)
        {
            foreach (var formAnsweredArgs in e.Answers)
            {
                var playerProfile = _profileCalculator.CreateProfileFromFormAnswers(formAnsweredArgs.AnswerValue, generatorSettings);
                ProfileSelected?.Invoke(playerProfile);
            }
        }

        private void SelectPlayerProfile(object sender, EventArgs eventArgs)
        {
            YeePlayerProfile playerProfile = _playerDataController.CurrentPlayer.SerializedData.PlayerProfile;
            if (!ExperimentController.UseFixedProfile)
            {
                playerProfile = (YeePlayerProfile)_profileCalculator.CreateProfileFromGameplay(_playerDataController.CurrentPlayer, _playerDataController.CurrentPlayer.CurrentDungeon);
            }
            ProfileSelected?.Invoke(playerProfile);
            GameplayProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
        }
    }
}