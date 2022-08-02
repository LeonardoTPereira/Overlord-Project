using System;
using System.Linq;
using ScriptableObjects;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.EnemyManager
{
    [Serializable]
    public class EnemyByAmountDictionary : SerializableDictionaryBase<EnemySO, int>
    {
        public void RemoveEnemy(WeaponTypeSO killedEnemyWeapon)
        {
            for (var i = Count-1; i > -1; i--)
            {
                var enemy = Keys.ElementAt(i);
                if (enemy.weapon != killedEnemyWeapon) continue;
                this[enemy]--;
                if (this[enemy] == 0)
                {
                    Remove(enemy);
                }
                
                break;
            }
        }
    }
}