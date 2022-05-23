using Game.EnemyGenerator;

namespace Game.Quests
{
    public class QuestKillEnemyEventArgs : QuestElementEventArgs
    {
        public WeaponType EnemyWeaponType {get; set; }

        public QuestKillEnemyEventArgs(WeaponType enemyWeaponType)
        {
            EnemyWeaponType = enemyWeaponType;
        }
    }
}