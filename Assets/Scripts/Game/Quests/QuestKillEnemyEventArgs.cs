using Game.EnemyGenerator;
using ScriptableObjects;

namespace Game.Quests
{
    public class QuestKillEnemyEventArgs : QuestElementEventArgs
    {
        public WeaponTypeSo EnemyWeaponTypeSo {get; set; }

        public QuestKillEnemyEventArgs(WeaponTypeSo enemyWeaponTypeSo, int questId):base(questId)
        {
            EnemyWeaponTypeSo = enemyWeaponTypeSo;
        }
    }
}