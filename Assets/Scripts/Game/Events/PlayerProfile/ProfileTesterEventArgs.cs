using System;
using System.Collections.Generic;

namespace Game.Events
{
    public delegate void ProfileTesterEvent(object sender, ProfileTesterEventArgs e);
    public class ProfileTesterEventArgs : EventArgs
    {
        public List<FormAnsweredEventArgs> Answers {get; private set; }

        public ProfileTesterEventArgs(List<FormAnsweredEventArgs> answers)
        {
            Answers = answers;
        }
    }
}