using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.DataCollection;
using Game.EnemyGenerator;
using Game.Events;
using Game.LevelGenerator;
using Game.LevelGenerator.LevelSOs;
using Game.LevelSelection;
using Game.Maestro;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.NpcRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using MyBox;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator
{
    [RequireComponent(typeof(EnemyGeneratorManager), typeof(LevelGeneratorManager))]
    public class QuestGeneratorManager : MonoBehaviour
    {
        [MustBeAssigned, SerializeReference, SerializeField]
        private PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLines;
        public static event ProfileSelectedEvent ProfileSelectedEventHandler;
        public static event QuestLineCreatedEvent QuestLineCreatedEventHandler;
        [SerializeReference, SerializeField] private QuestLineList questLines;
        [field:SerializeField] public bool MustCreateNarrative { get; set; } = false;
        private bool isRealTimeGeneration;
        [SerializeField] private FormQuestionsData preTestQuestionnaire;
        private EnemyGeneratorManager _enemyGeneratorManager;
        private LevelGeneratorManager _levelGeneratorManager;
        public List<NpcSo> PlaceholderNpcs => placeholderNpcs;
        public TreasureRuntimeSetSO PlaceholderItems => placeholderItems;
        [SerializeField, MustBeAssigned] private List<NpcSo> placeholderNpcs;
        [SerializeField, MustBeAssigned] private TreasureRuntimeSetSO placeholderItems;
        [SerializeField, MustBeAssigned] private WeaponTypeRuntimeSetSO possibleWeapons;
        public WeaponTypeRuntimeSetSO PossibleWeapons => possibleWeapons;
        public Selector Selector { get; set; }
        public QuestLine Quests { get; set; }
        [field: SerializeField, MustBeAssigned] public SelectedLevels SelectedLevels { get; set; }
        [field: SerializeField, MustBeAssigned] public PlayerDataController CurrentPlayerDataController {get; set; }
        [field: SerializeField, MustBeAssigned] public DungeonDataController CurrentDungeonDataController {get; set; }

        public FormQuestionsData PreTestQuestionnaire
        {
            get => preTestQuestionnaire;
            set => preTestQuestionnaire = value;
        }

        private void Awake()
        {
            Quests = ScriptableObject.CreateInstance<QuestLine>();
            InitSelector();
        }

        private void InitSelector ()
        {
            Selector = new Selector();
        }

        public void OnEnable()
        {
            NarrativeGenerator.NarrativeCreatorEventHandler += SelectPlayerProfile;
            FormBhv.PreTestFormQuestionAnsweredEventHandler += SelectPlayerProfile;
            LevelSelectManager.CompletedAllLevelsEventHandler += SelectPlayerProfile;
        }

        public void OnDisable()
        {
            NarrativeGenerator.NarrativeCreatorEventHandler -= SelectPlayerProfile;
            FormBhv.PreTestFormQuestionAnsweredEventHandler -= SelectPlayerProfile;
            LevelSelectManager.CompletedAllLevelsEventHandler -= SelectPlayerProfile;
        }

        private async void SelectPlayerProfile(object sender, NarrativeCreatorEventArgs e)
        {
            var playerProfile = ProfileCalculator.CreateProfile(e);
            isRealTimeGeneration = false;
            await CreateOrLoadNarrativeForProfile(playerProfile);
        }

        private async void SelectPlayerProfile(object sender, FormAnsweredEventArgs e)
        {
            var playerProfile = ProfileCalculator.CreateProfile(e.AnswerValue);
            isRealTimeGeneration = true;
            await CreateOrLoadNarrativeForProfile(playerProfile);
        }
        
        private async void SelectPlayerProfile(object sender, EventArgs eventArgs)
        {
            
            var playerProfile = ProfileCalculator.CreateProfile(CurrentPlayerDataController.CurrentPlayer, CurrentDungeonDataController.CurrentDungeon);
            isRealTimeGeneration = true;
            await CreateOrLoadNarrativeForProfile(playerProfile);
        }

        private async Task CreateOrLoadNarrativeForProfile(PlayerProfile playerProfile)
        {
            if (MustCreateNarrative)
            {
                Selector.CreateMissions(this);
                await CreateNarrative(playerProfile);
            }
            else
            {
                ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            }
        }

        private void Start()
        {
            Quests.Init();
            _enemyGeneratorManager = GetComponent<EnemyGeneratorManager>();
            _levelGeneratorManager = GetComponent<LevelGeneratorManager>();
        }

        private async Task CreateNarrative(PlayerProfile playerProfile)
        {
            Debug.Log("Creating Quest Line for Profile");
            SetQuestLineListForProfile(playerProfile);
            CreateGeneratorParametersForQuestLine(playerProfile);
            Debug.Log("Creating Contents for Quest Line");
            await CreateContentsForQuestLine();
            if (!isRealTimeGeneration)
            {
                SaveSOs(playerProfile.PlayerProfileEnum.ToString());
            }
            Debug.Log("Initializing Quest Content");
            SelectedLevels.Init(Quests);
            ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            QuestLineCreatedEventHandler?.Invoke(this, new QuestLineCreatedEventArgs(Quests));
        }

        private async Task CreateContentsForQuestLine()
        {
            Debug.Log("Creating Enemies");
            Quests.EnemySos = _enemyGeneratorManager.EvolveEnemies(Quests.EnemyParametersForQuestLine.Difficulty);
            Quests.NpcSos = PlaceholderNpcs;
            Quests.ItemSos = new List<ItemSo>(PlaceholderItems.Items);
            Debug.Log("Creating Dungeons");
            Quests.DungeonFileSos = await CreateDungeonsForQuestLine();
        }

        private async Task<List<DungeonFileSo>> CreateDungeonsForQuestLine()
        {
            return await _levelGeneratorManager.EvolveDungeonPopulation(new CreateEADungeonEventArgs(Quests));
            //Quests.DungeonFileSos = LevelSelector.FilterLevels(Quests.DungeonFileSos);
        }

        private void SaveSOs(string profileName)
        {
#if UNITY_EDITOR
            // Build the target path
            var target = "Assets";
            target += Constants.SEPARATOR_CHARACTER + "Resources";
            target += Constants.SEPARATOR_CHARACTER + "Experiment";
            var questLineFile = target + Constants.SEPARATOR_CHARACTER + profileName;
            questLines.SaveAsset(questLineFile);
            var newFolder = Constants.SEPARATOR_CHARACTER + profileName;
            if (!AssetDatabase.IsValidFolder(target + newFolder))
            {
                AssetDatabase.CreateFolder(target, newFolder);
            }
            target += Constants.SEPARATOR_CHARACTER + newFolder;
            Quests.SaveAsset(target);
            questLines.AddQuestLine(Quests);

            EditorUtility.SetDirty(questLines);
            AssetDatabase.SaveAssetIfDirty(questLines);

            EditorUtility.SetDirty(playerProfileToQuestLines);
            AssetDatabase.SaveAssetIfDirty(playerProfileToQuestLines);
#endif
        }

        private void SetQuestLineListForProfile(PlayerProfile playerProfile)
        {
            if (playerProfileToQuestLines.QuestLinesForProfile.TryGetValue(
                    playerProfile.PlayerProfileEnum.ToString(), out var questLinesForProfile))
            {
                questLines = questLinesForProfile;
            }
            else
            {
                questLines = ScriptableObject.CreateInstance<QuestLineList>();
                questLines.QuestLinesList = new List<QuestLine>();
                playerProfileToQuestLines.QuestLinesForProfile.Add(playerProfile.PlayerProfileEnum.ToString(), questLines);
            }
        }

        private void CreateGeneratorParametersForQuestLine(PlayerProfile playerProfile)
        {
            Quests.DungeonParametersForQuestLine = new QuestDungeonsParameters();
            Quests.EnemyParametersForQuestLine = new QuestEnemiesParameters();
            Quests.NpcParametersForQuestLine = new QuestNpcsParameters();
            Quests.ItemParametersForQuestLine = new QuestItemsParameters();
            Quests.DungeonParametersForQuestLine.CalculateDungeonParametersFromQuests(Quests, playerProfile.CreativityPreference);
            Quests.EnemyParametersForQuestLine.CalculateMonsterFromQuests(Quests);
            Quests.EnemyParametersForQuestLine.CalculateDifficultyFromProfile(playerProfile);
            Quests.NpcParametersForQuestLine.CalculateNpcsFromQuests(Quests);
            Quests.ItemParametersForQuestLine.CalculateItemsFromQuests(Quests);
        }
    }
}
