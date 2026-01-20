namespace Overlord.ProfileAnalyst
{
    public interface IPlayerProfile
    {
        public bool IsFixedFromExperiment { get; set; }
        public string PlayerProfilingType { get; }          // e.g. "Yee", "Bartle", "Hexad"
        public void Normalize();
        public string ToString();
    }
}