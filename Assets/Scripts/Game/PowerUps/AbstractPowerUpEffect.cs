using UnityEngine;

namespace Game.PowerUps
{
    public abstract class AbstractPowerUpEffect : ScriptableObject
    {
        public abstract void ApplyEffect(GameObject target);
    }
}