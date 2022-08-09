using System.Collections;
using System.ComponentModel;
using Game.GameManager;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.EnemyManager
{
    public class MageController : EnemyController
    {
        [field: SerializeField] private ProjectileTypeSO ProjectileType { get; set; }
        [field: SerializeField] private float ProjectileSpeed { get; set; }
        [field: SerializeField] private GameObject ProjectilePrefab { get; set; }
        [field: SerializeField] protected float CooldownTime { get; set; }
        [field: SerializeField] private GameObject ProjectileSpawn { get; set; }
        [field: SerializeField] private GameObject HeadObject { get; set; }
        [field: SerializeField] private GameObject Eyes { get; set; }
        [field: SerializeField] private GameObject Hands { get; set; }
        [field: SerializeField] private float WaitForStartTimer { get; set; }

        protected override void Awake()
        {
            base.Awake();
            WaitForStartTimer = 0.5f;
        }
        protected override void Start()
        {
            base.Start();
            HeadObject.GetComponent<SpriteRenderer>().color = GetColorBasedOnMovement();
            StartCoroutine(BeginAttackRoutine());
        }

        protected override void StartDeath()
        {
            base.StartDeath();
            Hands.SetActive(false);
        }

        private IEnumerator BeginAttackRoutine()
        {
            yield return new WaitForSeconds(WaitForStartTimer);
            yield return StartCoroutine(UseSkill());
        }
        
        protected virtual IEnumerator UseSkill()
        {
            while (true)
            {
                var playerPosition = PlayerObj.transform.position;
                var thisPosition = transform.position;
                Vector2 target = new Vector2(playerPosition.x - thisPosition.x, playerPosition.y - thisPosition.y);
                target.Normalize();
                target *= ProjectileSpeed;
            
                GameObject bullet = Instantiate(ProjectilePrefab, ProjectileSpawn.transform.position, ProjectileSpawn.transform.rotation);
                if (ProjectilePrefab.name == "EnemyBomb")
                {
                    var bombController = bullet.GetComponent<BombController>();
                    bombController.ShootDirection = target;
                    bombController.Damage = Damage;
                    bombController.EnemyThatShot = IndexOnEnemyList;
                }
                else
                {
                    var projectileController = bullet.GetComponent<ProjectileController>();
                    projectileController.SetEnemyThatShot(IndexOnEnemyList);
                    projectileController.ProjectileSo = ProjectileType;
                    projectileController.Shoot(target);
                }
                yield return new WaitForSeconds(CooldownTime);
            }
        }

        public override void LoadEnemyData(EnemySO enemyData, int questId)      
        {
            base.LoadEnemyData(enemyData, questId);
            ProjectilePrefab = enemyData.weapon.projectile.projectilePrefab;
            ProjectileType = enemyData.weapon.projectile;
            if(ProjectilePrefab != null)
            {
                if (ProjectilePrefab.name == "EnemyBomb")
                {
                    AttackSpeed /= 2.0f;
                    SetColors(enemyColorPalette.MainColorA, enemyColorPalette.DetailColorA);
                }
                else
                {
                    SetColors(enemyColorPalette.MainColorB, enemyColorPalette.DetailColorB);

                }
            }
            else
            {
                SetColors(enemyColorPalette.MainColorC, enemyColorPalette.DetailColorC);
            }
            CooldownTime = 1.0f / AttackSpeed;
            ProjectileSpeed = enemyData.projectileSpeed * 4;
        }

        private void SetColors(Color mainColor, Color detailColor)
        {
            OriginalColor = mainColor;
            GetComponent<SpriteRenderer>().color = OriginalColor;
            Eyes.GetComponent<SpriteRenderer>().color = detailColor;
        }
    }
}