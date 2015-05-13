using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGame : MonoBehaviour {

	public MainGame mainGame;

	public Text scoreValue;
	public Text currencyValue;
	public Text countdownText;

	public Button[] unitButtons;
	public Button[] cancelButtons;

	public SimUnit[] units;

	void Awake()
	{
		refreshUnitButtons();
	}

	public void setScore(int score)
	{
		scoreValue.text = score.ToString();
	}

	public void setCurrency(int currency)
	{
		currencyValue.text = currency.ToString();

		refreshUnitButtons();
	}

	public void refreshUnitButtons()
	{
		for (int i = 0; i < unitButtons.Length; i++)
		{
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

		refreshUnitButtons();
	}

	public void unitPlaced()
	{
		hideCancelButtons();
		refreshUnitButtons();
	}

	private void disableUnitButtons()
	{
		for (int i = 0; i < unitButtons.Length; i++)
		{
			unitButtons[i].interactable = false;
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
