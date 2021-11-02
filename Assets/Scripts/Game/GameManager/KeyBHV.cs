using Game.LevelManager;
using UnityEngine;
using Util;

public class KeyBHV : PlaceableRoomObject
{

    public int keyID;
    private Color color;

    public static event KeyCollectEvent KeyCollectEventHandler;

    // Use this for initialization
    private void Start()
    {
        //Render the key sprite with the color relative to its ID
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Constants.colorId[keyID - 1];
        color = sr.color;
    }

    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 4);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
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
