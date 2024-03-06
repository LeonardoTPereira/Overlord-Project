﻿using ScriptableObjects.SerializableDictionaryLite;
using System;

namespace Game.Audio
{
    [Serializable]
    public class BgmNameToSoundDictionary : SerializableDictionaryBase<AudioManager.BgmTracks, Sound>
    {
    }
}