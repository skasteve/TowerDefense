using UnityEngine;
using System.Collections;

public interface ISimObjectEventHandler {
	void OnSimDestroy(SimUnitInstance sender);	
}
