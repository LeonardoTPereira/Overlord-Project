using System;

public delegate void CreateEADungeonEvent(object sender, CreateEADungeonEventArgs e);
public class CreateEADungeonEventArgs : EventArgs
{
    private Fitness fitness;
    private JSonWriter.parametersMonsters parametersMonsters;
    public CreateEADungeonEventArgs(Fitness fitness)
    {
        Fitness = fitness;
    }
    public CreateEADungeonEventArgs(JSonWriter.parametersDungeon parametersDungeon, JSonWriter.parametersMonsters parametersMonsters)
    {
        Fitness = new Fitness(parametersDungeon.size, parametersDungeon.nKeys, parametersDungeon.nKeys, parametersDungeon.linearity);
        ParametersMonsters = parametersMonsters;
    }


    public Fitness Fitness { get => fitness; set => fitness = value; }
    public JSonWriter.parametersMonsters ParametersMonsters { get => parametersMonsters; set => parametersMonsters = value; }
}
