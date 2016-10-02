using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	public float speed = 3f;
	int itemsCollected = 0;
	float smooth = 0.05f; // Used for camera lerp

	public GameObject gameSpaceObj;
	public GameSpace gameSpaceScript;

	public GameObject itemCollected;
	public AudioSource itemCollectedAudioSource;
	public AudioClip itemCollectedClip;

	public GameObject winAudio;
	public AudioSource winAudioSource;
	public AudioClip winClip;

	Animator animator;
	bool running;

	new public GameObject camera;

	void Start() {
		// 2-step process to getting a game object and only then referencing a script
		// Apparently, must use a GameObject to "get" a script from
		gameSpaceObj = GameObject.Find ("Maze");
		gameSpaceScript = gameSpaceObj.GetComponent<GameSpace> ();

		itemCollected = GameObject.FindGameObjectWithTag ("ItemCollected");
		itemCollectedAudioSource = itemCollected.GetComponent<AudioSource> ();
		itemCollectedClip = itemCollectedAudioSource.clip;

		winAudio = GameObject.FindGameObjectWithTag ("YouWin");
		winAudioSource = winAudio.GetComponent<AudioSource> ();
		winClip = winAudioSource.clip;

		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.transform.position = this.transform.position + new Vector3 (0f, 0f, -7.5f);
		camera.transform.parent = this.transform;

		animator = GetComponent<Animator> ();
	}

	float rotatePlayerX = 0F;
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		if (moveHorizontal != 0 || moveVertical != 0) { // If the player is moving
			running = true;
		} else {
			running = false;
		}

		if (moveHorizontal > 0) {
			rotatePlayerX = -90F;
		} else if (moveHorizontal < 0) {
			rotatePlayerX = 90F;
		} else if (moveVertical > 0) {
			rotatePlayerX = 0F;
		}  else if (moveVertical < 0) {
			rotatePlayerX = 180F;
		}

		animator.SetBool ("Running", running);

		Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
		GetComponent<Rigidbody2D> ().velocity = movement * speed;

		gameObject.transform.rotation = Quaternion.Euler (0, 0, rotatePlayerX);
		camera.transform.rotation = Quaternion.Euler (0, 0, 0); // Keep child-object camera from rotating
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Destroy (collider.gameObject);
		if (collider.tag == "Keys") {
			speed = 4f;
		}

		Vector3 newCameraPos = camera.transform.position + new Vector3 (0, 0, -500f);

		camera.transform.position = Vector3.Lerp (camera.transform.position, newCameraPos, Time.deltaTime * smooth);

		itemsCollected++;
		if (itemsCollected < 5) {
			itemCollectedAudioSource.PlayOneShot (itemCollectedClip);
		} else {
			itemCollectedAudioSource.PlayOneShot (winClip);
		}
	}
}