using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBHV : MonoBehaviour {

	public int x;
	public int y;
	public int id;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetPosition(int x, int y, float centerX, float centerY){
		this.x = x;
		this.y = y;
		transform.localPosition = new Vector2 (centerX, centerY);
	}
}
