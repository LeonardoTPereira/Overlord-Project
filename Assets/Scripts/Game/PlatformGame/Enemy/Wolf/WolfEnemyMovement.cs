using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Util;

namespace PlatformGame.Enemy.Movement
{
    public class WolfEnemyMovement : EnemyMovement
    {
        protected override void GenerateMovementComponent(Enums.MovementEnum moveEnum)
        {
            switch (moveEnum)
            {
                case Enums.MovementEnum.None:
                    _moveManager = gameObject.AddComponent(typeof(WolfNoMovementManager)) as WolfNoMovementManager;
                    break;
                case Enums.MovementEnum.Random:
                    _moveManager = gameObject.AddComponent(typeof(WolfRandomMovementManager)) as WolfRandomMovementManager;
                    break;
                case Enums.MovementEnum.Random1D:
                    break;
                case Enums.MovementEnum.Flee1D:
                    break;
                case Enums.MovementEnum.Flee:
                    break;
                case Enums.MovementEnum.Follow1D:
                    break;
                case Enums.MovementEnum.Follow:
                    break;
                default:
                    throw new InvalidEnumArgumentException("Movement Enum does not exist");
            }
        }
    }
}
