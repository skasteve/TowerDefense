using UnityEngine;
using System.Collections;

/// <summary>
/// Representation of the player ship object.
/// Controls player movement.
/// </summary>
public class PlayerShip : MonoBehaviour
{
	public float FireRate = 2.0f;
	private float _nextfiretime = 0.0f;

	public float speed;

	private Vector3 _screenPos = Vector3.zero;
	private Vector3 _targetPos = Vector3.zero;

	void Update()
	{
		if(ShouldControlShip()) {
			ControlShip ();
		}

		if(ShouldFireProjectiles()) {
			FireProjectiles();
		}
	}

	bool ShouldControlShip() {
		if (!Input.GetMouseButton(0) || UnitPlacement.placingUnit)
			return false; // ignore when mouse is not down or currently placing units
		
		if (Input.mousePosition.y > Screen.height * 0.25f || Input.mousePosition.y < Screen.height * 0.08f)
			return false; // ignore input above the bottom section of the screen

		return true;
	}

	void ControlShip() {
		// get the screen position that we are going to project
		_screenPos = Input.mousePosition;
		_screenPos.x = Mathf.Clamp(_screenPos.x, 0, Screen.width); // clamp x to fit screen
		_screenPos.z = transform.position.y; // depth relative to the ship position
		
		// constrain movement to the x axis
		_targetPos = transform.position;
		_targetPos.x = Camera.main.ScreenToWorldPoint(_screenPos).x;
		
		// interpolate
		transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * speed);
	}

	bool ShouldFireProjectiles() {
		if(Time.time >= _nextfiretime && FireRate > 0.0f) {
			_nextfiretime = Time.time + 1.0f/FireRate;
			return true;
		}
		return false;
	}

	void FireProjectiles() {
		SimProjectileSpawner[] spawners = GetComponentsInChildren<SimProjectileSpawner>();
		foreach(SimProjectileSpawner s in spawners) {
			s.FireProjectile();
		}
	}
}
