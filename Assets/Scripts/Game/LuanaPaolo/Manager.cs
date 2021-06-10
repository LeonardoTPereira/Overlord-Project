using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;

public class Manager : MonoBehaviour
{

    public static event ProfileSelectedEvent ProfileSelectedEventHandler;

    /// List of answers of the pre-questionnaire.
    public FormQuestionsData preTestQuestionnaire;

    /// It controls if the content was selected or not.
    private bool selected = false;

    public Selector selector; //seletor

    public List<Quest> graph; //lista de quests/

    public bool isFinished = true; //verifica se a missão já terminou

    public Player_Movement player; //jogador

    private JSonWriter writer;

    private PlayerProfileEnum playerProfile;

    void Awake()
    {
        graph = new List<Quest>();
        selector = new Selector();
        writer = new JSonWriter();
    }

    void Start()
    {
        player = FindObjectOfType<Player_Movement>();
        List<int> answers = new List<int>();
        foreach (FormQuestionData questionData in preTestQuestionnaire.questions)
            answers.Add(questionData.answer);

        Debug.Log("Answers: " + answers.Count);

        playerProfile = selector.Select(this, answers);

        makeBranches();

        writer.writeJSon(graph);

        for (int i = 0; i < graph.Count; i++) Debug.Log(graph[i].tipo + ", " + graph[i].c1 + ", " + graph[i].c2);

        Debug.Log("Profile: " + playerProfile.ToString());
        ProfileSelectedEventHandler(this, new ProfileSelectedEventArgs(playerProfile));
    }

    void makeBranches()
    {
        int index = 0, b, c1, c2;

        graph[index].parent = -1;

        while (index < graph.Count)
        {
            b = Random.Range(0, 100);

            if (b % 2 == 0)
            {
                c1 = Random.Range(index + 1, graph.Count);
                if (c1 < graph.Count) graph[c1].parent = index;

                c2 = Random.Range(index + 1, graph.Count);
                if (c2 < graph.Count) graph[c2].parent = index;

                graph[index].c1 = c1;
                if (c1 != c2) graph[index].c2 = c2;
            }
            else if (index + 1 < graph.Count)
            {
                if (graph[index + 1].parent == -1)
                {
                    graph[index + 1].parent = index;
                    graph[index].c1 = index + 1;
                }
            }

            index++;
        }
    }
}