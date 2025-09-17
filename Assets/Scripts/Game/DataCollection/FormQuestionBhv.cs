using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.DataCollection
{
    public class FormQuestionBhv : MonoBehaviour
    {
        public List<FormQuestionAnswer> answers;
        public Text questionText;
        public Text descriptionText;

        public FormQuestionData questionData;
        public FormQuestionAnswer questionAnswerPrefab;
        public Transform answerParent;

        // Use this for initialization
        void Start()
        {
            questionData.answer = -1;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeValue(Toggle selected)
        {
            if (!selected.isOn)
            {
                //Debug.Log("IsOff:"+ int.Parse(selected.GetComponentInChildren<Text>().text));
                questionData.answer = -1;
            }
            else
            {
                foreach (FormQuestionAnswer answr in answers)
                {
                    if (answr.toggle != selected)
                    {
                        //Debug.Log("NotSelected:"+ int.Parse(t.GetComponentInChildren<Text>().text));
                        answr.toggle.isOn = false;
                        //Debug.Log("After Falsing");
                    }
                }

                questionData.answer = int.Parse(selected.GetComponentInChildren<Text>().text);
            }

        }

        public void ResetToggles()
        {
            foreach (FormQuestionAnswer answr in answers)
            {
                answr.toggle.isOn = false;
            }
        }

        public void LoadData(FormQuestionData q)
        {
            questionData = q;
            questionText.text = q.question;
            descriptionText.text = q.description;
            
            for (int i = 0; i < questionData.totalAnswers; i++)
            {
                FormQuestionAnswer answer = Instantiate(questionAnswerPrefab, answerParent);
                answer.SetAnswer(i + 1, this);
                answers.Add(answer);
            }
        }
    }
}
