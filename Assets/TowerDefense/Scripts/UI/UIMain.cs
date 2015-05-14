using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIMain : MonoBehaviour {

	public GameObject startScreen;
	public GameObject gameScreen;
	public GameObject endScreen;

	private GameObject _currentScreen;

	void Awake()
	{
		_currentScreen = startScreen;
		loadStartScreen();
		AudioEngine.instance.PlayAmbientAudio();
	}

	public void unloadCurrentScreen()
	{
		_currentScreen.SetActive(false);
	}

	public void loadStartScreen()
	{
		unloadCurrentScreen();

		startScreen.SetActive(true);
		_currentScreen = startScreen;
	}

	public void loadGameScreen()
	{
		unloadCurrentScreen();

		gameScreen.SetActive(true);
		_currentScreen = gameScreen;
	}

	public void loadEndScreen()
	{
		unloadCurrentScreen();

		endScreen.SetActive(true);
		_currentScreen = endScreen;
	}
}
