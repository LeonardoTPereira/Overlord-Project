using Game.EnemyGenerator;
using ScriptableObjects;

namespace Game.Quests
{
    public class QuestKillEnemyEventArgs : QuestElementEventArgs
    {
        public WeaponTypeSO EnemyWeaponTypeSo {get; set; }

        public QuestKillEnemyEventArgs(WeaponTypeSO enemyWeaponTypeSo)
        {
            EnemyWeaponTypeSo = enemyWeaponTypeSo;
        }
    }
}