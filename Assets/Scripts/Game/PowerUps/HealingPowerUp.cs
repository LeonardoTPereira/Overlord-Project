using Game.GameManager;
using UnityEngine;

namespace Game.PowerUps
{
    [CreateAssetMenu(menuName = "PowerUps/Healing")]
    public class HealingPowerUp : AbstractPowerUpEffect
    {
        [SerializeField] private float amount;

        public override void ApplyEffect(GameObject target)
        {
            var healthController = target.GetComponent<HealthController>();
            healthController.ApplyHeal((int)Amount);
        }
        
        public float Amount
        {
            get => amount;
            set => amount = value;
        }
    }
}