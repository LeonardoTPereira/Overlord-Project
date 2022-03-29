
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Game.DataCollection;
using Game.EnemyGenerator;
using Game.Events;
using Game.GameManager;
using Game.LevelGenerator;
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
using UnityEngine.SceneManagement;

namespace Game.NarrativeGenerator
{
    [RequireComponent(typeof(EnemyGeneratorManager), typeof(LevelGeneratorManager))]
    public class QuestGeneratorManager : MonoBehaviour
    {
        [MustBeAssigned, SerializeReference, SerializeField]
        private PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLinesDictionarySo;
        
        public static event ProfileSelectedEvent ProfileSelectedEventHandler;
        public static event QuestLineCreatedEvent QuestLineCreatedEventHandler;

        [SerializeReference, SerializeField]
        private QuestLineList _questLines;

        public bool createNarrative = false;

        public bool isFinished = true; //verifica se a missão já terminou
        
        [SerializeField] private FormQuestionsData preTestQuestionnaire;
        private EnemyGeneratorManager _enemyGeneratorManager;
        private LevelGeneratorManager _levelGeneratorManager;

        
        public List<NpcSo> PlaceholderNpcs => _placeholderNpcs;
        public TreasureRuntimeSetSO PlaceholderItems => _placeholderItems;

        [SerializeField, MustBeAssigned] private List<NpcSo> _placeholderNpcs;
        [SerializeField, MustBeAssigned] private TreasureRuntimeSetSO _placeholderItems;
        [SerializeField, MustBeAssigned] private WeaponTypeRuntimeSetSO _possibleWeapons;

        public WeaponTypeRuntimeSetSO PossibleWeapons => _possibleWeapons;
        public Selector Selector { get; set; }
        public QuestLine Quests { get; set; }

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
            if (createNarrative)
            {
                Selector.CreateMissions(this);
                CreateNarrative(playerProfile);
            }
            else
            {
                ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            }
        }

        private void SelectPlayerProfile(object sender, FormAnsweredEventArgs e)
        {
            var answers = e.AnswerValue;
            
            var playerProfile = Selector.SelectProfile(answers);
            if (createNarrative)
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
            var startTime = Time.realtimeSinceStartup;
            SetQuestLineListForProfile(playerProfile);
            CreateGeneratorParametersForQuestline(playerProfile);
            yield return StartCoroutine(CreateContentsForQuestLine());
            //Quests.CreateAsset(playerProfile.PlayerProfileEnum);
            //_questLines.AddQuestLine(Quests);
            //SaveSOs();
            Debug.Log("Created Contents");
            Debug.Log(Quests.DungeonFileSos.Count);
            Debug.Log(Quests.EnemySos.Count);
            var endTime = Time.realtimeSinceStartup;
            Debug.Log("Time taken: "+(endTime-startTime));
            ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            QuestLineCreatedEventHandler?.Invoke(this, new QuestLineCreatedEventArgs(Quests));
            Debug.Log("Created Narratives");
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

        private void SaveSOs()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(_questLines);
            AssetDatabase.SaveAssetIfDirty(_questLines);

            EditorUtility.SetDirty(playerProfileToQuestLinesDictionarySo);
            AssetDatabase.SaveAssetIfDirty(playerProfileToQuestLinesDictionarySo);
#endif
        }

        private void SetQuestLineListForProfile(PlayerProfile playerProfile)
        {
            if (playerProfileToQuestLinesDictionarySo.QuestLinesForProfile.TryGetValue(
                    playerProfile.PlayerProfileEnum.ToString(), out var questLines))
            {
                _questLines = questLines;
            }
            else
            {
                _questLines = ScriptableObject.CreateInstance<QuestLineList>();
                _questLines.QuestLinesList = new List<QuestLine>();
                //_questLines.SaveAsAsset(playerProfile.PlayerProfileEnum.ToString());
                playerProfileToQuestLinesDictionarySo.QuestLinesForProfile.Add(playerProfile.PlayerProfileEnum.ToString(),
                    _questLines);
            }
        }

        private void CreateGeneratorParametersForQuestline(PlayerProfile playerProfile)
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
