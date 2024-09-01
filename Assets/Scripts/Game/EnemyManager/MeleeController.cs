using Game.GameManager;
using ScriptableObjects;
using UnityEngine;

namespace Game.EnemyManager
{
    public class MeleeController : EnemyController
    {
        [field: SerializeField] protected GameObject WeaponPrefab { get; set; }

        [field: SerializeField] protected GameObject ShieldSpawn { get; set; }
        [field: SerializeField] protected GameObject SwordSpawn { get; set; }
        
        public override void LoadEnemyData(EnemySO enemyData, int questId)
        {
            base.LoadEnemyData(enemyData, questId);
            switch (enemyData.weapon.name)
            {
                case "Shield":
                    WeaponPrefab = Instantiate(enemyData.weapon.WeaponPrefab, ShieldSpawn.transform);
                    break;
                case "Sword":
                    WeaponPrefab = Instantiate(enemyData.weapon.WeaponPrefab, SwordSpawn.transform);
                    break;
            }
        }
    }
}