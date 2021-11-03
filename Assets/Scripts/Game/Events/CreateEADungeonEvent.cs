﻿using System;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;

public delegate void CreateEADungeonEvent(object sender, CreateEADungeonEventArgs e);
public class CreateEADungeonEventArgs : EventArgs
{
    private Fitness fitness;
    public QuestLine QuestLineForDungeon { get; }
    private string playerProfile;

    public CreateEADungeonEventArgs(Fitness fitness)
    {
        Fitness = fitness;
        QuestLineForDungeon = null;
    }
<<<<<<< HEAD
    
    //TODO review why so many parameters
    public CreateEADungeonEventArgs(QuestLine questLine)
    {
        QuestLineForDungeon = questLine;
        QuestDungeonsParameters questDungeonParameters = questLine.DungeonParametersForQuestLine;
        Fitness = new Fitness(questDungeonParameters.Size, questDungeonParameters.NKeys, questDungeonParameters.NKeys, questDungeonParameters.GetLinearity());
=======
    public CreateEADungeonEventArgs(JSonWriter.ParametersDungeon parametersDungeon,
        JSonWriter.ParametersMonsters parametersMonsters, JSonWriter.ParametersItems parametersItems,
            JSonWriter.ParametersNpcs parametersNpcs, string playerProfile, string narrativeName)
    {
        int enemies = 30;
        Fitness = new Fitness(parametersDungeon.size, parametersDungeon.nKeys, parametersDungeon.nKeys, enemies, parametersDungeon.linearity);
        ParametersMonsters = parametersMonsters;
        ParametersNpcs = parametersNpcs;
        ParametersItems = parametersItems;
        PlayerProfile = playerProfile;
        NarrativeName = narrativeName;
>>>>>>> Arena
    }


    public Fitness Fitness { get => fitness; set => fitness = value; }
}
