using Game.Audio;
using Game.GameManager.Player;
using Game.Quests;
using ScriptableObjects;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using Util;
using Game.Events;

namespace Game.GameManager
{
    public class EnemyController : MonoBehaviour, IQuestElement, ISoundEmitter
    {
        [field: SerializeField] protected int IndexOnEnemyList { get; set; }
        [field: SerializeField] protected GameObject PlayerObj { get; set; }

        [SerializeField] private ParticleSystem bloodParticle;
        [field: SerializeField] protected ParticleSystem CureParticle { get; set; }

        private BehaviorType behavior;
        protected static readonly int DieTrigger = Animator.StringToHash("Die");
        private Animator _animator;
        private Color _originalColor;
        [field: SerializeField] protected ColorPaletteSo enemyColorPalette;
        public EnemySO EnemyData { get; set; }
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
        private HealthController _healthController;
        private Rigidbody2D _enemyRigidBody;
        private Collider2D[] _childrenCollider;
        private Collider2D _enemyCollider;
        private bool _isRandomMovement;

        public static event EventHandler PlayerHitEventHandler;
        public static event KillEnemyEvent KillEnemyEventHandler;

        private bool _hasGotComponents;

        public EventHandler<EnemySO> EnemyKilledHandler;

        private Coroutine _walkRoutine;


        protected virtual void Start()
        {
            if (!_hasGotComponents)
            {
                GetAllComponents();
            }
            _isRandomMovement = IsRandomMovement();
            _walkRoutine = StartCoroutine(WalkAndWait());
        }

        private void GetAllComponents()
        {
            _enemyCollider = GetComponent<Collider2D>();
            _childrenCollider = GetComponentsInChildren<Collider2D>();
            _animator = GetComponent<Animator>();
            _healthController = gameObject.GetComponent<HealthController>();
            _enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
            PlayerObj = Player.DungeonPlayer.Instance.gameObject;
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
            if (!_healthController.ApplyHeal(health)) return false;
            CureParticle.Play();
            return true;
        }

        public void ApplyDamageEffects(Vector3 impactDirection)
        {
            if (_healthController.GetHealth() <= 0) return;
            ((ISoundEmitter)this).OnSoundEmitted(this, new EmitSfxEventArgs(AudioManager.SfxTracks.EnemyHit));
            var mainParticle = bloodParticle.main;
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
            StartDeath();
        }

        private IEnumerator WalkAndWait()
        {
            while (true)
            {
                if (EnemyData is TopdownEnemySO ed)
                    yield return new WaitForSeconds(ed.restTime);
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
                if (EnemyData is TopdownEnemySO ed)
                    yield return new WaitForSeconds(ed.activeTime);
            }
            else
            {
                _enemyRigidBody.velocity = GetMovementVector(ref directionMask, true);
                if (EnemyData is TopdownEnemySO ed)
                    while (timeWalked < ed.activeTime)
                    {
                        _enemyRigidBody.velocity = GetMovementVector(ref directionMask, false);
                        timeWalked += Time.deltaTime;
                        yield return null;
                    }
            }
        }

        private bool IsRandomMovement()
        {
            return EnemyData.movement.name.Contains("Random");
        }

        private Vector2 GetMovementVector(ref Vector2 directionMask, bool updateMask)
        {
            int xOffset, yOffset;
            var playerPosition = (Vector2)PlayerObj.transform.position;
            var targetMoveDir = EnemyData.movement.movementType(playerPosition, gameObject.transform.position, ref directionMask, updateMask);
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

            if (EnemyData is TopdownEnemySO ed)
                return new Vector2(targetMoveDir.x * ed.movementSpeed, targetMoveDir.y * ed.movementSpeed);
            Debug.Log("EnemyData is not a TopdownEnemySO, check the enemy generator's output");
            return new Vector2();
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
            if (EnemyData is TopdownEnemySO ed)
                collision.gameObject.GetComponent<HealthController>().ApplyDamage(ed.damage, collisionDirection, IndexOnEnemyList);
        }

        public void CheckDeath()
        {
            if (_healthController.GetHealth() > 0f) return;
            StartDeath();
            InvokeEnemyKilledEvents();
        }

        protected virtual void StartDeath()
        {
            ((ISoundEmitter)this).OnSoundEmitted(this, new EmitSfxEventArgs(AudioManager.SfxTracks.EnemyDeath));
            StopCoroutine(_walkRoutine);
            _animator.SetTrigger(DieTrigger);
            _enemyCollider.enabled = false;
            _enemyRigidBody.velocity = Vector2.zero;
            foreach (var childCollider in _childrenCollider)
            {
                childCollider.enabled = false;
            }
        }

        private void InvokeEnemyKilledEvents()
        {
            EnemyKilledHandler?.Invoke(this, EnemyData);
            ((IQuestElement) this).OnQuestTaskResolved(this, new QuestKillEnemyEventArgs(EnemyData.weapon, QuestId));
            KillEnemyEventHandler?.Invoke(this, new KillEnemyEventArgs( EnemyData.movement.enemyMovementIndex, EnemyData.weapon.Type));
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        public virtual void LoadEnemyData(EnemySO enemyData, int questId)
        {
            if (!_hasGotComponents)
            {
                GetAllComponents();
            }
            EnemyData = enemyData;
            if (EnemyData is TopdownEnemySO ed)
                _healthController.SetHealth(ed.health);
            QuestId = questId;
        }

        protected Color GetColorBasedOnMovement()
        {
            switch (EnemyData.movement.enemyMovementIndex)
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