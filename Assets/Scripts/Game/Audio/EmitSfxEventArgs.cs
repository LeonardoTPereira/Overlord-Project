namespace Game.Audio
{
    public class EmitSfxEventArgs: EmitSoundEventArgs
    {
        public AudioManager.SfxTracks SpecialEffect { get; set; }
        public EmitSfxEventArgs(AudioManager.SfxTracks specialEffect)
        {
            SpecialEffect = specialEffect;
        }
    }
}