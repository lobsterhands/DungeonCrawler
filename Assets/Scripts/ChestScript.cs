using UnityEngine;
using System.Collections;

public class ChestScript : MonoBehaviour {

	/*
	 Chests TODO:
	 1. Spawn in proper locations, meaning it should look like a chest placement makes sense
	 2. Get a chest sprite with 2 lid states: closed and opened (can just modify the chest with a black rect to show opened
	*/

	bool chestOpened;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		chestOpened = false;
		spriteRenderer = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// TODO: Remove collisions on chest after opened
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (!chestOpened) {
			Debug.Log ("chest opened");
			chestOpened = true;
			spriteRenderer.color = new Color (255f, 0f, 0f, 1f); // Temporary to show if already opened
		}
	}
}
