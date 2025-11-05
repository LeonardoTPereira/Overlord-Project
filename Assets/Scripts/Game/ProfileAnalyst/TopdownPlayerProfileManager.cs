using Game.DataCollection;
using Game.Events;
using Game.ExperimentControllers;
using Game.GameManager;
using Game.LevelSelection;
using MyBox;
using System;
using UnityEngine;
using Overlord.ProfileAnalyst;
using Game.Overlord.ProfileAnalyst;

namespace Topdown.Overlord.ProfileAnalyst
{
    public class TopdownPlayerProfileManager : PlayerProfileManager
    {
        [field: SerializeField, MustBeAssigned] private PlayerDataController _playerDataController;
        [field: SerializeField, MustBeAssigned] private DungeonDataController dungeonDataController;
        [field: SerializeField, MustBeAssigned] private GeneratorSettings generatorSettings;
        public static event ProfileSelectedEvent GameplayProfileSelectedEventHandler;

        private IPlayerProfileCalculator _profileCalculator = new TopdownYeeProfileCalculator();        // Change it with another player profile calculator if needed

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
            if (_profileCalculator is TopdownYeeProfileCalculator yeeProfileCalculator)
            {
                var playerProfile = yeeProfileCalculator.CreateProfileFromNarrative(e);
                InvokeEventOnSelectedProfile(playerProfile);
            }
        }

        private void SelectPlayerProfile(object sender, FormAnsweredEventArgs e)
        {
            if (_profileCalculator is TopdownYeeProfileCalculator yeeProfileCalculator)
            {
                var playerProfile = yeeProfileCalculator.CreateProfileFromFormAnswers(e.AnswerValue, generatorSettings);
                playerProfile.IsFixedFromExperiment = sender.GetType() == typeof(RealTimeLevelSelectManager);
                InvokeEventOnSelectedProfile(playerProfile);
                //ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs((YeePlayerProfile)playerProfile));
            }
        }

        private void SelectPlayerProfile(object sender, ProfileTesterEventArgs e)
        {
            if (_profileCalculator is TopdownYeeProfileCalculator yeeProfileCalculator)
            {
                foreach (var formAnsweredArgs in e.Answers)
                {
                    var playerProfile = yeeProfileCalculator.CreateProfileFromFormAnswers(formAnsweredArgs.AnswerValue, generatorSettings);
                    InvokeEventOnSelectedProfile(playerProfile);
                }
            }
        }

        private void SelectPlayerProfile(object sender, EventArgs eventArgs)
        {
            if (_profileCalculator is TopdownYeeProfileCalculator yeeProfileCalculator)
            {
                YeePlayerProfile playerProfile = _playerDataController.CurrentPlayer.SerializedData.PlayerProfile;
                if (!ExperimentController.UseFixedProfile)
                {
                    playerProfile = (YeePlayerProfile)yeeProfileCalculator.CreateProfileFromGameplay(_playerDataController.CurrentPlayer, _playerDataController.CurrentPlayer.CurrentDungeon);
                }
                InvokeEventOnSelectedProfile(playerProfile);
                GameplayProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            }
        }
    }
}