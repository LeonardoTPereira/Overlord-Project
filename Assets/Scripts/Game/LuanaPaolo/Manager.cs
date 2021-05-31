using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    /// The form ID of the pre-questionnaire.
    private const int PRE_QUESTIONNAIRE = 0;

    /// The pre-questionnaire size.
    private const int PRE_QUESTIONNAIRE_SIZE = 11;

    /// List of answers of the pre-questionnaire.
    private List<int> answers;

    /// It controls if the content was selected or not.
    private bool selected = false;

    public Selector selector = new Selector(); //seletor

    public List<Quest> graph = new List<Quest>(); //lista de quests

    public bool isFinished = true; //verifica se a missão já terminou

    public Player_Movement player; //jogador

    private JSonWriter writer;

    void Start()
    {
        player = FindObjectOfType<Player_Movement>();

        writer = new JSonWriter();
    }

    void Update()
    {
        // Check if all the pre-questionnaire answers were received and if the 
        // contents were not selected yet
        if (!(this.answers is null)
            && !this.selected
            && this.answers.Count == PRE_QUESTIONNAIRE_SIZE
        ) {
            // Select the appropriate contents for the player and store them in
            // the attribute `graph` and the game objects for the dungeons
            this.selector.Select(this, this.answers);
            this.selected = true;
        }

        if (isFinished == true)
        {
            isFinished = false;
            selector.Select(this, this.answers);

            makeBranches();

            writer.writeJSon(graph);

            for (int i = 0; i < graph.Count; i++) Debug.Log(graph[i].tipo + ", " + graph[i].c1 + ", " + graph[i].c2);

            isFinished = false;
        }

        //if(isFinished == false) mission();
    }

    /// This method is called when this Manager is toggled.
    ///
    /// It adds the event responses related to this class.
    protected void OnEnable()
    {
        FormBHV.FormQuestionAnsweredEventHandler += OnFormQuestionAnswered;
    }

    /// This method is called when this Manager is toggled.
    ///
    /// It removes the event responses related to this class.
    protected void OnDisable()
    {
        FormBHV.FormQuestionAnsweredEventHandler -= OnFormQuestionAnswered;
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

    /// This method gets the pre-questionnaire result and adds the answers to 
    /// the list of answers of this Manager.
    private void OnFormQuestionAnswered(object sender, FormAnsweredEventArgs e)
    {
        // Get the arguments
        int form = e.FormID;
        int answer = e.AnswerValue;
        // Check if the given form is the pre-questionnaire
        if (form == PRE_QUESTIONNAIRE) {
            // Initiliaze the list of answers with the given answer
            if (this.answers is null)
            {
                this.answers = new List<int>();
            }
            // Add the given answer in the list of answers
            this.answers.Add(answer);
        }
    }
}