using System;

namespace Game.Audio
{
    public delegate void EmitSoundEvent(object sender, EmitSoundEventArgs e);
    public abstract class EmitSoundEventArgs : EventArgs
    {
    }
}