/*
 * DFS Maze algorithm with much help from: www.migapro.com/depth-first-search
 */

using UnityEngine;
using System; // Allows Convert from binary string to integer
using System.Collections;
using System.Collections.Generic; // Allows for use of List<> data type

public class DrawMaze : MonoBehaviour {

	// Board dimensions variables
	public int dimSquare;
	int width;
	int height;

	// Variables to avoid out of bounds errors
	int max;

	// 2-dimensional arrays to hold maze information
	int[,] gameMaze; // Holds info returned by GenerateMaze();
	int[,] maze; // Used in generateMaze(); @LYLE: Can these be condensed?

	public GameObject[] mazeSprites; // Create array to hold sprites
	GameObject child; // Used to attach maze objects as children of Maze (for organization)

	// Use this for initialization
	void Start () {
		// Set board dimensions
		width = height = dimSquare = 33;
		max = dimSquare - 1;

		gameMaze = generateMaze ();

		// Place the floor
		for (int x = 0; x < dimSquare; ++x) {
			for (int y = 0; y < dimSquare; ++y) {
				if (gameMaze [x, y] == 0) { // If the object is a floor
					drawTile (0, x, y); // Instantiate floor
				} else { // The object is a wall
					// Place wall corners
					if (x == 0 && y == 0) { // Place bottom left corner
						drawTile (12, x, y);
					} else if (x == max && y == 0) { // Place bottom right corner
						drawTile (9, x, y);
					} else if (x == 0 && y == max) { // Place top left corner
						drawTile (6, x, y);
					} else if (x == max && y == max) { // Place top right corner
						drawTile (3, x, y);
					} // Done placing corners

					// If wall is an outer edge, place them
					if (y - 1 < 0 || y + 1 > max || x - 1 < 0 || x + 1 > max) {
						if (y - 1 < 0 && x > 0 && x < max) { // On the bottom row
							if (gameMaze [x + 1, y] == 1 && gameMaze [x, y + 1] == 1 && gameMaze [x - 1, y] == 1) {
								// Place URL connecting wall
								drawTile (13, x, y); 
							} else { // Place RL wall
								drawTile (5, x, y);
							}
						} else if (y + 1 > max && x > 0 && x < max) { // On the top row
							// Place RDL connecting wall
							if (gameMaze [x + 1, y] == 1 && gameMaze [x, y - 1] == 1 && gameMaze [x - 1, y] == 1) {
								drawTile (7, x, y);
							} else { // Place RL wall
								drawTile (5, x, y);
							}
						} else if (x - 1 < 0 && y > 0 && y < max) { // On the leftmost column
							// Place URD connecting wall
							if (gameMaze [x + 1, y] == 1 && gameMaze [x, y - 1] == 1 && gameMaze [x, y + 1] == 1) {
								drawTile (14, x, y);
							} else {
								drawTile (10, x, y); // Place UD wall
							}
						} else if (x + 1 > max && y > 0 && y < max) { // On rightmost column
							// Place UDL connecting wall
							if (gameMaze [x, y + 1] == 1 && gameMaze [x - 1, y] == 1 && gameMaze [x, y - 1] == 1) {
								drawTile (11, x, y);
							} else {
								drawTile (10, x, y); // Place UD wall
							}
						}
					} else { // Place inner walls
						int tileElement = getWallTile (x, y);
						drawTile (tileElement, x, y);
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}

	// getWallTile() accepts a gameMaze objects x and y values and returns
	// an integer based on the binary sequence of it's Up Right Down Left neighbors;
	// 0 represents a floor; 1, a wall. Example: 1100 means a tile has a wall to the Up and Right of it.
	// 1100 (base 2) tu rns into 12 (base 10)
	int getWallTile(int x, int y) {
		string binaryString = gameMaze [x, y + 1].ToString() + gameMaze[x + 1, y].ToString() + 
			gameMaze[x, y - 1].ToString() + gameMaze[x - 1, y].ToString();
		int elementNum = Convert.ToInt16 (binaryString, 2);
		return elementNum;
	}

	// drawTile() takes a tileElement and instantiates the matching gameMaze element
	void drawTile(int tileElement, int x, int y) {
		child = Instantiate (mazeSprites [tileElement], new Vector2 (x, y), Quaternion.identity) as GameObject;
		child.transform.parent = this.transform;
	}

	public int[,] generateMaze() {
		maze = new int[dimSquare, dimSquare];

		for (int i = 0; i < dimSquare; i++) {
			for (int j = 0; j < dimSquare; j++) {
				maze [i, j] = 1;
			}
		}

		// Choose maze starting row and column
		int r = UnityEngine.Random.Range (0, dimSquare);
		while (r % 2 == 0) {
			r = UnityEngine.Random.Range (0, dimSquare);
		}

		int c = UnityEngine.Random.Range (0, dimSquare);
		while (c % 2 == 0) {
			c = UnityEngine.Random.Range (0, dimSquare);
		}

		maze [r, c] = 0; // Set starting maze position to 0 (floor tile)
		recursion (r, c);
		return maze;
	}

	void recursion(int r, int c) {
		// Get [1,2,3,4] in randomized order
		int[] randDirs = generateRandomDirections ();


		for (int i = 0; i < randDirs.Length; i++) {

			switch (randDirs [i]) {
			case 1: // Up
				//　Whether 2 cells up is out or not
				if (r - 2 <= 0) {
					continue;
				}
				if (maze [r - 2, c] != 0) {
					maze [r - 2, c] = 0;
					maze [r - 1, c] = 0;
					recursion (r - 2, c);
				}
				break;
			case 2: // Right
				// Whether 2 cells to the right is out or not
				if (c + 2 >= width - 1) {
					continue;
				}
				if (maze [r, c + 2] != 0) {
					maze [r, c + 2] = 0;
					maze [r, c + 1] = 0;
					recursion (r, c + 2);
				}
				break;
			case 3: // Down
				// Whether 2 cells down is out or not
				if (r + 2 >= height - 1) {
					continue;
				}
				if (maze [r + 2, c] != 0) {
					maze [r + 2, c] = 0;
					maze [r + 1, c] = 0;
					recursion (r + 2, c);
				}
				break;
			case 4: // Left
				// Whether 2 cells to the left is out or not
				if (c - 2 <= 0){
					continue;
				}
				if (maze [r, c - 2] != 0) {
					maze [r, c - 2] = 0;
					maze [r, c - 1] = 0;
					recursion (r, c - 2);
				}
				break;
			}
		}
	}

	int[] generateRandomDirections() {
		List<int> randoms = new List<int> ();
		for (int i = 0; i < 4; i++) {
			randoms.Add (i + 1);
		}

		// Shuffle the randoms list
		for (int j = 0; j < 4; j++) {
			int temp = randoms [j];
			int randomIndex = UnityEngine.Random.Range (j, randoms.Count);
			randoms [j] = randoms [randomIndex];
			randoms [randomIndex] = temp;
		}

		return randoms.ToArray ();
	}
}
