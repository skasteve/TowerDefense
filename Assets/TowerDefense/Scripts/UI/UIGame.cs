using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGame : MonoBehaviour {

	public MainGame mainGame;

	public UIReel scoreReel;
	public UIReel currencyReel;
	public Text countdownText;
	public Text currentWave;

	public Button[] unitButtons;
	public Text[] unitCostValues;
	public Button[] cancelButtons;

	public SimUnitConfig[] units;

	void Awake()
	{
		refreshUnitButtons();
	}

	public void setScore(int score)
	{
		scoreReel.setReelValue(score);
	}

	public void setCurrency(int currency)
	{
		currencyReel.setReelValue(currency);

		refreshUnitButtons();
	}

	public void setWave(int waveNum)
	{
		currentWave.text = "Wave  " + (waveNum + 1);
	}

	public void refreshUnitButtons()
	{
		for (int i = 0; i < unitButtons.Length; i++)
		{
			unitCostValues[i].text = units[i].Cost.ToString();
			unitButtons[i].interactable = (mainGame.getCurrency() >= units[i].Cost);
		}
	}

	public void selectUnit(int unitNum)
	{
		cancelButtons[unitNum].gameObject.SetActive(true);

		mainGame.incrementCurrency(-units[unitNum].Cost);

		disableUnitButtons();
		//Debug.Log ("Unit placed: " + units[unitNum].name.ToString() + " $" + units[unitNum].Cost.ToString());
	}

	public void cancelUnit(int unitNum)
	{
		mainGame.incrementCurrency(units[unitNum].Cost); // refund currency

		for (int i = 0; i < cancelButtons.Length; i++)
		{
			cancelButtons[i].gameObject.SetActive(false);
		}
		enableUnitButtons();
		refreshUnitButtons();
	}

	public void unitPlaced()
	{
		hideCancelButtons();
		enableUnitButtons();
		refreshUnitButtons();
	}

	private void disableUnitButtons()
	{
		for (int i = 0; i < unitButtons.Length; i++)
		{
			unitButtons[i].gameObject.SetActive(false);
		}
	}

	private void enableUnitButtons()
	{
		for (int i = 0; i < unitButtons.Length; i++)
		{
			unitButtons[i].gameObject.SetActive(true);
		}
	}

	private void hideCancelButtons()
	{
		for (int i = 0; i < cancelButtons.Length; i++)
		{
			cancelButtons[i].gameObject.SetActive(false);
		}
	}
}
