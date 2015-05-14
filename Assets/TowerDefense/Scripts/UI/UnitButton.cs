using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour {

	public GameObject unitOptionsObject;

	private GameObject _optionsHolder;
	private GameObject _unitOptions;

	void Awake()
	{
		_optionsHolder = GameObject.Find("UnitOptions");
	}

	void OnEnable()
	{
		_unitOptions = (GameObject)Instantiate(unitOptionsObject);
		_unitOptions.transform.SetParent(_optionsHolder.transform);
	}

	void Update()
	{
		_unitOptions.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
	}
}
