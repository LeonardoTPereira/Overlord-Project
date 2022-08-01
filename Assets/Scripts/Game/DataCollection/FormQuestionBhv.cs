using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.DataCollection
{
    public class FormQuestionBhv : MonoBehaviour
    {
        public Toggle[] toggles;
        public Text questionText;
        public Text descriptionText;

        public FormQuestionData questionData;

        void Awake()
        {
            toggles = GetComponentsInChildren<Toggle>().ToArray<Toggle>();
        }

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
                foreach (Toggle t in toggles)
                {
                    if (t != selected)
                    {
                        //Debug.Log("NotSelected:"+ int.Parse(t.GetComponentInChildren<Text>().text));
                        t.isOn = false;
                        //Debug.Log("After Falsing");
                    }

                }

                questionData.answer = int.Parse(selected.GetComponentInChildren<Text>().text);
            }

        }

        public void ResetToggles()
        {
            foreach (Toggle t in toggles)
            {
                t.isOn = false;
            }
        }

        public void LoadData(FormQuestionData q)
        {
            questionData = q;
            questionText.text = q.question;
            descriptionText.text = q.description;
        }

    }
}
