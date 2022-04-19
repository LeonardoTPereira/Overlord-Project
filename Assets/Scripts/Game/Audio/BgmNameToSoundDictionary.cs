using System;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.Audio
{
    [Serializable]
    public class BgmNameToSoundDictionary : SerializableDictionaryBase<AudioManager.BgmTracks, Sound>
    {
    }
}