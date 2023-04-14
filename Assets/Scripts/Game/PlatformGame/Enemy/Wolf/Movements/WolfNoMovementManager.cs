using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy.Movement
{
    public class WolfNoMovementManager : WolfMovementManager
    {
        public override void Move(float moveDirection, float speed) { }

        public override void Test()
        {
            Debug.Log("This is a Wolf NO MOVEMENT Test");
        }
    }
}
