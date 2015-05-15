using UnityEngine;
using System.Collections;

public class MainGame : MonoBehaviour {

	public SimulationComponent simulation;
	public WaveSpawner waveSpawner;
	public UIGame gameUI;
	public GameClock gameClock;

	public float gameTime;

	private int _seed = 0;
	private int _currencyBalance = 20;
	private int _score = 0;

	private float _currentTime = 0;

	private const int COUNTDOWN_TIME_SEC = 5;

	private PlayerShip playerShip;

	public void StartGame()
	{
		AudioEngine.instance.PlayStart();

		gameUI.setScore(_score);
		gameUI.setCurrency(_currencyBalance);

		simulation.StartSim(_seed);
		StartCoroutine(StartCountDown());

		waveSpawner.onWaveComplete += OnWaveComplete;
		
		UnitComponent.onSimExplode += HandleonSimExplode;

		gameClock.totalTime = gameTime;

		_currentTime = gameTime;
	}

	void Update()
	{
		if (_currentTime <= 0)
			return;

		_currentTime -= Time.deltaTime;

		gameClock.currentTime = _currentTime;
	}

	private IEnumerator StartCountDown()
	{
		AudioEngine.instance.PlayIncomingWave();

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

	void BroadcastToPlayerShip(string message) {
		if(playerShip==null) {
			playerShip = FindObjectOfType<PlayerShip>();
		}
		playerShip.BroadcastMessage(message);
	}

	public void StartWave()
	{
		BroadcastToPlayerShip("StartWave");
		waveSpawner.NextWave();
		gameUI.setWave(waveSpawner.GetWaveNum());
	}

	void OnWaveComplete()
	{
		BroadcastToPlayerShip("OnWaveComplete");
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
