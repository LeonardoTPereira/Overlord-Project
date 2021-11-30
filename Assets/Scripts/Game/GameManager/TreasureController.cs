﻿using Game.LevelManager;
using ScriptableObjects;
using UnityEngine;

public class TreasureController : PlaceableRoomObject
{
    [SerializeField]
    public ItemSo Treasure { get; set; }
    [SerializeField]
    private AudioClip takenSnd;
    private AudioSource audioSrc;
    private bool canDestroy;

    public static event TreasureCollectEvent treasureCollectEvent;
    
    // Start is called before the first frame update
    void Awake()
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
    void Update()
    {
        if (!audioSrc.isPlaying && canDestroy)
        {
            Destroy(gameObject);
        }
    }
    public void DestroyTreasure()
    {
        //Debug.Log("Destroying Bullet");
        audioSrc.PlayOneShot(takenSnd, 0.15f);
        canDestroy = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnTreasureCollect();
            DestroyTreasure();
        }
    }

    protected void OnTreasureCollect()
    {
        Debug.Log("Collected the treasure");
        treasureCollectEvent(this, new TreasureCollectEventArgs(Treasure.value)); //Luana e Paolo
    }
}
