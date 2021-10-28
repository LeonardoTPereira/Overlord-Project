
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;
// using static Assets.Scripts.Game.NarrativeGenerator.LoadText;

namespace Game.NarrativeGenerator
{
    public class Manager : MonoBehaviour
    {

        public static event ProfileSelectedEvent ProfileSelectedEventHandler;

        public bool createNarrative = false;

        public bool isFinished = true; //verifica se a missão já terminou
        
        private JSonWriter writer;
        [SerializeField] private FormQuestionsData preTestQuestionnaire;

        public Selector Selector { get; set; }
        public QuestList Quests { get; set; }

        public FormQuestionsData PreTestQuestionnaire
        {
            get => preTestQuestionnaire;
            set => preTestQuestionnaire = value;
        }

        void Awake()
        {
            Quests = new QuestList();
            Selector = new Selector();
            writer = new JSonWriter();
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
            PlayerProfileEnum playerProfile = Selector.Select(this, e);
            if (createNarrative)
            {
                makeBranches();

                writer.writeJSon(Quests, playerProfile);

                // for (int i = 0; i < Quests.graph.Count; i++)
                //     Debug.Log(Quests.graph[i].Tipo + ", " + Quests.graph[i].c1 + ", " + Quests.graph[i].c2);
            }

            ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
        }

        void Start()
        {
            PlayerProfileEnum playerProfile;
            List<int> answers = new List<int>();
            if (PreTestQuestionnaire != null)
            {
                foreach (FormQuestionData questionData in PreTestQuestionnaire.questions)
                    answers.Add(questionData.answer);

                Debug.Log("Answers: " + answers.Count);

                playerProfile = Selector.Select(this, answers);

                if (createNarrative)
                {
                    makeBranches();

                    writer.writeJSon(Quests, playerProfile);

                    // for (int i = 0; i < Quests.graph.Count; i++)
                    //     Debug.Log(Quests.graph[i].Tipo + ", " + Quests.graph[i].c1 + ", " + Quests.graph[i].c2);
                }

                ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
            }
        }

        void makeBranches()
        {
            // int index = 0, b, c1, c2;

            // Quests.graph[index].parent = -1;

            // while (index < Quests.graph.Count)
            // {
            //     b = Random.Range(0, 100);

            //     if (b % 2 == 0)
            //     {
            //         c1 = Random.Range(index + 1, Quests.graph.Count);
            //         if (c1 < Quests.graph.Count)
            //         {
            //             Quests.graph[c1].parent = index;
            //         }

            //         c2 = Random.Range(index + 1, Quests.graph.Count);
            //         if (c2 < Quests.graph.Count)
            //         {
            //             Quests.graph[c2].parent = index;
            //         }

            //         Quests.graph[index].c1 = c1;
            //         if (c1 != c2)
            //         {
            //             Quests.graph[index].c2 = c2;
            //         }
            //     }
            //     else if ((index + 1) < Quests.graph.Count && Quests.graph[index + 1].parent == -1)
            //     {
            //         Quests.graph[index + 1].parent = index;
            //         Quests.graph[index].c1 = index + 1;
            //     }

            //     index++;
            // }
        }
    }
}