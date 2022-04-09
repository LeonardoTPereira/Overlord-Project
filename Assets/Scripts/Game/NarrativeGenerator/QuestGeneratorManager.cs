
using System;
using System.Collections;
using System.Collections.Generic;
using Game.DataCollection;
using Game.EnemyGenerator;
using Game.Events;
using Game.LevelGenerator;
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

        public FormQuestionsData PreTestQuestionnaire
        {
            get => preTestQuestionnaire;
            set => preTestQuestionnaire = value;
        }

        private void Awake()
        {
            Quests = ScriptableObject.CreateInstance<QuestLine>();
            Selector = new Selector();
        }

        public void OnEnable()
        {
            NarrativeGenerator.NarrativeCreatorEventHandler += SelectPlayerProfile;
            FormBHV.PreTestFormQuestionAnsweredEventHandler += SelectPlayerProfile;
        }

        public void OnDisable()
        {
            NarrativeGenerator.NarrativeCreatorEventHandler -= SelectPlayerProfile;
            FormBHV.PreTestFormQuestionAnsweredEventHandler -= SelectPlayerProfile;
        }

        private void SelectPlayerProfile(object sender, NarrativeCreatorEventArgs e)
        {
            var playerProfile = Selector.SelectProfile(e);
            isRealTimeGeneration = false;
            CreateOrLoadNarrativeForProfile(playerProfile);
        }

        private void SelectPlayerProfile(object sender, FormAnsweredEventArgs e)
        {
            var answers = e.AnswerValue;
            var playerProfile = Selector.SelectProfile(answers);
            isRealTimeGeneration = true;
            CreateOrLoadNarrativeForProfile(playerProfile);
        }

        private void CreateOrLoadNarrativeForProfile(PlayerProfile playerProfile)
        {
            if (MustCreateNarrative)
            {
                Selector.CreateMissions(this);
                StartCoroutine(CreateNarrative(playerProfile));
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

        private IEnumerator CreateNarrative(PlayerProfile playerProfile)
        {
            SetQuestLineListForProfile(playerProfile);
            CreateGeneratorParametersForQuestLine(playerProfile);
            yield return StartCoroutine(CreateContentsForQuestLine());
            if (!isRealTimeGeneration)
            {
                SaveSOs(playerProfile.PlayerProfileEnum.ToString());
            }
            EliteSelector.SelectEliteForEachLevel(Quests, SelectedLevels);
            ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            QuestLineCreatedEventHandler?.Invoke(this, new QuestLineCreatedEventArgs(Quests));
        }

        private IEnumerator CreateContentsForQuestLine()
        {
            Quests.EnemySos = _enemyGeneratorManager.EvolveEnemies(Quests.EnemyParametersForQuestLine.Difficulty);
            Quests.ItemSos = new List<ItemSo>(PlaceholderItems.Items);
            yield return StartCoroutine(CreateDungeonsForQuestLine());
        }

        private IEnumerator CreateDungeonsForQuestLine()
        {
            Debug.Log("Creating Dungeons");
            _levelGeneratorManager.EvolveDungeonPopulation(this, new CreateEADungeonEventArgs(Quests));
            yield return new WaitUntil(() => _levelGeneratorManager.hasFinished);
            Debug.Log("Created Dungeons");
            Quests.DungeonFileSos = LevelSelector.FilterLevels(Quests.DungeonFileSos);
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
