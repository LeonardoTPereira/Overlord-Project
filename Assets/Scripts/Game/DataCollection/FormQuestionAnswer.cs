using UnityEngine;
using UnityEngine.UI;

namespace Game.DataCollection
{
    public class FormQuestionAnswer : MonoBehaviour
    {
        public Toggle toggle;
        [SerializeField] private Text text;
        private FormQuestionBhv questionBhv;

        public void SetAnswer(int answer, FormQuestionBhv questionBhv)
        {
            text.text = answer.ToString();
            this.questionBhv = questionBhv;
            toggle.onValueChanged.AddListener(OnToggle);
        }

        private void OnToggle(bool toggleOn)
        {
            if (toggleOn)
            {
                questionBhv.ChangeValue(toggle);
            }
        }
    }
}
