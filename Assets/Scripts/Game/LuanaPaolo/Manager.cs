
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;

public class Manager : MonoBehaviour
{

    public static event ProfileSelectedEvent ProfileSelectedEventHandler;

    public bool isFinished = true; //verifica se a missão já terminou

    public Player_Movement player; //jogador

    private JSonWriter writer;
    [SerializeField]
    private FormQuestionsData preTestQuestionnaire;

    public Selector Selector { get; set; }
    public List<Quest> Graph { get; set; }
    public FormQuestionsData PreTestQuestionnaire { get => preTestQuestionnaire; set => preTestQuestionnaire = value; }

    void Awake()
    {
        Graph = new List<Quest>();
        Selector = new Selector();
        writer = new JSonWriter();
    }

    void Start()
    {
        PlayerProfileEnum playerProfile;
        player = FindObjectOfType<Player_Movement>();
        List<int> answers = new List<int>();
        foreach (FormQuestionData questionData in PreTestQuestionnaire.questions)
            answers.Add(questionData.answer);

        Debug.Log("Answers: " + answers.Count);

        playerProfile = Selector.Select(this, answers);

        makeBranches();

        writer.writeJSon(Graph);

        for (int i = 0; i < Graph.Count; i++) Debug.Log(Graph[i].Tipo + ", " + Graph[i].c1 + ", " + Graph[i].c2);

        Debug.Log("Profile: " + playerProfile.ToString());
        ProfileSelectedEventHandler?.Invoke(this, new ProfileSelectedEventArgs(playerProfile));
    }

    void makeBranches()
    {
        int index = 0, b, c1, c2;

        Graph[index].parent = -1;

        while (index < Graph.Count)
        {
            b = Random.Range(0, 100);

            if (b % 2 == 0)
            {
                c1 = Random.Range(index + 1, Graph.Count);
                if (c1 < Graph.Count)
                {
                    Graph[c1].parent = index;
                }

                c2 = Random.Range(index + 1, Graph.Count);
                if (c2 < Graph.Count)
                {
                    Graph[c2].parent = index;
                }

                Graph[index].c1 = c1;
                if (c1 != c2)
                {
                    Graph[index].c2 = c2;
                }
            }
            else if (index + 1 < Graph.Count && Graph[index + 1].parent == -1)
            {
                Graph[index + 1].parent = index;
                Graph[index].c1 = index + 1;
            }

            index++;
        }
    }
}