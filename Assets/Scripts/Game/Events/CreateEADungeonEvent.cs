using System;
using Game.NarrativeGenerator;
using static Enums;

public delegate void CreateEADungeonEvent(object sender, CreateEADungeonEventArgs e);
public class CreateEADungeonEventArgs : EventArgs
{
    private Fitness fitness;
    private QuestEnemiesSO parametersMonsters;
    private QuestItemsSO _questItemsSo;
    private QuestNpcsSO _questNpcsSo;
    private string playerProfile;
    private string narrativeName;

    public CreateEADungeonEventArgs(Fitness fitness)
    {
        Fitness = fitness;
        ParametersMonsters = null;
        QuestItemsSo = null;
        QuestNpcsSo = null;
    }
    
    //TODO review why so many parameters
    public CreateEADungeonEventArgs(QuestDungeonsSO parametersDungeon, 
        QuestEnemiesSO parametersMonsters, QuestItemsSO questItemsSo, 
            QuestNpcsSO questNpcsSo, string playerProfile, string narrativeName)
    {
        Fitness = new Fitness(parametersDungeon.Size, parametersDungeon.NKeys, parametersDungeon.NKeys, parametersDungeon.getLinearity());
        ParametersMonsters = parametersMonsters;
        QuestNpcsSo = questNpcsSo;
        QuestItemsSo = questItemsSo;
        PlayerProfile = playerProfile;
        NarrativeName = narrativeName;
    }


    public Fitness Fitness { get => fitness; set => fitness = value; }
    public QuestEnemiesSO ParametersMonsters { get => parametersMonsters; set => parametersMonsters = value; }
    public QuestNpcsSO QuestNpcsSo { get => _questNpcsSo; set => _questNpcsSo = value; }
    public QuestItemsSO QuestItemsSo { get => _questItemsSo; set => _questItemsSo = value; }
    public string PlayerProfile { get => playerProfile; set => playerProfile = value; }
    public string NarrativeName { get => narrativeName; set => narrativeName = value; }
}
