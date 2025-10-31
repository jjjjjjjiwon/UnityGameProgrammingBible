using System.Collections;
using UnityEngine;

public class StageSettings
{
	public const int tileX = 1;
	public const int tileZ = 3;
	public const float XLength = 20f;
	public const float ZLength = 20f;

	public static StreetNumber GetStreetNumber(Vector3 position)
	{
		float halfX = XLength * 0.5f;
		float halfZ = ZLength * 0.5f;
		int numX = (int)Mathf.FloorToInt((position.x + halfX) / XLength);
		int numZ = (int)Mathf.FloorToInt((position.z + halfZ) / ZLength);
		return new StreetNumber(numX, numZ);
	}
}
