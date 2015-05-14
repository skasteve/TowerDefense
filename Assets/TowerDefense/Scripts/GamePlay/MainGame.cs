using UnityEngine;
using System.Collections;

public class MainGame : MonoBehaviour {

	public SimulationComponent simulation;
	public WaveSpawner waveSpawner;
	public UIGame gameUI;

	private int _seed = 0;
	private int _currencyBalance = 20;
	private int _score = 0;

	private const int COUNTDOWN_TIME_SEC = 5;

	public void StartGame()
	{
		AudioEngine.instance.PlayStart();

		gameUI.setScore(_score);
		gameUI.setCurrency(_currencyBalance);

		simulation.StartSim(_seed);
		StartCoroutine(StartCountDown());

		waveSpawner.onWaveComplete += OnWaveComplete;
		
		UnitComponent.onSimExplode += HandleonSimExplode;
	}

	private IEnumerator StartCountDown()
	{
		int startTime = COUNTDOWN_TIME_SEC;

		gameUI.countdownText.gameObject.SetActive(true);

		while (startTime > -1)
		{
			gameUI.countdownText.text = startTime.ToString();

			if (startTime == 0)
				gameUI.countdownText.text = "Go!";

			startTime--;

			yield return new WaitForSeconds(1);
		}

		gameUI.countdownText.text = "";
		gameUI.countdownText.gameObject.SetActive(false);

		StartWave();
	}

	private void StartWave()
	{
		AudioEngine.instance.PlayIncomingWave();
		waveSpawner.NextWave();
		gameUI.setWave(waveSpawner.GetWaveNum());
	}

	void OnWaveComplete()
	{
		StartCoroutine(StartCountDown());
	}

	public void incrementCurrency(int currency)
	{
		_currencyBalance += currency;
		
		gameUI.setCurrency(_currencyBalance);
	}

	public int getCurrency()
	{
		return _currencyBalance;
	}

	public void incrementScore(int score)
	{
		_score += score;

		gameUI.setScore(_score);
	}
	
	private void HandleonSimExplode (SimUnitConfig simUnit)
	{
		incrementCurrency(simUnit.Cost);
		incrementScore(simUnit.Points);
	}
}
