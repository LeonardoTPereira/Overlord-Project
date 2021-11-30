
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Game.DataCollection;
using Game.EnemyGenerator;
using Game.Events;
using Game.GameManager;
using Game.Maestro;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.NpcRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using LevelGenerator;
using MyBox;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    [RequireComponent(typeof(EnemyGeneratorManager), typeof(LevelGeneratorManager))]
    public class QuestGeneratorManager : MonoBehaviour
    {
        [MustBeAssigned, SerializeReference, SerializeField]
        private PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLinesDictionarySo;
        
        public static event ProfileSelectedEvent ProfileSelectedEventHandler;
        public static event CreateEADungeonEvent CreateEaDungeonEventHandler;

        [SerializeReference, SerializeField]
        private QuestLineList _questLines;

        public bool createNarrative = false;

        public bool isFinished = true; //verifica se a missão já terminou
        
        [SerializeField] private FormQuestionsData preTestQuestionnaire;
        private EnemyGeneratorManager _enemyGeneratorManager;
        private LevelGeneratorManager _levelGeneratorManager;

        
        public List<NpcSO> PlaceholderNpcs => _placeholderNpcs;
        public TreasureRuntimeSetSO PlaceholderItems => _placeholderItems;

        [SerializeField, MustBeAssigned] private List<NpcSO> _placeholderNpcs;
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
            PlayerProfile playerProfile = Selector.SelectProfile(e);
            Debug.Log("Profile For Narrative: "+playerProfile.PlayerProfileEnum);
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

        private void SelectPlayerProfile(object sender, FormAnsweredEventArgs e)
        {
            var answers = e.AnswerValue;

            Debug.Log("Answers: " + answers.Count);

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
            Debug.Log("Starting QuestGeneratorManager");
        }

        private IEnumerator CreateNarrative(PlayerProfile playerProfile)
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
                _questLines.SaveAsAsset(playerProfile.PlayerProfileEnum.ToString());
                playerProfileToQuestLinesDictionarySo.QuestLinesForProfile.Add(playerProfile.PlayerProfileEnum.ToString(), _questLines);
            }
            _enemyGeneratorManager = GetComponent<EnemyGeneratorManager>();
            _levelGeneratorManager = GetComponent<LevelGeneratorManager>();
                    
            MakeBranches();
                    
            CreateGeneratorParametersForQuestline(playerProfile);

            Quests.EnemySos = _enemyGeneratorManager.EvolveEnemies(Quests.EnemyParametersForQuestLine.Difficulty);
            // Start the generation process
            _levelGeneratorManager.EvolveDungeonPopulation(this, new CreateEADungeonEventArgs(Quests));
            while (!_levelGeneratorManager.hasFinished)
            {
                yield return null;
            }
            Debug.Log("Continuing Quest Generator");
            //CreateEaDungeonEventHandler?.Invoke(this, new CreateEADungeonEventArgs(Quests));
            //TODO create NPC RuntimeSet
            //TODO create these procedurally
            Quests.NpcSos = PlaceholderNpcs;
            Quests.ItemSos = new List<ItemSo>(PlaceholderItems.Items);
            Quests.CreateAsset(playerProfile.PlayerProfileEnum);
            _questLines.AddQuestLine(Quests);

#if UNITY_EDITOR
            EditorUtility.SetDirty(_questLines);
            AssetDatabase.SaveAssetIfDirty(_questLines);
            
            EditorUtility.SetDirty(playerProfileToQuestLinesDictionarySo);
            AssetDatabase.SaveAssetIfDirty(playerProfileToQuestLinesDictionarySo);
#endif
            ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
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

        private void MakeBranches()
        {
            var index = 0;
            Debug.Log("NQuests: "+ Quests.graph.Count);
            /*while (index < Quests.graph.Count)
            {
                var b = Random.Range(0, 100);
                var currentQuest = Quests.graph[index];
                if (b % 2 == 0)
                {
                    var childIndex = Random.Range(index + 1, Quests.graph.Count);
                    var nextWhenSuccess = Quests.graph[childIndex];
                    if (childIndex < Quests.graph.Count)
                    {
                        nextWhenSuccess.Previous = currentQuest;
                    }
                    childIndex = Random.Range(index + 1, Quests.graph.Count);
                    var nextWhenFailure = Quests.graph[childIndex];
                    if (childIndex < Quests.graph.Count)
                    {
                        nextWhenFailure.Previous = currentQuest;
                    }

                    currentQuest.NextWhenSuccess = nextWhenSuccess;
                    if (nextWhenSuccess != nextWhenFailure)
                    {
                        currentQuest.NextWhenFailure = nextWhenFailure;
                    }
                }
                else if (index + 1 < Quests.graph.Count && Quests.graph[index + 1].Previous == null)
                {
                    Quests.graph[index + 1].Previous = currentQuest;
                    Quests.graph[index].NextWhenSuccess = Quests.graph[index + 1];
                }
                index++;
            }*/
        }
    }
}