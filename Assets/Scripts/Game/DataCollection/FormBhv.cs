using System.Collections.Generic;
using Game.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.DataCollection
{
    public class FormBhv : MonoBehaviour
    {
        [SerializeField] private bool _hasCheckbox = false;
        public FormQuestionsData questionsData;
        public GameObject questionPrefab;
        public GameObject checkboxPrefab;
        public RectTransform questionsPanel;
        public RectTransform submitButton;
        public float extraQuestionsPanelHeight = 100;
        private List<FormQuestionBhv> questions = new List<FormQuestionBhv>();
        private FormCheckboxBhv checkboxForm;
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

            if (_hasCheckbox)
            {
                GameObject g = Instantiate(checkboxPrefab);
                g.transform.SetParent(questionsPanel);
                checkboxForm = g.GetComponent<FormCheckboxBhv>();

                panelHeight += checkboxPrefab.GetComponent<RectTransform>().rect.height;
            }

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

            if (_hasCheckbox)
            {
                foreach (Toggle t in checkboxForm.toggles)
                {
                    if (t.isOn)
                        answers.Add(-1);
                    else 
                        answers.Add(-2);
                }
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
