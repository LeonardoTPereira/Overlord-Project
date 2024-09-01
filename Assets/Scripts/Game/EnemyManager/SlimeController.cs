using System.ComponentModel;
using Game.GameManager;
using UnityEngine;
using Util;

namespace Game.EnemyManager
{
    public class SlimeController : EnemyController
    {
        [field: SerializeField] private GameObject Eye { get; set; }

        protected override void Start()
        {
            base.Start();
            OriginalColor = GetColorBasedOnMovement();
            GetComponent<SpriteRenderer>().color = OriginalColor;
            Eye.GetComponent<SpriteRenderer>().color = GetEyeColorBasedOnMovement();
        }
        
        private Color GetEyeColorBasedOnMovement()
        {
            switch (EnemyData.movement.enemyMovementIndex)
            {
                case Enums.MovementEnum.Random:
                case Enums.MovementEnum.Random1D:
                    return enemyColorPalette.DetailColorA; 
                case Enums.MovementEnum.Flee1D:
                case Enums.MovementEnum.Flee:
                    return enemyColorPalette.DetailColorB;
                case Enums.MovementEnum.Follow1D:
                case Enums.MovementEnum.Follow:
                    return enemyColorPalette.DetailColorC;
                case Enums.MovementEnum.None:
                    return enemyColorPalette.DetailColorD; 
                default:
                    throw new InvalidEnumArgumentException("Movement Enum does not exist");
            }
        }
    }
}