using System;
using static Util.Enums;

namespace Game.Events
{
    public delegate void KillEnemyEvent(object sender, KillEnemyEventArgs e);

    public class KillEnemyEventArgs : EventArgs
    {
        public string EnemyTypeString => _movementEnum.ToString() + _weaponTypeEnum.ToString();
        private MovementEnum _movementEnum;
        private WeaponTypeEnum _weaponTypeEnum;
        public KillEnemyEventArgs(MovementEnum movementEnum, WeaponTypeEnum weaponTypeEnum)
        {
            _movementEnum = movementEnum;
            _weaponTypeEnum = weaponTypeEnum;
        }
    }
}