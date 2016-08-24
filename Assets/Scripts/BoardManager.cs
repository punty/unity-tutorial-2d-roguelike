using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count {
		public int minimun;
		public int maximun;

		public Count (int min, int max) {
			minimun = min;
			maximun = max;
		}
	}

	public int columns = 8;
	public int rows = 8;

	public Count wallCount = new Count (5, 9);
	public Count foodCount = new Count (1, 5);

	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outherWallTiles;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3> ();


	void InitialiseList() {
		gridPositions.Clear ();
		for (int x = 1; x < columns; x++) {
			for (int y = 1; y < rows; y++) {
				gridPositions.Add (new Vector3 (x, y, 0.0f));
			}
		}
	}


	void BoardSetup () {
		boardHolder = new GameObject ("Board").transform;
		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows+1; y++) {
				GameObject toInstatiate = null;
				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstatiate = outherWallTiles [Random.Range (0, outherWallTiles.Length)];
				} else {
					toInstatiate = floorTiles [Random.Range (0, floorTiles.Length)];
				}
				GameObject instance = Instantiate (toInstatiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	Vector3 RandomPosition () {
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimun, int maximun) {
		int objectCount = Random.Range (minimun, maximun + 1);
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}


	public void SetupScene(int level) {
		BoardSetup ();
		InitialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimun, wallCount.maximun);
		LayoutObjectAtRandom (foodTiles, foodCount.minimun, foodCount.maximun);
		int enemyCount = (int)Mathf.Log (level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}
}
