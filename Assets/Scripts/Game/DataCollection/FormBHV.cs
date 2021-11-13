using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataCollection
{
    public class FormBHV : MonoBehaviour
    {

        public FormQuestionsData questionsData;
        public GameObject questionPrefab;
        public RectTransform questionsPanel;
        public RectTransform submitButton;
        public float extraQuestionsPanelHeight = 100;
        private List<FormQuestionBHV> questions = new List<FormQuestionBHV>();
        public int formID; //0 for pretest, 1 for posttest

        public static event FormAnsweredEvent FormQuestionAnsweredEventHandler;
        public static event EventHandler PostTestFormAnswered;

        // Use this for initialization
        void Start()
        {
            foreach (FormQuestionData q in questionsData.questions)
            {
                GameObject g = Instantiate(questionPrefab);
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
        void Update()
        {

        }

        public void Submit()
        {
            foreach (FormQuestionBHV q in questions)
            {
                FormQuestionAnsweredEventHandler(this, new FormAnsweredEventArgs(formID, q.questionData.answer));
                q.ResetToggles();
            }
            if (formID == 1)
                PostTestFormAnswered?.Invoke(null, EventArgs.Empty);
        }
    }
}
