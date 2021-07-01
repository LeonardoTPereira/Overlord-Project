using Game.LevelManager;
using UnityEngine;

public class KeyBHV : PlaceableRoomObject
{

    public int keyID;

    public static event KeyCollectEvent KeyCollectEventHandler;

    // Use this for initialization
    void Start()
    {
        //Render the key sprite with the color relative to its ID
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Util.colorId[keyID - 1];
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            OnGetKey();
            Destroy(gameObject);
        }
    }

    private void OnGetKey()
    {
        KeyCollectEventHandler(this, new KeyCollectEventArgs(keyID));
    }
}
