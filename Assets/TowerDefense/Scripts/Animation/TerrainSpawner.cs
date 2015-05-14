using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainSpawner : MonoBehaviour {

	public GameObject[] terrainObjects;
	public Collider spawnArea;

	private List<GameObject> spawnedTerrain = new List<GameObject>();
	private const int TERRAIN_COUNT = 4;
	private const float TERRAIN_OFFSET_Z = 40;

	void Start()
	{
		while (spawnedTerrain.Count < TERRAIN_COUNT) 
		{
			SpawnTerrain();
		}

		InvokeRepeating("CheckTerrainPos", 0, 0.5f);
	}

	void SpawnTerrain()
	{
		GameObject terrainToSpawn = terrainObjects [Random.Range (0, terrainObjects.Length)];
		GameObject newTerrain = (GameObject)Instantiate (terrainToSpawn);
		newTerrain.transform.parent = this.transform;
		if (spawnedTerrain.Count > 0) {
			Vector3 lastTerrainPos = spawnedTerrain [spawnedTerrain.Count - 1].transform.position;
			newTerrain.transform.position = new Vector3 (lastTerrainPos.x, lastTerrainPos.y, lastTerrainPos.z + TERRAIN_OFFSET_Z);
		}
		spawnedTerrain.Add (newTerrain);
	}

	private void CheckTerrainPos()
	{
		if (spawnedTerrain [0].transform.position.z < -TERRAIN_OFFSET_Z) 
		{
			GameObject removeTerrain = spawnedTerrain[0];
			spawnedTerrain.RemoveAt(0);
			GameObject.Destroy(removeTerrain);

			SpawnTerrain();
		}
	}
}
