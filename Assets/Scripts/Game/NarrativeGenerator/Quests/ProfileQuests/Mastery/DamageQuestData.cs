using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class DamageQuestData
    {
        public int Damage { get; set; }
        public WeaponTypeSO Enemy { get; set; }

        public DamageQuestData(int damage, WeaponTypeSO enemy)
        {
            Enemy = enemy;
            Damage = damage;
        }
        
        public DamageQuestData(DamageQuestData damageToCopy)
        {
            Enemy = damageToCopy.Enemy;
            Damage = damageToCopy.Damage;
        }
        
        public DamageQuestData()
        {
            Enemy = null;
            Damage = -1;
        }
    }
}