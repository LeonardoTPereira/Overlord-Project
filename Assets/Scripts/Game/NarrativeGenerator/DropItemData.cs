using ScriptableObjects;

namespace Game.NarrativeGenerator
{
    public class DropItemData
    {
        public WeaponTypeSo Enemy { get; set; }
        private ItemSo DropItem { get; set; }
        private float DropChance { get; set; }
        private int TotalItem { get; set; }
        private bool IsActive { get; set; }
    }
}