using PlatformGame.Player;
using UnityEngine;

namespace PlatformGame.PowerUps
{
    [CreateAssetMenu(menuName = "Platform/PowerUps/Healing")]
    public class HealingPowerUp : AbstractPowerUpEffect
    {
        [SerializeField] private float amount;

        public override void ApplyEffect(GameObject target)
        {
            var healthController = target.GetComponent<PlayerHealth>();
            healthController.ApplyHeal((int)Amount);
        }
        
        public float Amount
        {
            get => amount;
            set => amount = value;
        }
    }
}