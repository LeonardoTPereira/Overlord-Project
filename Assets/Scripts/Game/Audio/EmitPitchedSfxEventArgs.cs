namespace Game.Audio
{
    public class EmitPitchedSfxEventArgs : EmitSfxEventArgs
    {
        public int Pitch { get; set; }

        public EmitPitchedSfxEventArgs(AudioManager.SfxTracks specialEffect, int pitch) : base(specialEffect)
        {
            Pitch = pitch;
        }
    }
}