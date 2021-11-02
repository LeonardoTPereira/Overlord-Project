using System;
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
    
    //TODO review why so many parameters
    public CreateEADungeonEventArgs(QuestLine questLine)
    {
        QuestLineForDungeon = questLine;
        QuestDungeonsParameters questDungeonParameters = questLine.DungeonParametersForQuestLine;
        Fitness = new Fitness(questDungeonParameters.Size, questDungeonParameters.NKeys, questDungeonParameters.NKeys, questDungeonParameters.GetLinearity());
    }


    public Fitness Fitness { get => fitness; set => fitness = value; }
}
