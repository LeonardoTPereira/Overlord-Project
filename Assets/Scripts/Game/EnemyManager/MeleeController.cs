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
        
        public override void LoadEnemyData(EnemySO enemyData)
        {
            if (enemyData.weapon.name == "Shield")
                WeaponPrefab = Instantiate(enemyData.weapon.weaponPrefab, ShieldSpawn.transform);
            else if (enemyData.weapon.name == "Sword")
                WeaponPrefab = Instantiate(enemyData.weapon.weaponPrefab, SwordSpawn.transform);
        }
    }
}