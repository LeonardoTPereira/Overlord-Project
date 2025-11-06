namespace Overlord.ProfileAnalyst
{
    // Responsável por receber algum input (ou nenhum) e retornar um perfil de jogador
    public interface IPlayerProfileCalculator
    {
        public IPlayerProfile CreateProfileFromPlayerProfileSO(PlayerProfileSO playerProfileSO);
        public IPlayerProfile GetRandomPlayerProfile();
    }
}