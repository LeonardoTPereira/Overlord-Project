using Game.GameManager;
using Overlord.NarrativeGenerator;
using Overlord.NarrativeGenerator.NPCs;
using Overlord.NarrativeGenerator.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TopdownQuestLine : QuestLine
{
    public new void PopulateQuestLine(in NarrativeSettings narrativeSettings, NpcSo npcInCharge)
    {
        Dictionary<string, Func<int, float>> startSymbolWeights = YeeProfileCalculator.StartSymbolWeights;
        if (ExperimentController.UseRandomProfile)
        {
            startSymbolWeights = GetRandomSymbolWeights();
        }
        PopulateQuestLineMarkov(narrativeSettings, npcInCharge, startSymbolWeights);
    }
}