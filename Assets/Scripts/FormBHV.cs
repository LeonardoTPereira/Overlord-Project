using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormBHV : MonoBehaviour {

    public FormQuestionsData questionsData;
    public GameObject questionPrefab;
    public RectTransform questionsPanel;
    public RectTransform submitButton;
    public float extraQuestionsPanelHeight = 100;
    private List<FormQuestionBHV> questions = new List<FormQuestionBHV>();

	// Use this for initialization
	void Start () {
		foreach (FormQuestionData q in questionsData.questions)
        {
            GameObject g = (GameObject)Instantiate(questionPrefab);
            g.GetComponent<FormQuestionBHV>().LoadData(q);
            g.transform.SetParent(questionsPanel);
            questions.Add(g.GetComponent<FormQuestionBHV>());
        }
        float panelHeight = questionsData.questions.Count
            * questionPrefab.GetComponent<RectTransform>().rect.height;
        panelHeight += extraQuestionsPanelHeight;
        questionsPanel.sizeDelta = new Vector2(0.0f, panelHeight);
        submitButton.SetAsLastSibling();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Submit ()
    {
        bool isAnswered = true;
        foreach (FormQuestionBHV q in questions)
        {
            if (q.value == 0)
                isAnswered = false;
        }
        if (isAnswered)
        {
            Debug.Log("Answered Correctly");
            foreach (FormQuestionBHV q in questions)
            {
                int answer = q.value;
                Debug.Log("Value:" + q.value);
                // TODO: lógica para submissão das respostas
                // Sugestão: passar todos os int para o formato int1, int2, int3... (csv)
                // e criar um novo método em player profile para receber essa adição e fazer o post
                PlayerProfile.instance.OnFormAnswered(answer);
            }
            GameManager.instance.CheckEndOfBatch();
        }
    }
}
