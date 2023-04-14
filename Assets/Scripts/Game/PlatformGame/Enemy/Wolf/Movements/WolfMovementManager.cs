using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy.Movement
{
    public abstract class WolfMovementManager : MovementManager
    {
        public override void Move(float speed, bool canMove) { }
        public override void Attack() { }
        public override void Victory() { }
        public override void Death() { }

        public override void Test() { }
    }
}
