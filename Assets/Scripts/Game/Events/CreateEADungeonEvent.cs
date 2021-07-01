using System;

public delegate void CreateEADungeonEvent(object sender, CreateEADungeonEventArgs e);
public class CreateEADungeonEventArgs : EventArgs
{
    private Fitness fitness;
    private JSonWriter.ParametersMonsters parametersMonsters;
    private JSonWriter.ParametersItems parametersItems;
    private JSonWriter.ParametersNpcs parametersNpcs;
    public CreateEADungeonEventArgs(Fitness fitness)
    {
        Fitness = fitness;
        ParametersMonsters = null;
        ParametersItems = null;
        ParametersNpcs = null;
    }
    public CreateEADungeonEventArgs(JSonWriter.ParametersDungeon parametersDungeon, 
        JSonWriter.ParametersMonsters parametersMonsters, JSonWriter.ParametersItems parametersItems, 
            JSonWriter.ParametersNpcs parametersNpcs)
    {
        Fitness = new Fitness(parametersDungeon.size, parametersDungeon.nKeys, parametersDungeon.nKeys, parametersDungeon.linearity);
        ParametersMonsters = parametersMonsters;
        ParametersNpcs = parametersNpcs;
        ParametersItems = parametersItems;
    }


    public Fitness Fitness { get => fitness; set => fitness = value; }
    public JSonWriter.ParametersMonsters ParametersMonsters { get => parametersMonsters; set => parametersMonsters = value; }
    public JSonWriter.ParametersNpcs ParametersNpcs { get => parametersNpcs; set => parametersNpcs = value; }
    public JSonWriter.ParametersItems ParametersItems { get => parametersItems; set => parametersItems = value; }
}
