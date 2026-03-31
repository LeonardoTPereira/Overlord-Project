using MyBox;
using Overlord.Events;
using Overlord.LevelGenerator.LevelSOs;
using Overlord.LevelGenerator.Manager;
using Overlord.NarrativeGenerator.EnemyRelatedNarrative;
using Overlord.NarrativeGenerator.ItemRelatedNarrative;
using Overlord.NarrativeGenerator.Quests;
using Overlord.ProfileAnalyst;
using Overlord.RulesGenerator.EnemyGeneration;
using ScriptableObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Util;
using static Util.Enums;

namespace Overlord.NarrativeGenerator
{
    [RequireComponent(typeof(PlayerProfileManager), typeof(EnemyGeneratorManager), typeof(LevelGeneratorManager))]
    public class QuestGeneratorManager : MonoBehaviour
    {
        [field: SerializeField] public bool MustCreateNarrative { get; set; }
        [SerializeField] public Language language = Language.Portuguese;

        [DisplayInspector]
        [field: SerializeField] protected NarrativeSettings _narrativeSettings;

        [SerializeReference, SerializeField] public QuestLineList questLines;

        protected EnemyGeneratorManager _enemyGeneratorManager;
        protected LevelGeneratorManager _levelGeneratorManager;

        public void OnEnable()
        {
            PlayerProfileManager.ProfileSelected += HandleProfileSelected;
        }

        public void OnDisable()
        {
            PlayerProfileManager.ProfileSelected -= HandleProfileSelected;
        }

        private void Awake()
        {
            _enemyGeneratorManager = GetComponent<EnemyGeneratorManager>();
            _levelGeneratorManager = GetComponent<LevelGeneratorManager>();
        }

        // Event handler for when a player profile is selected
        // Put here anything that should happen when a profile is selected
        protected virtual async void HandleProfileSelected(IPlayerProfile profile)
        {
            if (profile is YeePlayerProfile yeeProfile)
            {
                if (yeeProfile.IsFixedFromExperiment || MustCreateNarrative)
                {                    
                    questLines = QuestSelector.CreateMissions(_narrativeSettings, language);
                    await CreateNarrative(yeeProfile);
                }
            }
        }

        protected virtual async Task CreateNarrative(YeePlayerProfile playerProfile)
        {
            CreateGeneratorParametersForQuestLine(playerProfile);
            questLines.TargetProfile = playerProfile;
            await CreateContentsForQuestLine();
        }

        protected async Task CreateContentsForQuestLine()
        {
            questLines.EnemySos = _enemyGeneratorManager.GetEnemySOList(questLines.EnemyParametersForQuestLines.Difficulty);
            questLines.NpcSos = _narrativeSettings.PlaceholderNpcs;
            questLines.ItemSos = new List<ItemSo>(_narrativeSettings.PlaceholderItems.Items);
            questLines.DungeonFileSos = await CreateDungeonsForQuestLine();
        }

        protected async Task<List<DungeonFileSo>> CreateDungeonsForQuestLine()
        {
            return await _levelGeneratorManager.EvolveDungeonPopulation(new CreateEaDungeonEventArgs(questLines,
                _levelGeneratorManager.GeneticAlgorithmSettings, _levelGeneratorManager.GeneticAlgorithmSettings.TotalRunsOfEA));
        }

        protected void CreateGeneratorParametersForQuestLine(YeePlayerProfile playerProfile)
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
