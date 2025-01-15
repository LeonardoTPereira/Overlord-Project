using System;
using System.Collections.Generic;

namespace Game.Events
{
    public delegate void ProfileAnalystFinishedEvent(object sender, ProfileAnalystFinishedEventArgs e);
    public class ProfileAnalystFinishedEventArgs : EventArgs
    {
        private Dictionary<string, int> answerByQuestion;

        public ProfileAnalystFinishedEventArgs(Dictionary<string, int> answerByQuestion)
        {
            AnswerByQuestion = answerByQuestion;
        }

        public Dictionary<string, int> AnswerByQuestion
        {
            get => answerByQuestion; 
            set => answerByQuestion = value;
        }
    }
}