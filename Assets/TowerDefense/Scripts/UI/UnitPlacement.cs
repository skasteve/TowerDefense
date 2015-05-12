using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the placement of player units.
/// </summary>
public class UnitPlacement : MonoBehaviour
{
	public Transform placementPos;

	private Plane _placementPlane;
	private SimUnit _selectedUnitType = null;

	void Awake()
	{
		_placementPlane = new Plane(Vector3.up, placementPos.position);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{ // cast a ray onto the placement plane to determine where to place a unit
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			float dist;
			if (_placementPlane.Raycast(ray, out dist))
				PlaceUnit(ray.GetPoint(dist));
		}
	}

	/// <summary>
	/// Places the unit at the given position.
	/// </summary>
	/// <param name="pos">world position</param>
	private void PlaceUnit(Vector3 pos)
	{
		if (_selectedUnitType != null)
			Instantiate(_selectedUnitType.UnitPrefab, pos, Quaternion.identity);
	}

	public void SelectUnit(SimUnit unit)
	{
		_selectedUnitType = unit;
	}
}
