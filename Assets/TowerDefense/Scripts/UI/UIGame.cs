using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGame : MonoBehaviour {

	public Text scoreValue;
	public Text currencyValue;
	public Text countdownText;

	public Button[] unitButtons;
	public Button[] cancelButtons;

	public SimUnit[] units;
	
	private int _currencyBalance = 0;	

	void Awake()
	{
		refreshUnitButtons();
	}

	public void setScore(int score)
	{
		scoreValue.text = score.ToString();
	}

	public void incrementCurrency(int currency)
	{
		_currencyBalance += currency;

		currencyValue.text = _currencyBalance.ToString();

		refreshUnitButtons();
	}

	public void refreshUnitButtons()
	{
		for (int i = 0; i < unitButtons.Length; i++)
		{
			unitButtons[i].interactable = (_currencyBalance >= units[i].Cost);
		}
	}

	public void selectUnit(int unitNum)
	{
		cancelButtons[unitNum].gameObject.SetActive(true);

		incrementCurrency(-units[unitNum].Cost);

		disableUnitButtons();
		//Debug.Log ("Unit placed: " + units[unitNum].name.ToString() + " $" + units[unitNum].Cost.ToString());
	}

	public void cancelUnit(int unitNum)
	{
		incrementCurrency(units[unitNum].Cost); // refund currency

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
