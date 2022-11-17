using Game.Audio;
using Game.Events;
using UnityEngine;
using Util;

namespace Game.LevelManager.DungeonManager
{
    public class KeyBhv : PlaceableRoomObject, ISoundEmitter
    {
        public int KeyID { get; set; }
        [field: SerializeField] public GameObject MinimapIcon { get; set; }
        private SpriteRenderer _minimapSprite;
        private Color color;
        

        public static event KeyCollectEvent KeyCollectEventHandler;

        // Use this for initialization
        private void Start()
        {
            //Render the key sprite with the color relative to its ID
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = Constants.ColorId[KeyID - 1];
            color = sr.color;
            _minimapSprite = MinimapIcon.GetComponent<SpriteRenderer>();
            _minimapSprite.enabled = false;
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
            ((ISoundEmitter)this).OnSoundEmitted(this, new EmitPitchedSfxEventArgs(AudioManager.SfxTracks.ItemPickup, 1));
            KeyCollectEventHandler?.Invoke(this, new KeyCollectEventArgs(KeyID));
        }

        public void ShowKeyMinimapIcon()
        {
	        _minimapSprite.color = color;
	        _minimapSprite.enabled = true;
        }
    }
}
