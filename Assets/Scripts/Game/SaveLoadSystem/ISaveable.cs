using System;

namespace Game.SaveLoadSystem
{
    public interface ISaveable
    {
        object SaveState();
        void LoadState(object state);
    }
}