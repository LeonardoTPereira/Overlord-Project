using UnityEngine;

namespace PlatformGame.PowerUps
{
    public class PowerUpController : MonoBehaviour
    {
        [SerializeField] private AbstractPowerUpEffect powerUpEffect;

        private void OnTriggerEnter2D(Collider2D collided)
        {
            if (!collided.CompareTag("Player")) return;
            PowerUpEffect.ApplyEffect(collided.gameObject);
            Destroy(gameObject);
        }

        public AbstractPowerUpEffect PowerUpEffect
        {
            get => powerUpEffect;
            set => powerUpEffect = value;
        }
    }
}