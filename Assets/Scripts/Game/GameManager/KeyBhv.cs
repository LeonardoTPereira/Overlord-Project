using Game.Events;
using Game.LevelManager;
using UnityEngine;
using Util;

namespace Game.GameManager
{
    public class KeyBhv : PlaceableRoomObject
    {
        public int KeyID { get; set; }
        private Color color;

        public static event KeyCollectEvent KeyCollectEventHandler;

        // Use this for initialization
        private void Start()
        {
            //Render the key sprite with the color relative to its ID
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = Constants.colorId[KeyID - 1];
            color = sr.color;
        }

        private void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, 4);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("PlayerTrigger")) return;
            OnGetKey();
            Destroy(gameObject);
        }

        private void OnGetKey()
        {
            KeyCollectEventHandler?.Invoke(this, new KeyCollectEventArgs(KeyID));
        }
    }
}
