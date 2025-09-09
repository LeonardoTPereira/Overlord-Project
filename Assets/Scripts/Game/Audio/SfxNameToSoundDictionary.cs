using ScriptableObjects.SerializableDictionaryLite;
using System;

namespace Game.Audio
{
    [Serializable]
    public class SfxNameToSoundDictionary : SerializableDictionaryBase<AudioManager.SfxTracks, Sound>
    {
    }
}