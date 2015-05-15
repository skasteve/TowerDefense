using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIUnitOptions : MonoBehaviour {

	public Text upgradeCost;
	public Text sellBackValue;
	public Text upgradeLevel;
	
	public GameObject upgradeButton;
	public GameObject sellButton;
	public GameObject cancelUnitOptions;
	public GameObject openUnitOptions;

	public UnitComponent unitComponent;

	public delegate void SellUnitAction(UnitComponent UnitComponent);
	public static event SellUnitAction onSellUnit;

	void Awake ()
	{
		upgradeButton.SetActive(false);
		sellButton.SetActive(false);
		cancelUnitOptions.SetActive(false);
		openUnitOptions.SetActive(true);
	}

	public void SetUnitOptions(int upgradeLevel, int upgradeCost, int sellBackValue)
	{
		this.upgradeLevel.text = "Lvl " + upgradeLevel.ToString();
		this.upgradeCost.text = upgradeCost.ToString();
		this.sellBackValue.text = sellBackValue.ToString();
	}

	public void showUnitOptions(bool show)
	{
		upgradeButton.SetActive(show);
		sellButton.SetActive(show);
		openUnitOptions.SetActive(!show);
		cancelUnitOptions.SetActive(show);

		playButtonSound();
	}

	public void playButtonSound()
	{
		AudioEngine.instance.PlayButtonSound();
	}

	public void sellUnit()
	{
		if (onSellUnit != null)
			onSellUnit(unitComponent);

		Destroy(gameObject);

		unitComponent.SetSimUnit(null);
	}
}
