using System.Collections.Generic;

public class DungeonRoom : DungeonPart
{
    protected Dimensions dimensions; // inicializar valores antes de acessar os tiles
    public int[,] tiles = null;
    public List<int> keyIDs;
    protected int difficulty, treasure;
    public int Treasure { get => treasure; set => treasure = value; }
    public int Difficulty { get => difficulty; set => difficulty = value; }
    public Dimensions Dimensions { get => dimensions; set => dimensions = value; }

    public DungeonRoom(Coordinates coordinates, string code, List<int> keyIDs, int difficulty, int treasure) : base(coordinates, code)
    {        
        this.keyIDs = keyIDs;
        Treasure = treasure;
        Difficulty = difficulty;
    }

    public void InitializeTiles()
    { // prepara a memória para receber os valores dos tiles
        tiles = new int[dimensions.Width, dimensions.Height];
    }
}

