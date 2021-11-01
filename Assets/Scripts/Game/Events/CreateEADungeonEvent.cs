using System;
using Game.NarrativeGenerator;

public delegate void CreateEADungeonEvent(object sender, CreateEADungeonEventArgs e);
public class CreateEADungeonEventArgs : EventArgs
{
    private Fitness fitness;
    private QuestEnemiesParameters parametersMonsters;
    private QuestItemsParameters _questItemsParameters;
    private QuestNpcsParameters _questNpcsParameters;
    private string playerProfile;
    private string narrativeName;

    public CreateEADungeonEventArgs(Fitness fitness)
    {
        Fitness = fitness;
        ParametersMonsters = null;
        QuestItemsParameters = null;
        QuestNpcsParameters = null;
    }
    
    //TODO review why so many parameters
    public CreateEADungeonEventArgs(QuestDungeonsParameters parametersDungeon, 
        QuestEnemiesParameters parametersMonsters, QuestItemsParameters questItemsParameters, 
            QuestNpcsParameters questNpcsParameters, string playerProfile, string narrativeName)
    {
        Fitness = new Fitness(parametersDungeon.Size, parametersDungeon.NKeys, parametersDungeon.NKeys, parametersDungeon.GetLinearity());
        ParametersMonsters = parametersMonsters;
        QuestNpcsParameters = questNpcsParameters;
        QuestItemsParameters = questItemsParameters;
        PlayerProfile = playerProfile;
        NarrativeName = narrativeName;
    }


    public Fitness Fitness { get => fitness; set => fitness = value; }
    public QuestEnemiesParameters ParametersMonsters { get => parametersMonsters; set => parametersMonsters = value; }
    public QuestNpcsParameters QuestNpcsParameters { get => _questNpcsParameters; set => _questNpcsParameters = value; }
    public QuestItemsParameters QuestItemsParameters { get => _questItemsParameters; set => _questItemsParameters = value; }
    public string PlayerProfile { get => playerProfile; set => playerProfile = value; }
    public string NarrativeName { get => narrativeName; set => narrativeName = value; }
}
