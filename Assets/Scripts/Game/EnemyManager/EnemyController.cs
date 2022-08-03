using System;
using System.Collections;
using System.ComponentModel;
using Game.Audio;
using Game.GameManager.Player;
using Game.LevelManager.DungeonManager;
using Game.Quests;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.GameManager
{
    public class EnemyController : MonoBehaviour, IQuestElement, ISoundEmitter
    {
        [SerializeField]
        private float restTime, activeTime, movementSpeed;
        [field: SerializeField] protected float AttackSpeed { get; set; }
        [field: SerializeField] protected int Damage { get; set; }
        [field: SerializeField] protected int IndexOnEnemyList { get; set; }
        [field: SerializeField] protected GameObject PlayerObj { get; set; }

        [SerializeField] private ParticleSystem bloodParticle;
        [field: SerializeField] protected ParticleSystem CureParticle { get; set; }

        [field: SerializeField] protected MovementTypeSO Movement { get; set; }
        private BehaviorType behavior;
        protected WeaponTypeSO EnemyWeapon { get; set; }
        protected static readonly int DieTrigger = Animator.StringToHash("Die");
        private Animator _animator;
        
        private bool _hasProjectile;
        private Color _originalColor;
        [field: SerializeField] protected ColorPaletteSo enemyColorPalette;
        public int QuestId { get; set; }

        private Vector2 _directionMask;

        protected Color OriginalColor
        {
            get => _originalColor;
            set
            {
                _originalColor = value;
                if (_healthController != null)
                {
                    _healthController.SetOriginalColor(_originalColor);
                }
            }
        }
        private float _lastX, _lastY;
        private RoomBhv _room;
        private HealthController _healthController;
        private Rigidbody2D _enemyRigidBody;
        private Collider2D[] _childrenCollider;
        private Collider2D _enemyCollider;
        private bool _isRandomMovement;

        public static event EventHandler PlayerHitEventHandler;
        public static event EventHandler KillEnemyEventHandler;

        private bool _hasGotComponents;

        protected virtual void Start()
        {
            if (!_hasGotComponents)
            {
                GetAllComponents();
            }
            _isRandomMovement = IsRandomMovement();
            StartCoroutine(WalkAndWait());
        }

        private void GetAllComponents()
        {
            _enemyCollider = GetComponent<Collider2D>();
            _childrenCollider = GetComponentsInChildren<Collider2D>();
            _animator = GetComponent<Animator>();
            _healthController = gameObject.GetComponent<HealthController>();
            _enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
            PlayerObj = Player.Player.Instance.gameObject;
            _hasGotComponents = true;
        }

        protected virtual void Awake()
        {
            _hasGotComponents = false;
        }

        private void OnEnable()
        {
            PlayerController.PlayerDeathEventHandler += PlayerHasDied;
        }

        private void OnDisable()
        {
            PlayerController.PlayerDeathEventHandler -= PlayerHasDied;
        }

        private void OnPlayerHit()
        {
            PlayerHitEventHandler?.Invoke(null, EventArgs.Empty);
        }
        
        public virtual bool Heal(int health)
        {
            if(!_healthController.ApplyHeal(health)) return false;
            CureParticle.Play();
            return true;
        }

        public void ApplyDamageEffects(Vector3 impactDirection)
        {
            if (_healthController.GetHealth() <= 0) return;
            ((ISoundEmitter)this).OnSoundEmitted(this, new EmitSfxEventArgs(AudioManager.SfxTracks.EnemyHit));
            var mainParticle= bloodParticle.main;
            mainParticle.startSpeed = 0;
            var forceOverLifetime = bloodParticle.forceOverLifetime;
            forceOverLifetime.enabled = true;
            forceOverLifetime.x = impactDirection.x * 40;
            forceOverLifetime.y = impactDirection.y * 40;
            forceOverLifetime.z = impactDirection.z * 40;
            bloodParticle.Play();
        }

        private void PlayerHasDied(object sender, EventArgs eventArgs)
        {
            StopAllCoroutines();
        }

        private IEnumerator WalkAndWait()
        {
            while (true)
            {
                yield return new WaitForSeconds(restTime);
                yield return StartCoroutine(Walk());
                Wait();
            }
        }

        private IEnumerator Walk()
        {
            var timeWalked = 0.0f;
            var directionMask = new Vector2();
            if (_isRandomMovement)
            {
                _enemyRigidBody.velocity = GetMovementVector(ref directionMask, true);
                yield return new WaitForSeconds(activeTime);
            }
            else
            {
                _enemyRigidBody.velocity = GetMovementVector(ref directionMask, true);
                while (timeWalked < activeTime)
                {
                    _enemyRigidBody.velocity = GetMovementVector(ref directionMask, false);
                    timeWalked += Time.deltaTime;
                    yield return null;
                }
            }
        }

        private bool IsRandomMovement()
        {
            return Movement.name.Contains("Random");
        }

        private Vector2 GetMovementVector(ref Vector2 directionMask, bool updateMask)
        {
            int xOffset, yOffset;
            var playerPosition = (Vector2)PlayerObj.transform.position;
            var targetMoveDir = Movement.movementType(playerPosition, gameObject.transform.position, ref directionMask, updateMask);
            targetMoveDir.Normalize();
            if (targetMoveDir.x > 0)
                xOffset = 1;
            else if (targetMoveDir.x < 0)
                xOffset = -1;
            else
                xOffset = 0;
            if (targetMoveDir.y > 0)
                yOffset = 1;
            else if (targetMoveDir.y < 0)
                yOffset = -1;
            else
                yOffset = 0;
            targetMoveDir = new Vector2((targetMoveDir.x + xOffset), (targetMoveDir.y + yOffset));
            return new Vector2(targetMoveDir.x * movementSpeed, targetMoveDir.y * movementSpeed);
        }

        private void Wait()
        {
            _enemyRigidBody.velocity = Vector3.zero;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            var collisionDirection = Vector3.Normalize(gameObject.transform.position - collision.gameObject.transform.position);
            if (!collision.gameObject.CompareTag("Player")) return;
            OnPlayerHit();
            collision.gameObject.GetComponent<HealthController>().ApplyDamage(Damage, collisionDirection, IndexOnEnemyList);
        }

        public void CheckDeath()
        {
            if (_healthController.GetHealth() > 0f) return;
            StartDeath();
        }

        protected virtual void StartDeath()
        {
            ((ISoundEmitter) this).OnSoundEmitted(this, new EmitSfxEventArgs(AudioManager.SfxTracks.EnemyDeath));
            _animator.SetTrigger(DieTrigger);
            _enemyCollider.enabled = false;
            foreach (var childCollider in _childrenCollider)
            {
                childCollider.enabled = false;
            }
            ((IQuestElement) this).OnQuestTaskResolved(this, new QuestKillEnemyEventArgs(EnemyWeapon, QuestId));
            KillEnemyEventHandler?.Invoke(null, EventArgs.Empty);
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        public virtual void LoadEnemyData(EnemySO enemyData)
        {
            if (!_hasGotComponents)
            {
                GetAllComponents();
            }
            _healthController.SetHealth(enemyData.health);
            Damage = enemyData.damage;
            movementSpeed = enemyData.movementSpeed;
            restTime = enemyData.restTime;
            activeTime = enemyData.activeTime;
            AttackSpeed = enemyData.attackSpeed;
            EnemyWeapon = enemyData.weapon;
            Movement = enemyData.movement;
            behavior = enemyData.behavior.enemyBehavior;
        }

        public void SetRoom(RoomBhv room)
        {
            _room = room;
        }
        
        protected Color GetColorBasedOnMovement()
        {
            switch (Movement.enemyMovementIndex)
            {
                case Enums.MovementEnum.Random:
                case Enums.MovementEnum.Random1D:
                    return enemyColorPalette.OutfitColorA; 
                case Enums.MovementEnum.Flee1D:
                case Enums.MovementEnum.Flee:
                    return enemyColorPalette.OutfitColorB;
                case Enums.MovementEnum.Follow1D:
                case Enums.MovementEnum.Follow:
                    return enemyColorPalette.OutfitColorC;
                case Enums.MovementEnum.None:
                    return enemyColorPalette.OutfitColorD; 
                default:
                    throw new InvalidEnumArgumentException("Movement Enum does not exist");
            }
        }

    }
}
