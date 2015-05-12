using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {

	public SimUnit[] unitPool;
	private int _currentWave = 0;

	private const int DIFFICULTY_MULTIPLIER = 20;

	void Awake()
	{
		determinWaveUnits();
	}

	public void incrementWave()
	{
		_currentWave++;
		determinWaveUnits();
	}

	private void determinWaveUnits()
	{
		int poolValue = _currentWave * DIFFICULTY_MULTIPLIER + DIFFICULTY_MULTIPLIER;

		List<SimUnit> unitPool = possibleUnits();
		List<SimUnit> waveUnits = new List<SimUnit>();

		while (poolValue > 0)
		{
			SimUnit newUnit = unitPool[Random.Range(0, unitPool.Count)];

			if (newUnit.PowerValue <= poolValue)
			{
				waveUnits.Add(newUnit);
				poolValue -= (int)newUnit.PowerValue;
			}
		}

		foreach(SimUnit unit in waveUnits)
		{
			Debug.Log("Unit: " + unit.name.ToString() + " PowerValue: " + unit.PowerValue.ToString());
		}
	}

	private List<SimUnit> possibleUnits()
	{
		List<SimUnit> availableUnits = new List<SimUnit>();
		
		foreach (SimUnit unit in unitPool)
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
}
