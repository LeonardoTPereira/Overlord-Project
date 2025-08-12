using Game.Events;
using System.Collections.Generic;
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

        private List<int> answers;

        public static event FormAnsweredEvent PreTestFormQuestionAnsweredEventHandler;
        public static event FormAnsweredEvent PostTestFormQuestionAnsweredEventHandler;

        void Start()
        {
            InstantiateForms();
            ResizeFormsHeight();
        }
                
        public void Submit()
        {
#if UNITY_EDITOR
            AssetDatabase.SaveAssetIfDirty(questionsData);
#endif
            answers = GetIntListFromFormQuestionBhvList(questions);
            GetToggleAnswersFromFormCheckboxBhv(checkboxForm);
            SendFormToRightEventHandler(formID);
        }

        private void InstantiateForms()
        {
            foreach (FormQuestionData q in questionsData.questions)
            {
                GameObject g = Instantiate(questionPrefab);
                g.GetComponent<FormQuestionBhv>().LoadData(q);
                g.transform.SetParent(questionsPanel);
                questions.Add(g.GetComponent<FormQuestionBhv>());
            }
        }

        private void ResizeFormsHeight()
        {
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

        private List<int> GetIntListFromFormQuestionBhvList(List<FormQuestionBhv> questions)
        {
            List<int> answers = new List<int>();

            foreach (FormQuestionBhv q in questions)
            {
                answers.Add(q.questionData.answer);
                q.ResetToggles();
            }

            return answers;
        }

        private void GetToggleAnswersFromFormCheckboxBhv(FormCheckboxBhv checkboxForm)
        {
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
        }

        private void SendFormToRightEventHandler(int formID)
        {
            if (formID == 1)
                PostTestFormQuestionAnsweredEventHandler?.Invoke(null, new FormAnsweredEventArgs(formID, answers));
            else
            {
                PreTestFormQuestionAnsweredEventHandler?.Invoke(this, new FormAnsweredEventArgs(formID, answers));
            }
        }
    }
}
