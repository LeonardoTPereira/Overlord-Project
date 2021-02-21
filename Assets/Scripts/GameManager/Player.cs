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
	public Camera minimap;
    private AudioSource audioSrc;
	public Mission_Log mission;
	public GameObject npc;
	public bool isColliding = false;

	void Awake(){
		if (instance == null){
			instance = this;
            audioSrc = GetComponent<AudioSource>();

		} else if (instance != this){
			Destroy (gameObject);
		}
		//DontDestroyOnLoad (gameObject);
        
    }

	private void OnEnable()
	{
		GameManager.newLevelLoadedEvent += ResetValues;
	}

	private void OnDisable()
	{
		GameManager.newLevelLoadedEvent -= ResetValues;
	}

	// Use this for initialization
	void Start () {
        cam = Camera.main;
		npc.SetActive(false);
		if(mission.type[mission.index1] == 0){
			npc.SetActive(true);
			mission.index1++;
			mission.index2++;
		}
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
		cam.transform.position = new Vector3 (roomTransf.position.x + Room.sizeX/3.5f, roomTransf.position.y, -5f);
		//minimap.transform.position = new Vector3(roomTransf.position.x, roomTransf.position.y, -5f);
	}

	void ResetValues()
	{
		keys.Clear();
		usedKeys.Clear();
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Npc") isColliding = true;
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Npc") isColliding = false;
	}
}
