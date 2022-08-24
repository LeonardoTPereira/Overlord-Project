using System;
using System.Collections;
using Game.GameManager;
using MyBox;
using ScriptableObjects;
using UnityEngine;

namespace Game.EnemyManager
{
    public class HealerController : MageController
    {
        private const float BaseCooldown = 2f;
        private const float HealAreaCooldown = 0.75f;
        private const int Health = 1;
        [field: SerializeField] private SpriteRenderer healArea;
        [field: SerializeField] private Collider2D _healerCollider;
        private Collider2D[] _collisionResults = new Collider2D[1000];


        /// Awake is called when the script instance is being loaded.
        protected override void Awake()
        {
            base.Awake();
            HideHealArea();
        }
        
        private void HideHealArea()
        {
            healArea.SetAlpha(0);
        }

        private void ShowHealArea()
        {
            healArea.SetAlpha(0.2f);
        }

        protected override IEnumerator UseSkill()
        {
            while (true)
            {
                var hitCount = _healerCollider.GetContacts(_collisionResults);
                var healed = false;
                for (var i = 0; i < hitCount; ++i)
                {
                    if (!_collisionResults[i].CompareTag("Enemy")) continue;
                    if(!_collisionResults[i].gameObject.GetComponent<EnemyController>().Heal(Health)) continue;
                    healed = true;
                }
                if (healed)
                {
                    ShowHealArea();
                    yield return new WaitForSeconds(HealAreaCooldown);
                    HideHealArea();
                }
                yield return new WaitForSeconds(CooldownTime);
            }
        }

        public override bool Heal(int health)
        {
            return false;
        }

        public override void LoadEnemyData(EnemySO enemyData, int questId)
        {
            base.LoadEnemyData(enemyData, questId);
            CooldownTime = BaseCooldown * (1f / EnemyData.attackSpeed);
        }
    }
}