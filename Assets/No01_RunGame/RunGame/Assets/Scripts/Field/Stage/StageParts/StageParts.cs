using UnityEngine;
using System.Collections;

public class StageParts : MonoBehaviour
{
	public Vector3 Position
	{
		get { return transform.position; }
		set { transform.position = value; }
	}

	public virtual void Initialize(int xNumber, int zNumber)
	{
		SetStreetNumber(xNumber, zNumber);
	}

	void SetStreetNumber(int xNumber, int zNumber)
	{
		Position = new Vector3(StageSettings.XLength * xNumber, 0f, StageSettings.ZLength * zNumber);
		UpdateName();
	}

	public void UpdateName()
	{
		var street = StageSettings.GetStreetNumber(transform.position);
		gameObject.name = string.Format("x{0:D3}_z{1:D3}", street.x, street.z);
	}
}
