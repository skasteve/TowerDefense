using UnityEngine;
using System.Collections;

public interface IRandomNumberGenerator {
	/// <summary>
	/// Get the next random number from [0,1]
	/// </summary>
	/// <returns>The float.</returns>
	float NextFloat();
}
