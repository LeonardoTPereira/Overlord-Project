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
        private List<QuestLineList> _questLineListsForProfile;
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
                questLines = Selector.CreateMissions(PlaceholderNpcs, PlaceholderItems, PossibleWeapons);
                await CreateNarrative(playerProfile);
            }
            else
            {
                ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            }
        }

        private void Start()
        {
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
            SelectedLevels.Init(questLines);
            ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            QuestLineCreatedEventHandler?.Invoke(this, new QuestLineCreatedEventArgs(questLines));
        }

        private async Task CreateContentsForQuestLine()
        {
            Debug.Log("Creating Enemies");
            questLines.EnemySos = _enemyGeneratorManager.EvolveEnemies(questLines.EnemyParametersForQuestLines.Difficulty);
            questLines.NpcSos = PlaceholderNpcs;
            questLines.ItemSos = new List<ItemSo>(PlaceholderItems.Items);
            Debug.Log("Creating Dungeons");
            questLines.DungeonFileSos = await CreateDungeonsForQuestLine();
        }

        private async Task<List<DungeonFileSo>> CreateDungeonsForQuestLine()
        {
            return await _levelGeneratorManager.EvolveDungeonPopulation(new CreateEaDungeonEventArgs(questLines));
            //Quests.DungeonFileSos = LevelSelector.FilterLevels(Quests.DungeonFileSos);
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

        private void SetQuestLineListForProfile(PlayerProfile playerProfile)
        {
            if (playerProfileToQuestLines.QuestLinesForProfile.TryGetValue(
                    playerProfile.PlayerProfileEnum.ToString(), out var questLinesForProfile))
            {
                _questLineListsForProfile = questLinesForProfile;
            }
            else
            {
                _questLineListsForProfile = new List<QuestLineList>();
                _questLineListsForProfile.Add(questLines);
                playerProfileToQuestLines.QuestLinesForProfile.Add(playerProfile.PlayerProfileEnum.ToString(), _questLineListsForProfile);
            }
        }

        private void CreateGeneratorParametersForQuestLine(PlayerProfile playerProfile)
        {
            questLines.DungeonParametersForQuestLines = new QuestDungeonsParameters();
            questLines.EnemyParametersForQuestLines = new QuestEnemiesParameters();
            //questLines.NpcParametersForQuestLines = new QuestNpcsParameters();
            questLines.ItemParametersForQuestLines = new QuestItemsParameters();
            questLines.CalculateDifficultyFromProfile(playerProfile);
            questLines.CalculateMonsterFromQuests();
            questLines.CalculateDungeonParametersFromQuests(playerProfile.CreativityPreference);
            //questLines.CalculateNpcsFromQuests();
            questLines.CalculateItemsFromQuests();
        }
    }
}
