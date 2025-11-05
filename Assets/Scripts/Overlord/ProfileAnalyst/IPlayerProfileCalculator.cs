namespace Overlord.ProfileAnalyst
{
    public interface IPlayerProfileCalculator
    {
        public IPlayerProfile CreateProfile(PlayerProfileSO playerProfileSO);

        // Each of the following can be commented if not needed. Each is a form to collect input for the profile calculation.
        //public IPlayerProfile CreateProfileFromFormAnswers(List<int> answers, GeneratorSettings settings);
        //public IPlayerProfile CreateProfileFromNarrative(NarrativeCreatorEventArgs eventArgs);
        //public IPlayerProfile CreateProfileFromGameplay(PlayerData playerData, DungeonData dungeonData);
    }
}
