using UnityEngine;
using System.Collections;

public class UIEndscreen : MonoBehaviour {

	public UIReel scoreReel;
	public UIReel currencyReel;
	public UIReel unitsDestroyedReel;
	public UIReel waveReel;

	public void setFinalScore(int score, int currency, int numUnits, int wave)
	{
		scoreReel.setReelValue(score);
		currencyReel.setReelValue(currency);
		unitsDestroyedReel.setReelValue(numUnits);
		waveReel.setReelValue(numUnits);
	}
}
