
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.NarrativeGenerator.Quests;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
// using static Assets.Scripts.Game.NarrativeGenerator.LoadText;

namespace Game.NarrativeGenerator
{
    public class Manager : MonoBehaviour
    {
        public static event ProfileSelectedEvent ProfileSelectedEventHandler;

        // [MustBeAssigned]
        [SerializeField] private QuestLineList questLines;

        public bool createNarrative = false;

        public bool isFinished = true; //verifica se a missão já terminou
        
        [SerializeField] private FormQuestionsData preTestQuestionnaire;

        public Selector Selector { get; set; }
        public QuestUI ui;
        public QuestLine Quests { get; set; } // implementar coisas com a questline

        public FormQuestionsData PreTestQuestionnaire
        {
            get => preTestQuestionnaire;
            set => preTestQuestionnaire = value;
        }

        void Awake()
        {
            Quests = new QuestLine();
            Selector = new Selector();
        }

        public void OnEnable()
        {
            NarrativeGenerator.NarrativeCreatorEventHandler += SelectPlayerProfile;
        }

        public void OnDisable()
        {
            NarrativeGenerator.NarrativeCreatorEventHandler -= SelectPlayerProfile;
        }

        private void SelectPlayerProfile(object sender, NarrativeCreatorEventArgs e)
        {
            PlayerProfile playerProfile = Selector.Select(this, e);
            if (createNarrative)
            {
                makeBranches();

                Quests.CreateAsset(playerProfile.PlayerProfileEnum);

                questLines.AddQuestLine(Quests);

                for (int i = 0; i < Quests.graph.Count; i++)
                    Debug.Log(Quests.graph[i].name + ", " + Quests.graph[i].NextWhenSuccess + ", " + Quests.graph[i].NextWhenFailure);
            }

            ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
        }

        void Start()
        {
            PlayerProfile playerProfile;
            List<int> answers = new List<int>();
            //teste
            if ( questLines == null )
            {
                questLines = ScriptableObject.CreateInstance<QuestLineList>();
            }
            // makeBranches();
            //
            if (PreTestQuestionnaire != null)
            {
                foreach (FormQuestionData questionData in PreTestQuestionnaire.questions)
                    answers.Add(questionData.answer);

                Debug.Log("Answers: " + answers.Count);

                playerProfile = Selector.Select(this, answers);

                if (createNarrative)
                {
                    makeBranches();

                    // writer.writeJSon(Quests, playerProfile);

                    // for (int i = 0; i < Quests.graph.Count; i++)
                    //     Debug.Log(Quests.graph[i].Tipo + ", " + Quests.graph[i].c1 + ", " + Quests.graph[i].c2);
                    // leo
                    Quests.CreateAsset(playerProfile.PlayerProfileEnum);
                    
                    questLines.AddQuestLine(Quests);

                    for (int i = 0; i < Quests.graph.Count; i++)
                        Debug.Log(Quests.graph[i].name + ", " + Quests.graph[i].NextWhenSuccess + ", " + Quests.graph[i].NextWhenFailure);
                }

                ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            }
        }

        void makeBranches()
        {
            for (int i = 0; i < 10; i++)
            {
                Selector.DrawMissions( this );
                QuestLine questLine = new QuestLine();// create instance
                //dar merge
                questLine.graph = Quests.graph;
                questLines.AddQuestLine( questLine );
            }
            /// leo
            // int index = 0, b;
            // QuestSO nextWhenSuccess;
            // QuestSO nextWhenFailure;

            // Quests.graph[index].Previous = null;

            // while (index < Quests.graph.Count)
            // {
            //     Debug.Log("entrou");
            //     b = Random.Range(0, 100);
            //     QuestSO currentQuest = Quests.graph[index];
            //     if (b % 2 == 0)
            //     {
            //         int childIndex = Random.Range(index + 1, Quests.graph.Count);
            //         nextWhenSuccess = Quests.graph[childIndex];
            //         if (childIndex < Quests.graph.Count)
            //         {
            //             nextWhenSuccess.Previous = currentQuest;
            //         }
            //         childIndex = Random.Range(index + 1, Quests.graph.Count);
            //         nextWhenFailure = Quests.graph[childIndex];
            //         if (childIndex < Quests.graph.Count)
            //         {
            //             nextWhenFailure.Previous = currentQuest;
            //         }

            //         currentQuest.NextWhenSuccess = nextWhenSuccess;
            //         if (nextWhenSuccess != nextWhenFailure)
            //         {
            //             currentQuest.NextWhenFailure = nextWhenFailure;
            //         }
            //     }
            //     else if ((index + 1) < Quests.graph.Count && Quests.graph[index + 1].Previous == null)
            //     {
            //         Quests.graph[index + 1].Previous = currentQuest;
            //         Quests.graph[index].NextWhenSuccess = Quests.graph[index + 1];
            //     }
            //     index++;
            // }
            ui.CreateQuestList( questLines );
        }
    }
}