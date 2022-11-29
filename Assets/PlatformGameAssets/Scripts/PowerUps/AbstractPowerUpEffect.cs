using UnityEngine;

namespace PlatformGame.PowerUps
{
    public abstract class AbstractPowerUpEffect : ScriptableObject
    {
        public abstract void ApplyEffect(GameObject target);
    }
}