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
                    _itFlips = true;
                    moveManager = gameObject.AddComponent(typeof(NoMovementManager)) as NoMovementManager;
                    break;
                case Enums.MovementEnum.Random:
                    //_moveManager = gameObject.AddComponent(typeof(WolfRandomMovementManager)) as WolfRandomMovementManager;
                    break;
                case Enums.MovementEnum.Random1D:
                    break;
                case Enums.MovementEnum.Flee1D:
                    _itFlips = true;
                    _flipsInOpositeDirection = true;
                    moveManager = gameObject.AddComponent(typeof(Flee1DMovementManager)) as Flee1DMovementManager;
                    break;
                case Enums.MovementEnum.Flee:
                    moveManager = gameObject.AddComponent(typeof(FleeMovementManager)) as FleeMovementManager;
                    _itFlips = true;
                    _flipsInOpositeDirection = true;
                    break;
                case Enums.MovementEnum.Follow1D:
                    _itFlips = true;
                    moveManager = gameObject.AddComponent(typeof(Follow1DMovementManager)) as Follow1DMovementManager;
                    break;
                case Enums.MovementEnum.Follow:
                    _itFlips = true;
                    moveManager = gameObject.AddComponent(typeof(FollowMovementManager)) as FollowMovementManager;
                    break;
                default:
                    throw new InvalidEnumArgumentException("Movement Enum does not exist");
            }
        }
    }
}
