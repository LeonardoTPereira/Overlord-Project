using System;
using Game.NarrativeGenerator;
using static Enums;

public delegate void CreateEADungeonEvent(object sender, CreateEADungeonEventArgs e);
public class CreateEADungeonEventArgs : EventArgs
{
    private Fitness fitness;
    private EnemyParameters parametersMonsters;
    private ParametersItems parametersItems;
    private ParametersNpcs parametersNpcs;
    private string playerProfile;
    private string narrativeName;

    public CreateEADungeonEventArgs(Fitness fitness)
    {
        Fitness = fitness;
        ParametersMonsters = null;
        ParametersItems = null;
        ParametersNpcs = null;
    }
    public CreateEADungeonEventArgs(ParametersDungeon parametersDungeon, 
        EnemyParameters parametersMonsters, ParametersItems parametersItems, 
            ParametersNpcs parametersNpcs, string playerProfile, string narrativeName)
    {
        Fitness = new Fitness(parametersDungeon.size, parametersDungeon.nKeys, parametersDungeon.nKeys, parametersDungeon.linearity);
        ParametersMonsters = parametersMonsters;
        ParametersNpcs = parametersNpcs;
        ParametersItems = parametersItems;
        PlayerProfile = playerProfile;
        NarrativeName = narrativeName;
    }


    public Fitness Fitness { get => fitness; set => fitness = value; }
    public EnemyParameters ParametersMonsters { get => parametersMonsters; set => parametersMonsters = value; }
    public ParametersNpcs ParametersNpcs { get => parametersNpcs; set => parametersNpcs = value; }
    public ParametersItems ParametersItems { get => parametersItems; set => parametersItems = value; }
    public string PlayerProfile { get => playerProfile; set => playerProfile = value; }
    public string NarrativeName { get => narrativeName; set => narrativeName = value; }
}
