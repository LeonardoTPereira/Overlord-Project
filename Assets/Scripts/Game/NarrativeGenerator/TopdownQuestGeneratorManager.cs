//TODO: Organizar os scripts relacionados abaixo em um numero menor de namespaces
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Events;
using Game.LevelGenerator;
using Overlord.LevelGenerator.LevelSOs;
using Game.LevelSelection;
using Game.Maestro;
using Game.NarrativeGenerator;
using Overlord.NarrativeGenerator.EnemyRelatedNarrative;
using Overlord.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;
using Overlord.ProfileAnalyst;
using Overlord.NarrativeGenerator;
using Topdown.Overlord.RulesGenerator.EnemyGeneration;
using Topdown.Overlord.ProfileAnalyst;
using Overlord.Maestro.ExperimentControllers;

namespace Topdown.Overlord.NarrativeGenerator
{
    [RequireComponent(typeof(TopdownPlayerProfileManager), typeof(TopdownEnemyGeneratorManager), typeof(LevelGeneratorManager))]
    public class TopdownQuestGeneratorManager : QuestGeneratorManager
    {
        [field: SerializeField, MustBeAssigned] public GeneratorSettings CurrentGeneratorSettings { get; set; }
        [field: SerializeField, MustBeAssigned] public SelectedLevels SelectedLevels { get; set; }

        public static event ProfileSelectedEvent ProfileSelectedEventHandler;       // Topdown  event
        public static event QuestLineCreatedEvent QuestLineCreatedEventHandler;     // Topdown  event
        public static event ProfileSelectedEvent FixedLevelProfileEventHandler;     // Topdown  event

        [MustBeAssigned, SerializeReference, SerializeField]
        private PlayerProfileToQuestLinesDictionarySo _playerProfileToQuestLines;

        [SerializeReference, SerializeField] private QuestLineList questLines;

        private TopdownEnemyGeneratorManager _enemyGeneratorManager;
        private LevelGeneratorManager _levelGeneratorManager;
                
        private void Start()
        {
            _enemyGeneratorManager = GetComponent<TopdownEnemyGeneratorManager>();
            _levelGeneratorManager = GetComponent<LevelGeneratorManager>();
        }

        protected override async void HandleProfileSelected(IPlayerProfile profile)
        {
            if (profile is YeePlayerProfile yeeProfile)
            {
                if (yeeProfile.IsFixedFromExperiment || MustCreateNarrative)
                {
                    questLines = TopdownQuestSelector.CreateMissions(_narrativeSettings, language);
                    await CreateNarrative(yeeProfile);
                }
                else
                {
                    ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(yeeProfile));
                }
            }
        }

        protected override async Task CreateNarrative(YeePlayerProfile playerProfile)
        {
            CreateGeneratorParametersForQuestLine(playerProfile);
            questLines.TargetProfile = playerProfile;
            await CreateContentsForQuestLine();
#if UNITY_EDITOR
            if (!CurrentGeneratorSettings.GenerateInRealTime)
            {
                var narrativeExperimentRepository = new NarrativeExperimentRepository(playerProfile, _playerProfileToQuestLines, _narrativeSettings, language);
                narrativeExperimentRepository.Save(questLines, playerProfile.PlayerProfileEnum.ToString());
            }
#endif
            SelectedLevels.Init(questLines);
            FixedLevelProfileEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            QuestLineCreatedEventHandler?.Invoke(this, new QuestLineCreatedEventArgs(questLines));
        }

        private async Task CreateContentsForQuestLine()
        {
            questLines.EnemySos = _enemyGeneratorManager.GetEnemySOList(questLines.EnemyParametersForQuestLines.Difficulty);
            questLines.NpcSos = _narrativeSettings.PlaceholderNpcs;
            questLines.ItemSos = new List<ItemSo>(_narrativeSettings.PlaceholderItems.Items);
            questLines.DungeonFileSos = await CreateDungeonsForQuestLine();
        }

        private async Task<List<DungeonFileSo>> CreateDungeonsForQuestLine()
        {
            return await _levelGeneratorManager.EvolveDungeonPopulation(new CreateEaDungeonEventArgs(questLines, 
                CurrentGeneratorSettings.DungeonParameters, CurrentGeneratorSettings.TotalRunsOfEA));
        }

        private void CreateGeneratorParametersForQuestLine(YeePlayerProfile playerProfile)
        {
            questLines.DungeonParametersForQuestLines = new QuestDungeonsParameters();
            questLines.EnemyParametersForQuestLines = new QuestEnemiesParameters();
            //questLines.NpcParametersForQuestLines = new QuestNpcsParameters();
            questLines.ItemParametersForQuestLines = new QuestItemsParameters();
            questLines.CalculateDifficultyFromProfile(playerProfile.MasteryPreference);
#if UNITY_EDITOR
            Debug.Log("Profile: " + playerProfile);
#endif
            questLines.CalculateMonsterFromQuests();
            questLines.CalculateDungeonParametersFromQuests(playerProfile.CreativityPreference
                , playerProfile.AchievementPreference);
            //questLines.CalculateNpcsFromQuests();
            questLines.CalculateItemsFromQuests();
        }
    }
}