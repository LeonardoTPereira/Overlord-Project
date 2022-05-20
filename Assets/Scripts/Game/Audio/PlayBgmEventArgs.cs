namespace Game.Audio
{
    public class PlayBgmEventArgs : EmitSoundEventArgs
    {
        public AudioManager.BgmTracks BackGroundMusic { get; set; }
        public PlayBgmEventArgs(AudioManager.BgmTracks backGroundMusic)
        {
            BackGroundMusic = backGroundMusic;
        }
    }
}