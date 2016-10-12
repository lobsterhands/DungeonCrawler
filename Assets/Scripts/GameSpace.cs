using UnityEngine;
using System.Collections;

public class GameSpace : MonoBehaviour {
	DrawMaze drawMazeScript;
	int boardDimension;

	public GameObject[] collectibles;
	public GameObject player;
	public GameObject chest;
	public int numChests = 10;

	// Use this for initialization
	void Start () {
		drawMazeScript = this.GetComponent<DrawMaze>();
		boardDimension = drawMazeScript.dimSquare;

		Instantiate (player, new Vector2 (1, 1), Quaternion.identity);

		// Get collectible positions and place them
		Vector2 pacificerPos = getRandomPos (6);
		Vector2 bearPos = getRandomPos (14);
		Vector2 keysPos = getRandomPos (22);
		Vector2 ringPos = getRandomPos (28);
		Vector2 coffinPos = getRandomPos (32);
		Instantiate (collectibles [0], pacificerPos, Quaternion.identity);
		Instantiate (collectibles [1], bearPos, Quaternion.identity);
		Instantiate (collectibles [2], keysPos, Quaternion.identity);
		Instantiate (collectibles [3], ringPos, Quaternion.identity);
		Instantiate (collectibles [4], coffinPos, Quaternion.identity);

		// Instantiate chests
		for (int i = 0; i < drawMazeScript.endOfHallCoords.Count; i++) {

			int rand = UnityEngine.Random.Range (0, 3);

			if (rand == 0) {
				Instantiate (chest, drawMazeScript.endOfHallCoords [i], Quaternion.identity);
			}
		}

	}
	
	void Update () {

	}

	// getRandomPos() returns an odd value x and y to place a collectible.
	// An odd x and y ensures the item is placed on a floor
	Vector2 getRandomPos(int maxPoint) {
		int x;
		int y;
		int divisor = maxPoint / 5;
		if (divisor == 0) {
			divisor = 1;
		}

		do {
			x = Random.Range (0, maxPoint);
		} while (x % 2 == 0);

		do {
			y = (x <= (maxPoint / divisor)) ? Random.Range (0, maxPoint) : Random.Range (0, maxPoint / divisor);
		} while (y % 2 == 0);

		Vector2 maxVector = new Vector2 (maxPoint, maxPoint);
		Vector2 subVector = new Vector2 (x, y);
		Vector2 placeAtVector = maxVector - subVector;

		if (placeAtVector == new Vector2 (1, 1)) {
			int yNew;
			do {
				yNew = Random.Range (2, maxPoint);
			} while (yNew % 2 == 0);

			placeAtVector.y = yNew;
		}

		return placeAtVector;
	}

	Vector2 getChestPos()
	{
		float x;
		float y;

		do {
			x = Random.Range (1, boardDimension-1);
		} while (x % 2 == 0);

		do {
			y = Random.Range (1, boardDimension-1);
		} while (y % 2 == 0);

		x -= .4f;
		y += .4f;
		Vector2 placeAtVector = new Vector2 (x, y);

		return placeAtVector;
	}
}