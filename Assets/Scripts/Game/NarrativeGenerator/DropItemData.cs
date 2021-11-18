using ScriptableObjects;

namespace Game.NarrativeGenerator
{
    public class DropItemData
    {
        public WeaponTypeSO Enemy { get; set; }
        private ItemSO DropItem { get; set; }
        private float DropChance { get; set; }
        private int TotalItem { get; set; }
        private bool IsActive { get; set; }
    }
}