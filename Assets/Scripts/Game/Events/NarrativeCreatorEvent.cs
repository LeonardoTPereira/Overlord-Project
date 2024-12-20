using System;
using System.Collections.Generic;

namespace Game.Events
{
    public delegate void NarrativeCreatorEvent(object sender, NarrativeCreatorEventArgs e);

    public class NarrativeCreatorEventArgs : EventArgs
    {
        private Dictionary<string, float> questWeightsbyType;

        public NarrativeCreatorEventArgs(Dictionary<string, float> questWeightsbyType)
        {
            QuestWeightsbyType = questWeightsbyType;
        }

        public Dictionary<string, float> QuestWeightsbyType { get => questWeightsbyType; set => questWeightsbyType = value; }
    }
}