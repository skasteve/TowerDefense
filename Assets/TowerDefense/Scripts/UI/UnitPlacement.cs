using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the placement of player units.
/// </summary>
public class UnitPlacement : MonoBehaviour
{
	private SimUnit _selectedUnitType = null;

	private List<UnitComponent> placedUnits = new List<UnitComponent>(); 
	public static bool placingUnit = false;
	private Vector3 placingPos = Vector3.zero;
	private GameObject placementPreviewObject;

	void Update()
	{
		if (placingUnit)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("PlacementPlane")))
			{ // cast a ray onto the placement plane to determine where to place a unit
				placingPos = ray.GetPoint(raycastHit.distance);

				if (placementPreviewObject != null)
					placementPreviewObject.transform.position = placingPos;
				else
					CreatePreviewObject(placingPos);
			}

			if (Input.GetMouseButtonDown(0))
			{ 
				if (IsValidPlacement() && placingPos != Vector3.zero)
				{
					PlaceUnit(placingPos);
					_selectedUnitType = null;
				}
			}
		}
	}

	private void CreatePreviewObject(Vector3 pos)
	{
		if (_selectedUnitType == null)
			return;

		placementPreviewObject = (GameObject)Instantiate(_selectedUnitType.UnitPrefab, pos, Quaternion.identity);
		UnitComponent uc = placementPreviewObject.GetComponent<UnitComponent>();
		uc.ShowAreas(true);
		uc.SetSimUnit(_selectedUnitType);
	}

	/// <summary>
	/// Places the unit at the given position.
	/// </summary>
	/// <param name="pos">world position</param>
	private void PlaceUnit(Vector3 pos)
	{
		if (_selectedUnitType == null)
			return;

		GameObject newUnit = (GameObject)Instantiate(_selectedUnitType.UnitPrefab, pos, Quaternion.identity);
		placedUnits.Add(newUnit.GetComponent<UnitComponent>());

		GameObject.Destroy(placementPreviewObject);

		ShowPlacedUnitAreas(false);
		placingUnit = false;
	}

	private bool IsValidPlacement()
	{
		Bounds previewBounds = placementPreviewObject.GetComponent<UnitComponent>().placementArea.GetComponent<Collider>().bounds;

		foreach (UnitComponent unit in placedUnits)
		{
			Bounds unitBounds = unit.placementArea.GetComponent<Collider>().bounds;

			if (previewBounds.Intersects(unitBounds))
				return false;
		}

		return true;
	}

	public void SelectUnit(SimUnit unit)
	{
		_selectedUnitType = unit;
		placingUnit = true;

		ShowPlacedUnitAreas(true);
	}

	private void ShowPlacedUnitAreas(bool show)
	{

		if (placedUnits.Count == 0)
			return;

		foreach (UnitComponent unit in placedUnits)
		{
			unit.ShowAreas(show);
		}
	}
}
