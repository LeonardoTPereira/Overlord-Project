using Game.LevelManager;
using ScriptableObjects;
using UnityEngine;

public class TreasureController : PlaceableRoomObject
{
    [field: SerializeField]
    public ItemSo Treasure { get; set; }
    [SerializeField]
    private AudioClip takenSnd;
    private AudioSource audioSrc;
    private bool canDestroy;

    public static event TreasureCollectEvent treasureCollectEvent;
    
    // Start is called before the first frame update
    private void Awake()
    {
        canDestroy = false;
        audioSrc = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (Treasure != null)
            GetComponent<SpriteRenderer>().sprite = Treasure.sprite;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!audioSrc.isPlaying && canDestroy)
        {
            Destroy(gameObject);
        }
    }
    private void DestroyTreasure()
    {
        audioSrc.PlayOneShot(takenSnd, 0.15f);
        canDestroy = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        OnTreasureCollect();
        DestroyTreasure();
    }

    protected void OnTreasureCollect()
    {
        treasureCollectEvent?.Invoke(this, new TreasureCollectEventArgs(Treasure.Value));
    }
}
