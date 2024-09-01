using System;
using System.Collections;
using System.Threading.Tasks;
using Game.Audio;
using Game.GameManager.Player;
using UnityEngine;

namespace Game.GameManager
{
    public class BombController : MonoBehaviour, ISoundEmitter
    {
        private Rigidbody2D _bombRigidBody;
        private Animator _animator;
        private CircleCollider2D _bombCollider;

        private bool _hasBeenThrown, _hasTimerBeenSet, _isExploding;
        public int Damage { get; set; }
        public int EnemyThatShot { get; set; }

        [SerializeField]
        private float bombLifetime;

        public static event EventHandler PlayerHitEventHandler;

        private static readonly int Explode = Animator.StringToHash("Explode");

        private Collider2D[] _objectsInRange;
        public Vector2 ShootDirection { get; set; }

        private void Awake()
        {
            bombLifetime = 2.0f;
            _objectsInRange = new Collider2D[20];
        }

        private void OnEnable()
        {
            PlayerController.PlayerDeathEventHandler += PlayerHasDied;
        }

        private void OnDisable()
        {
            PlayerController.PlayerDeathEventHandler -= PlayerHasDied;
        }

        private void Start()
        {
            _bombRigidBody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _bombCollider = GetComponent<CircleCollider2D>();
            _bombCollider.enabled = false;
            _bombRigidBody.AddForce(ShootDirection, ForceMode2D.Impulse);
            StartCoroutine(ExplodeAfterLifetime());
        }

        private void PlayerHasDied(object sender, EventArgs eventArgs)
        {
            ExplodeBomb();
        }

        public void DestroyBomb()
        {
            Destroy(gameObject);
        }

        private IEnumerator ExplodeAfterLifetime()
        {
            yield return new WaitForSeconds(bombLifetime/100);
            _bombCollider.enabled = true;
            yield return new WaitForSeconds((int)bombLifetime);
            ExplodeBomb();
        }

        private static void OnPlayerHit()
        {
            PlayerHitEventHandler?.Invoke(null, EventArgs.Empty);
        }

        private void ExplodeBomb()
        {
            _animator.SetTrigger(Explode);
            ((ISoundEmitter)this).OnSoundEmitted(this, new EmitSfxEventArgs(AudioManager.SfxTracks.Explosion));
            var transform1 = transform;
            var currScale = transform1.localScale;
            transform1.localScale = new Vector3(currScale.x * 4, currScale.y * 4, currScale.z * 1);
            var position = _bombRigidBody.position;
            var size = Physics2D.OverlapCircleNonAlloc(new Vector2(position.x, position.y), 1.8f, _objectsInRange);
            for(var i=0; i < size; ++i)
            {
                if (!_objectsInRange[i].gameObject.CompareTag("Player")) continue;
                var collisionDirection =
                    Vector3.Normalize(_objectsInRange[i].gameObject.transform.position - gameObject.transform.position);
                OnPlayerHit();
                _objectsInRange[i].gameObject.GetComponent<HealthController>().ApplyDamage(Damage, collisionDirection, EnemyThatShot);
            }
        }
    }
}
