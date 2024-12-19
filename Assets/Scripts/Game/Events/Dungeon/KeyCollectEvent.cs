using System;

namespace Game.Events
{
    public delegate void KeyCollectEvent(object sender, KeyCollectEventArgs e);

    public class KeyCollectEventArgs : EventArgs
    {
        private int keyIndex;

        public KeyCollectEventArgs(int index)
        {
            KeyIndex = index;
        }
        public int KeyIndex { get => keyIndex; set => keyIndex = value; }
    }
}