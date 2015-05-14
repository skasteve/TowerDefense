using UnityEngine;
using System.Collections;

public class GameClock : MonoBehaviour
{
	public Transform dialHand;
	public float startRotation;
	public float endRotation;

	private float _totalTime = 0;
	private float _currentTime = 0;

	private Quaternion _handRotation = Quaternion.identity;

	public float totalTime
	{
		get
		{
			return _totalTime;
		}
		set
		{
			_totalTime = value;

			currentTime = _currentTime;
		}
	}

	public float currentTime
	{
		get
		{
			return _currentTime;
		}
		set
		{
			_currentTime = value < 0 ? 0 : value;

			_handRotation = dialHand.localRotation;

			float pctTime = _currentTime / _totalTime;

			Vector3 angles = _handRotation.eulerAngles;
			angles.z = startRotation + (endRotation - startRotation) * (1 - pctTime);

			_handRotation.eulerAngles = angles;

			dialHand.localRotation = _handRotation;
		}
	}
}
