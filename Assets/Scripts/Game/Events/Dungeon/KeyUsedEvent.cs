using System;

namespace Game.Events
{
    public delegate void KeyUsedEvent(object sender, KeyUsedEventArgs e);

    public class KeyUsedEventArgs : EventArgs
    {
        private int keyIndex;

        public KeyUsedEventArgs(int index)
        {
            KeyIndex = index;
        }
        public int KeyIndex { get => keyIndex; set => keyIndex = value; }
    }
}