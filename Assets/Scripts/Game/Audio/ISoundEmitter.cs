namespace Game.Audio
{
    public interface ISoundEmitter
    {
        public static event EmitSoundEvent EmitSoundEventHandler;
        public void OnSoundEmitted(object sender, EmitSoundEventArgs args)
        {
            EmitSoundEventHandler?.Invoke(sender, args);
        }
    }
}