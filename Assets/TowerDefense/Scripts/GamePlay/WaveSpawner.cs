using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {
	
	public Collider spawnArea;
	public SimUnitConfig[] unitPool;

	private Bounds _spawnArea;
	private int _currentWave = -1;
	private List<SimUnitConfig> _waveUnits = new List<SimUnitConfig>();
	private List<GameObject> _spawnedUnits = new List<GameObject>();

	private const int DIFFICULTY_MULTIPLIER = 20;

	void Awake()
	{
		_spawnArea = spawnArea.bounds;
		DeterminWaveUnits();
	}

	public void NextWave()
	{
		_waveUnits.Clear();
		_currentWave++;
		DeterminWaveUnits();
		SpawnWave();
	}

	private void DeterminWaveUnits()
	{
		int poolValue = _currentWave * DIFFICULTY_MULTIPLIER + DIFFICULTY_MULTIPLIER;

		List<SimUnitConfig> unitPool = PossibleUnits();

		while (poolValue > 0)
		{
			SimUnitConfig newUnit = unitPool[Random.Range(0, unitPool.Count)];

			if (newUnit.PowerValue <= poolValue)
			{
				_waveUnits.Add(newUnit);
				poolValue -= (int)newUnit.PowerValue;
			}
		}
	}

	private List<SimUnitConfig> PossibleUnits()
	{
		List<SimUnitConfig> availableUnits = new List<SimUnitConfig>();
		
		foreach (SimUnitConfig unit in unitPool)
		{
			if (unit.MinWave <= _currentWave)
			{
				int numUnitType = (int)(unit.spawnChance * 100);
				
				for (int i = 0; i < numUnitType; i++)
				{
					availableUnits.Add(unit); // Add unit to pool if it becomes available
				}
			}
		}
		
		return availableUnits;
	}

	public void SpawnWave()
	{
		_spawnedUnits.Clear();

		foreach (SimUnitConfig unit in _waveUnits)
		{
			Vector3 spawnPos = Vector3.zero;
			spawnPos.x = Random.Range(_spawnArea.min.x, _spawnArea.max.x);
			spawnPos.y = Random.Range(_spawnArea.min.y, _spawnArea.max.y);
			spawnPos.z = Random.Range(_spawnArea.min.z, _spawnArea.max.z);

			GameObject spawnedUnit = (GameObject)Instantiate(unit.UnitPrefab);
			spawnedUnit.transform.position = spawnPos;
			spawnedUnit.GetComponent<UnitComponent>().SetSimUnit(unit);
			spawnedUnit.transform.parent = gameObject.transform;
			_spawnedUnits.Add(spawnedUnit);
		}
	}
}
