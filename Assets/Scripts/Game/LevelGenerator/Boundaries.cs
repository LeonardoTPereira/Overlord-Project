using Game.LevelManager;

public class Boundaries
{
    protected Coordinates maxBoundaries;
    protected Coordinates minBoundaries;

    public Boundaries(Coordinates minBoundaries, Coordinates maxBoundaries)
    {
        MinBoundaries = minBoundaries;
        MaxBoundaries = maxBoundaries;
    }

    public Coordinates MinBoundaries { get => minBoundaries; set => minBoundaries = value; }
    public Coordinates MaxBoundaries { get => maxBoundaries; set => maxBoundaries = value; }
}
