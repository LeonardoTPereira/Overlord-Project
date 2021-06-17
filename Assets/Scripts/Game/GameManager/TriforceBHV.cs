using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriforceBHV : PlaceableRoomObject
{
    public static event EventHandler GotTriforceEventHandler;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GotTriforceEventHandler?.Invoke(this, EventArgs.Empty);
            SceneManager.LoadScene("LuanaPaolo"); //Luana
        }
    }
}
