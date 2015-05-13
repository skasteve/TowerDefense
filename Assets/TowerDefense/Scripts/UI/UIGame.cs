using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGame : MonoBehaviour {

	public Text scoreValue;
	public Text currencyValue;
	public Text countdownText;

	public Button[] unitButtons;

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

	public void placeUnit(int unitNum)
	{
		incrementCurrency(-units[unitNum].Cost);
		Debug.Log ("Unit placed: " + units[unitNum].name.ToString() + " $" + units[unitNum].Cost.ToString());
	}
}
