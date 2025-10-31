using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	readonly Vector3 defaultCharacterOffset = new Vector3(0f, 6f, -6f);
	RunCharacter character;

	Vector3 targetPosition;

	public void Initialize(RunCharacter runCharacter)
	{
		this.character = runCharacter;
		enabled = true;
	}

	void Start()
	{
		//カメラをスタートする
		transform.position = defaultCharacterOffset;
		transform.forward = new Vector3(0f, 0.75f); // 原点少し上くらいに向けておく
	}

	void LateUpdate()
	{
		targetPosition = character.transform.TransformPoint(defaultCharacterOffset);
		ApplyTarget();
	}

	void ApplyTarget()
	{
		const float posSmooth = 6f;
		const float attOffset = 3f;

		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * posSmooth);	
		// 人間の認知的に少し予測方向(今回は前方)を見たほうが自然に見える
		transform.LookAt(character.CenterPosition + attOffset * character.transform.forward);
	}
}
