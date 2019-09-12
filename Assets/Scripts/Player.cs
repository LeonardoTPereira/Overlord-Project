using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlaceableRoomObject {

	public static Player instance = null;
	public List<int> keys = new List<int>();
    public List<int> usedKeys = new List<int>();
	public int x { private set;  get;}
	public int y { private set;  get;}
	public Camera cam;
    private AudioSource audioSrc;

	void Awake(){
		if (instance == null){
			instance = this;
            audioSrc = GetComponent<AudioSource>();
        } else if (instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
        
    }


	// Use this for initialization
	void Start () {
        cam = Camera.main;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GetKey(int keyID){
        audioSrc.PlayOneShot(audioSrc.clip, 0.6f);
		keys.Add (keyID);
	}

	public void AdjustCamera(int x, int y){
		GameManager gm = GameManager.instance;
		Transform roomTransf = gm.roomBHVMap [x, y].transform;
		cam.transform.position = new Vector3 (roomTransf.position.x, roomTransf.position.y, -10f);
	}
		
}
