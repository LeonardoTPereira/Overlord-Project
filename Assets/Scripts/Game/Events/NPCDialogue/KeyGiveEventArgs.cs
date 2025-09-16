using System;
using ScriptableObjects;

namespace Game.Events
{
    public delegate void KeyGiveEvent(object sender, KeyGiveEventArgs e);

    public class KeyGiveEventArgs : EventArgs
    {
        public int KeyId { get; set; }
        public KeyGiveEventArgs(int keyId)
        {
            KeyId = keyId;
        }
    }
}