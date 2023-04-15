using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy.Movement
{
    public abstract class WolfMovementManager : MovementManager
    {
        public override void InitializeVariables() { }
        public override void Move(float moveDirection, float speed, bool canMove) { }
        public override void Attack() { }
        public override void Victory() { }
        public override void Death() { }
        public override void Flip() { }
        public override void Test() { }
    }
}
