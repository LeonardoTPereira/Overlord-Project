using System;

public delegate void CreateEADungeonEvent(object sender, CreateEADungeonEventArgs e);
public class CreateEADungeonEventArgs : EventArgs
{
    protected Fitness fitness;
    public CreateEADungeonEventArgs(Fitness fitness)
    {
        Fitness = fitness;
    }
    public CreateEADungeonEventArgs(int objectiveRooms, int objectiveKeys, int objectiveLocks, float objectiveLinearity)
    {
        Fitness = new Fitness(objectiveRooms, objectiveKeys, objectiveLocks, objectiveLinearity);
    }

    public Fitness Fitness { get => fitness; set => fitness = value; }
}
