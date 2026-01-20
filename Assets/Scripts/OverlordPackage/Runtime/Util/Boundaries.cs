namespace Util
{
    public class Boundaries
    {
        protected Coordinates maxBoundaries;
        protected Coordinates minBoundaries;

        public Boundaries(Coordinates minBoundaries, Coordinates maxBoundaries)
        {
            MinBoundaries = new Coordinates(minBoundaries);
            MaxBoundaries = new Coordinates(maxBoundaries);
        }

        public Coordinates MinBoundaries { get => minBoundaries; set => minBoundaries = value; }
        public Coordinates MaxBoundaries { get => maxBoundaries; set => maxBoundaries = value; }
    }
}
