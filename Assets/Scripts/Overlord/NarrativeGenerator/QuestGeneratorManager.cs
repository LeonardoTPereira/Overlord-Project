using Game.ExperimentControllers;
using MyBox;
using Overlord.ProfileAnalyst;
using Overlord.RulesGenerator.EnemyGeneration;
using System.Threading.Tasks;
using UnityEngine;
using Util;
using static Util.Enums;

namespace Overlord.NarrativeGenerator
{
    [RequireComponent(typeof(PlayerProfileManager), typeof(EnemyGeneratorManager)/*, typeof(LevelGeneratorManager)*/)]
    public class QuestGeneratorManager : MonoBehaviour
    {
        [field: SerializeField] public bool MustCreateNarrative { get; set; }
        [SerializeField]
        public Language language = Language.Portuguese;

        public void OnEnable()
        {
            PlayerProfileManager.ProfileSelected += HandleProfileSelected;
        }

        public void OnDisable()
        {
            PlayerProfileManager.ProfileSelected -= HandleProfileSelected;
        }

        // Event handler for when a player profile is selected
        // Put here anything that should happen when a profile is selected
        protected virtual async void HandleProfileSelected(IPlayerProfile profile)
        {
            /*
            if (profile is YeePlayerProfile yeeProfile)
            {
                if (yeeProfile.IsFixedFromExperiment || MustCreateNarrative)
                {
                    questLines = Selector.CreateMissions(CurrentGeneratorSettings);
                    await CreateNarrative(yeeProfile);
                }
                else
                {
                    ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(yeeProfile));
                }
            }
            */
        }

        protected virtual async Task CreateNarrative(YeePlayerProfile playerProfile)
        {
            /*
            CreateGeneratorParametersForQuestLine(playerProfile);
            questLines.TargetProfile = playerProfile;
            await CreateContentsForQuestLine();
#if UNITY_EDITOR
            if (!CurrentGeneratorSettings.GenerateInRealTime)
            {
                var narrativeExperimentRepository = new NarrativeExperimentRepository(playerProfile, _playerProfileToQuestLines, CurrentGeneratorSettings);
                narrativeExperimentRepository.Save(questLines, playerProfile.PlayerProfileEnum.ToString());
            }
#endif
            SelectedLevels.Init(questLines);
            FixedLevelProfileEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            QuestLineCreatedEventHandler?.Invoke(this, new QuestLineCreatedEventArgs(questLines));
            */
        }
    }
}
