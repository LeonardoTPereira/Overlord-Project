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
using Overlord.Maestro.ExperimentControllers;

namespace Topdown.Overlord.ProfileAnalyst
{
    public class TopdownPlayerProfileManager : PlayerProfileManager
    {
        public bool IsUsingManualPlayerProfileSO = false;

        [field: SerializeField, MustBeAssigned] private PlayerDataController _playerDataController;
        [field: SerializeField, MustBeAssigned] private DungeonDataController dungeonDataController;
        [field: SerializeField, MustBeAssigned] private GeneratorSettings generatorSettings;
        public static event ProfileSelectedEvent GameplayProfileSelectedEventHandler;

        protected new IPlayerProfileCalculator _profileCalculator = new TopdownYeeProfileCalculator();        // Change it with another player profile calculator if needed

        private void OnEnable()
        {
            if (IsUsingManualPlayerProfileSO)
                _profileCalculator = new YeeProfileCalculator();

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
            if (IsUsingManualPlayerProfileSO)
            {
                SetPlayerProfileFromManualPlayerProfileSO();
                return;
            }
            if (_profileCalculator is TopdownYeeProfileCalculator yeeProfileCalculator)
            {
                var playerProfile = yeeProfileCalculator.CreateProfileFromNarrative(e);
                InvokeEventOnSelectedProfile(playerProfile);
            }
        }

        private void SelectPlayerProfile(object sender, FormAnsweredEventArgs e)
        {
            if (IsUsingManualPlayerProfileSO)
            {
                SetPlayerProfileFromManualPlayerProfileSO();
                return;
            }
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
            if (IsUsingManualPlayerProfileSO)
            {
                SetPlayerProfileFromManualPlayerProfileSO();
                return;
            }
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
            if (IsUsingManualPlayerProfileSO)
            {
                SetPlayerProfileFromManualPlayerProfileSO();
                return;
            }
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