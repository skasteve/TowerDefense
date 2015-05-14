using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIReel : MonoBehaviour {

	public Image[] reelImages;
	public Text[] reelValues;

	private Color valueColor = Color.black;
	private Color noValueColor = Color.white;

	public void setReelValue(int value)
	{
		if (value == 0)
		{
			setZeroValue();
			return;
		}

		string reelText = value.ToString();
		Char[] reelCharacters = reelText.ToCharArray();
		Array.Reverse(reelCharacters);

		for (int i = 0; i < reelValues.Length; i++)
		{
			if (i < reelCharacters.Length)
			{
				reelValues[i].text = reelCharacters[i].ToString();
				reelValues[i].color = valueColor;
				reelImages[i].enabled = true;
			}
			else
			{
				reelValues[i].text = "0";
				reelValues[i].color = noValueColor;
				reelImages[i].enabled = false;
			}
		}
	}

	private void setZeroValue()
	{
		for (int i = 0; i < reelValues.Length; i++)
		{
			reelValues[i].text = "0";
			reelValues[i].color = noValueColor;
			reelImages[i].enabled = false;
		}
	}
}
