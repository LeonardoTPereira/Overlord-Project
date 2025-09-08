//TODO: Organizar os scripts relacionados abaixo em um numero menor de namespaces
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.EnemyGenerator;
using Game.Events;
using Game.ExperimentControllers;
using Game.LevelGenerator;
using Game.LevelGenerator.LevelSOs;
using Game.LevelSelection;
using Game.Maestro;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;
using Overlord.ProfileAnalyst;

namespace Game.NarrativeGenerator
{
    [RequireComponent(typeof(PlayerProfileManager), typeof(EnemyGeneratorManager), typeof(LevelGeneratorManager))]
    public class QuestGeneratorManager : MonoBehaviour
    {
        [MustBeAssigned, SerializeReference, SerializeField]
        private PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLines;
        public static event ProfileSelectedEvent ProfileSelectedEventHandler;
        public static event QuestLineCreatedEvent QuestLineCreatedEventHandler;

        [SerializeReference, SerializeField] private QuestLineList questLines;
        private List<QuestLineList> _questLinesForProfile;

        [field:SerializeField] public bool MustCreateNarrative { get; set; }
        private EnemyGeneratorManager _enemyGeneratorManager;
        private LevelGeneratorManager _levelGeneratorManager;

        [field: SerializeField, MustBeAssigned] public SelectedLevels SelectedLevels { get; set; }
        [field: SerializeField, MustBeAssigned] public GeneratorSettings CurrentGeneratorSettings { get; set; }

        public static event ProfileSelectedEvent FixedLevelProfileEventHandler;

        public void OnEnable()
        {
            PlayerProfileManager.ProfileSelected += HandleProfileSelected;
        }

        public void OnDisable()
        {
            PlayerProfileManager.ProfileSelected -= HandleProfileSelected;
        }

        private async void HandleProfileSelected(IPlayerProfile profile)
        {
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
        }

        private void Start()
        {
            _enemyGeneratorManager = GetComponent<EnemyGeneratorManager>();
            _levelGeneratorManager = GetComponent<LevelGeneratorManager>();
        }

        private async Task CreateNarrative(YeePlayerProfile playerProfile)
        {
            SetQuestLineListForProfile(playerProfile);
            CreateGeneratorParametersForQuestLine(playerProfile);
            questLines.TargetProfile = playerProfile;
            await CreateContentsForQuestLine();
            if (!CurrentGeneratorSettings.GenerateInRealTime)
            {
                SaveSOs(playerProfile.PlayerProfileEnum.ToString());
            }
            SelectedLevels.Init(questLines);
            FixedLevelProfileEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            QuestLineCreatedEventHandler?.Invoke(this, new QuestLineCreatedEventArgs(questLines));
        }

        private async Task CreateContentsForQuestLine()
        {
            questLines.EnemySos = _enemyGeneratorManager.GetEnemyList(questLines.EnemyParametersForQuestLines.Difficulty);
            questLines.NpcSos = CurrentGeneratorSettings.PlaceholderNpcs;
            questLines.ItemSos = new List<ItemSo>(CurrentGeneratorSettings.PlaceholderItems.Items);
            questLines.DungeonFileSos = await CreateDungeonsForQuestLine();
        }

        private async Task<List<DungeonFileSo>> CreateDungeonsForQuestLine()
        {
            return await _levelGeneratorManager.EvolveDungeonPopulation(new CreateEaDungeonEventArgs(questLines, 
                CurrentGeneratorSettings.DungeonParameters, CurrentGeneratorSettings.TotalRunsOfEA));
        }

        private void SaveSOs(string profileName)
        {
#if UNITY_EDITOR
            // TODO check if still works
            var target = "Assets";
            target += Constants.SeparatorCharacter + "Resources";
            target += Constants.SeparatorCharacter + "Experiment";
            var questLineFile = target + Constants.SeparatorCharacter + profileName;
            questLines.SaveAsset(questLineFile);

            EditorUtility.SetDirty(questLines);
            AssetDatabase.SaveAssetIfDirty(questLines);

            EditorUtility.SetDirty(playerProfileToQuestLines);
            AssetDatabase.SaveAssetIfDirty(playerProfileToQuestLines);
#endif
        }

        private void SetQuestLineListForProfile(YeePlayerProfile playerProfile)
        {
            if (playerProfileToQuestLines.QuestLinesForProfile.TryGetValue(
                    playerProfile.PlayerProfileEnum.ToString(), out var questLinesForProfile))
            {
                _questLinesForProfile = questLinesForProfile;
            }
            else
            {
                _questLinesForProfile = new List<QuestLineList>();
                _questLinesForProfile.Add(questLines);
                playerProfileToQuestLines.QuestLinesForProfile.Add(playerProfile.PlayerProfileEnum.ToString(), _questLinesForProfile);
            }
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
