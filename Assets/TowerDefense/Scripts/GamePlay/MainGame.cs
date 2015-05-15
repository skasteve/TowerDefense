using UnityEngine;
using System.Collections;

public class MainGame : MonoBehaviour {

	public SimulationComponent simulation;
	public WaveSpawner waveSpawner;
	public UIMain mainUI;
	public GameClock gameClock;

	public float gameTime;

	private int _seed = 0;
	private int _currencyBalance = 20;
	private int _currencyEarned = 0;
	private int _score = 0;
	private int _unitsDestroyed = 0;

	private float _currentTime = 0;
	private bool _gameStarted = false;

	private const int COUNTDOWN_TIME_SEC = 5;

	private PlayerShip playerShip;

	public void StartGame()
	{
		AudioEngine.instance.PlayStart();

		mainUI.gameUI.setScore(_score);
		mainUI.gameUI.setCurrency(_currencyBalance);

		simulation.StartSim(_seed);
		StartCoroutine(StartCountDown());

		waveSpawner.onWaveComplete += OnWaveComplete;
		
		UnitComponent.onSimExplode += HandleonSimExplode;

		gameClock.totalTime = gameTime;

		_currentTime = gameTime;
		_gameStarted = true;
	}

	public void EndGame()
	{
		mainUI.loadEndScreen();
		mainUI.endUI.setFinalScore(_score, _currencyEarned, _unitsDestroyed, waveSpawner.GetWaveNum());
	}

	void Update()
	{
		if (_currentTime <= 0)
		{
			if (_gameStarted)
				EndGame();
			return;
		}

		_currentTime -= Time.deltaTime;

		gameClock.currentTime = _currentTime;
	}

	private IEnumerator StartCountDown()
	{
		AudioEngine.instance.PlayIncomingWave();

		int startTime = COUNTDOWN_TIME_SEC;

		mainUI.gameUI.countdownText.gameObject.SetActive(true);

		while (startTime > -1)
		{
			mainUI.gameUI.countdownText.text = startTime.ToString();

			if (startTime == 0)
				mainUI.gameUI.countdownText.text = "Go!";

			startTime--;

			yield return new WaitForSeconds(1);
		}

		mainUI.gameUI.countdownText.text = "";
		mainUI.gameUI.countdownText.gameObject.SetActive(false);

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
		mainUI.gameUI.setWave(waveSpawner.GetWaveNum());
	}

	void OnWaveComplete()
	{
		BroadcastToPlayerShip("OnWaveComplete");
		StartCoroutine(StartCountDown());
	}

	public void incrementCurrency(int currency)
	{
		_currencyBalance += currency;
		
		mainUI.gameUI.setCurrency(_currencyBalance);
	}

	public int getCurrency()
	{
		return _currencyBalance;
	}

	public void incrementScore(int score)
	{
		_score += score;

		mainUI.gameUI.setScore(_score);
	}
	
	private void HandleonSimExplode (SimUnitConfig simUnit)
	{
		incrementCurrency(simUnit.Cost);
		_currencyEarned += simUnit.Cost;
		incrementScore(simUnit.Points);
		_unitsDestroyed++;
		
	}
}
