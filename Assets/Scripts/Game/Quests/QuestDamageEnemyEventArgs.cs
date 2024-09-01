using Game.EnemyGenerator;
using ScriptableObjects;

namespace Game.Quests
{
    public class QuestDamageEnemyEventArgs : QuestElementEventArgs
    {
        public WeaponTypeSo EnemyWeaponTypeSo {get; set; }
        public int Damage { get; set; }

        public QuestDamageEnemyEventArgs(WeaponTypeSo enemyWeaponTypeSo, int damage, int questId) : base(questId)
        {
            EnemyWeaponTypeSo = enemyWeaponTypeSo;
            Damage = damage;
        }
    }
}