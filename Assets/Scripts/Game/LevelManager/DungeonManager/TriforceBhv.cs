using System;
using UnityEngine;

namespace Game.LevelManager.DungeonManager
{
    public class TriforceBhv : PlaceableRoomObject
    {
        public static event EventHandler GotTriforceEventHandler;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerTrigger"))
            {
                GotTriforceEventHandler?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
