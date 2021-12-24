using System;
using ScriptableObjects;

namespace Game.Events
{
    public delegate void LoadWeaponButtonEvent(object sender, LoadWeaponButtonEventArgs eventArgs);
    public class LoadWeaponButtonEventArgs : EventArgs
    {
        protected ProjectileTypeSO projectileSO;

        public LoadWeaponButtonEventArgs(ProjectileTypeSO projectileSO)
        {
            ProjectileSO = projectileSO;
        }
        public ProjectileTypeSO ProjectileSO { get => projectileSO; set => projectileSO = value; }
    }
}