using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class npcBehaviour : MonoBehaviour
{
    public Player player;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isColliding == true) canvas.enabled = true;
    }
}
