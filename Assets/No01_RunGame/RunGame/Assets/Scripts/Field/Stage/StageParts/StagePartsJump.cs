using UnityEngine;
using System.Collections;

public class StagePartsJump : StageParts
{
	[SerializeField] StageEvent stageEvent;

	public override void Initialize(int xNumber, int zNumber)
	{
		base.Initialize(xNumber, zNumber);
		stageEvent.onEventSuccess = OnSuccess;
		stageEvent.onEventFailed = OnFailed;
	}

	void OnSuccess(RunCharacter character)
	{
		character.Jump();
	}

	void OnFailed(RunCharacter character)
	{
	}
}
