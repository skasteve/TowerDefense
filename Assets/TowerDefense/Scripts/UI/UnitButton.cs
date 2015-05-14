using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour {

	public GameObject unitOptionsObject;

	private Canvas _canvas;
	private GameObject _unitOptions;

	void Awake()
	{
		_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
	}

	void OnEnable()
	{
		_unitOptions = (GameObject)Instantiate(unitOptionsObject);
		_unitOptions.transform.SetParent(_canvas.transform);
	}

	void Update()
	{
		_unitOptions.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
	}
}
