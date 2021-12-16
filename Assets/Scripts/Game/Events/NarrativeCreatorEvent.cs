using System;
using System.Collections.Generic;

public delegate void NarrativeCreatorEvent(object sender, NarrativeCreatorEventArgs e);

public class NarrativeCreatorEventArgs : EventArgs
{
    private Dictionary<string, int> questWeightsbyType;

    public NarrativeCreatorEventArgs(Dictionary<string, int> questWeightsbyType)
    {
        QuestWeightsbyType = questWeightsbyType;
    }

    public Dictionary<string, int> QuestWeightsbyType { get => questWeightsbyType; set => questWeightsbyType = value; }
}
