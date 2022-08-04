using Game.EnemyGenerator;
using ScriptableObjects;

namespace Game.Quests
{
    public class QuestDamageEnemyEventArgs : QuestElementEventArgs
    {
        public WeaponTypeSO EnemyWeaponTypeSo {get; set; }
        public int Damage { get; set; }

        public QuestDamageEnemyEventArgs(WeaponTypeSO enemyWeaponTypeSo, int damage, int questId) : base(questId)
        {
            EnemyWeaponTypeSo = enemyWeaponTypeSo;
            Damage = damage;
        }
    }
}