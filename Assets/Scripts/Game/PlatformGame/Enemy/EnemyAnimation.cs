using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy
{
    public abstract class EnemyAnimation : MonoBehaviour
    {
        public abstract void AnimateMove(bool isRunning, float speed, bool canMove);
        public abstract void AnimateAttack();
        public abstract void AnimateVictory();
        public abstract void AnimateDeath();
    }
}
