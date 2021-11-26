using System;
using System.Collections.Generic;

namespace Game.Events
{
    public delegate void FormAnsweredEvent(object sender, FormAnsweredEventArgs e);
    public class FormAnsweredEventArgs : EventArgs
    {
        private int formID;
        private List<int> answerValue;

        public FormAnsweredEventArgs(int formID, List<int> answerValue)
        {
            FormID = formID;
            AnswerValue = answerValue;
        }

        public int FormID { get => formID; set => formID = value; }
        public List<int> AnswerValue { get => answerValue; set => answerValue = value; }
    }
}