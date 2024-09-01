using System;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.Audio
{
    [Serializable]
    public class SfxNameToSoundDictionary : SerializableDictionaryBase<AudioManager.SfxTracks, Sound>
    {
    }
}