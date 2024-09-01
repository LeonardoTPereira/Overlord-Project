using System.Collections.Generic;
using Game.Events;
using UnityEditor;
using UnityEngine;

namespace Game.DataCollection
{
    public class FormBhv : MonoBehaviour
    {

        public FormQuestionsData questionsData;
        public GameObject questionPrefab;
        public RectTransform questionsPanel;
        public RectTransform submitButton;
        public float extraQuestionsPanelHeight = 100;
        private List<FormQuestionBhv> questions = new List<FormQuestionBhv>();
        public int formID; //0 for pretest, 1 for posttest

        public static event FormAnsweredEvent PreTestFormQuestionAnsweredEventHandler;
        public static event FormAnsweredEvent PostTestFormQuestionAnsweredEventHandler;

        // Use this for initialization
        void Start()
        {
            foreach (FormQuestionData q in questionsData.questions)
            {
                GameObject g = Instantiate(questionPrefab);
                g.GetComponent<FormQuestionBhv>().LoadData(q);
                g.transform.SetParent(questionsPanel);
                questions.Add(g.GetComponent<FormQuestionBhv>());
            }
            float panelHeight = questionsData.questions.Count
                                * questionPrefab.GetComponent<RectTransform>().rect.height;
            panelHeight += extraQuestionsPanelHeight;
            questionsPanel.sizeDelta = new Vector2(0.0f, panelHeight);
            submitButton.SetAsLastSibling();
        }

        public void Submit()
        {
            #if UNITY_EDITOR
            AssetDatabase.SaveAssetIfDirty(questionsData);
            #endif
            List<int> answers = new List<int>();
            foreach (FormQuestionBhv q in questions)
            {
                answers.Add(q.questionData.answer);
                q.ResetToggles();
            }
            if (formID == 1)
                PostTestFormQuestionAnsweredEventHandler?.Invoke(null, new FormAnsweredEventArgs(formID, answers));
            else
            {
                PreTestFormQuestionAnsweredEventHandler?.Invoke(this, new FormAnsweredEventArgs(formID, answers));
            }
        }
    }
}
