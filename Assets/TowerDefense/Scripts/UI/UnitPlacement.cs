using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the placement of player units.
/// </summary>
public class UnitPlacement : MonoBehaviour
{
	private SimUnit _selectedUnitType = null;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{ // cast a ray onto the placement plane to determine where to place a unit
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("PlacementPlane")))
				PlaceUnit(ray.GetPoint(raycastHit.distance));
		}
	}

	/// <summary>
	/// Places the unit at the given position.
	/// </summary>
	/// <param name="pos">world position</param>
	private void PlaceUnit(Vector3 pos)
	{
		if (_selectedUnitType == null)
			return;

		Instantiate(_selectedUnitType.UnitPrefab, pos, Quaternion.identity);
	}

	public void SelectUnit(SimUnit unit)
	{
		_selectedUnitType = unit;
	}
}
