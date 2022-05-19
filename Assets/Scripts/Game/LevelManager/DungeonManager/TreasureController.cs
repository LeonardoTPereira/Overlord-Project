using Game.Audio;
using Game.Events;
using ScriptableObjects;
using UnityEngine;

namespace Game.LevelManager.DungeonManager
{
    public class TreasureController : PlaceableRoomObject, ISoundEmitter
    {
        [field: SerializeField]
        public ItemSo Treasure { get; set; }

        public static event TreasureCollectEvent TreasureCollectEventHandler;

        private void Start()
        {
            if (Treasure != null)
                GetComponent<SpriteRenderer>().sprite = Treasure.sprite;
        }
        
        private void DestroyTreasure()
        {
            ((ISoundEmitter)this).OnSoundEmitted(this, new EmitPitchedSfxEventArgs(AudioManager.SfxTracks.ItemPickup, 1));
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("PlayerTrigger")) return;
            OnTreasureCollect();
            DestroyTreasure();
        }

        private void OnTreasureCollect()
        {
            TreasureCollectEventHandler?.Invoke(this, new TreasureCollectEventArgs(Treasure.Value));
        }
    }
}
