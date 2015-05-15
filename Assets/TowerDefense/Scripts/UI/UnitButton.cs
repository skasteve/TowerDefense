using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour {

	public GameObject unitOptionsObject;

	private GameObject _optionsHolder;
	private GameObject _unitOptions;

	private UnitComponent _unitComponent;

	void Awake()
	{
		_optionsHolder = GameObject.Find("UnitOptions");
		_unitComponent = gameObject.GetComponent<UnitComponent>();
	}

	void OnEnable()
	{
		_unitOptions = (GameObject)Instantiate(unitOptionsObject);
		_unitOptions.transform.SetParent(_optionsHolder.transform);

		UIUnitOptions options = _unitOptions.GetComponent<UIUnitOptions>();
		options.SetUnitOptions(2, _unitComponent.simunit.Cost * 2, _unitComponent.simunit.Cost / 2);

		options.unitComponent = _unitComponent;
	}

	void Update()
	{
		_unitOptions.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
	}
}
