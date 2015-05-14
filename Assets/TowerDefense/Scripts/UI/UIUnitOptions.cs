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

	void Awake ()
	{
		upgradeButton.SetActive(false);
		sellButton.SetActive(false);
		cancelUnitOptions.SetActive(false);
		openUnitOptions.SetActive(true);
	}

	public void SetUnitOptions(int upgradeLevel, int upgradeCost, int sellBackValue)
	{
		this.upgradeLevel.text = upgradeLevel.ToString();
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
}
