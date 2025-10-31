using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageEvent : MonoBehaviour
{
	public enum Status { None, Entered, Success, Failed }

	[SerializeField] int needTapCount = 0;

	Status eventStatus;
	int remainTapCount;
	RunCharacter character = null;

	bool WillSuccess { get { return (EventStatus == Status.Entered && RemainCount <= 0); } }
	public bool IsFailed { get { return (EventStatus == Status.Failed); } }
	public int NeedTapCount { get { return needTapCount; } }

	public Action<RunCharacter, int> onEventTap = null;
	public Action<RunCharacter> onEventSuccess = null;
	public Action<RunCharacter> onEventFailed = null;

	Status EventStatus
	{
		get { return eventStatus; }
		set
		{
			eventStatus = value;
			EventUpdate();
		}
	}

	int RemainCount
	{
		get { return remainTapCount; }
		set
		{
			remainTapCount = value;
			EventUpdate();
		}
	}

	void Awake()
	{
		eventStatus = Status.None;
		remainTapCount = needTapCount;
		if (GetComponent<BoxCollider>() == null) remainTapCount = 0;
	}

	void OnTriggerEnter(Collider other)
	{
		// Unityの挙動？コライダの判定がShift前の位置関係でEnterすることがあるので距離で弾いておく
		if (Vector3.Distance(other.transform.position, transform.position) >= StageSettings.ZLength) return;

		EventStatus = Status.Entered;
		character = other.GetComponent<RunCharacter>();
		if (character != null) character.OnEventEnter(this);
	}

	void Update()
	{
		if (character == null) return;
		if (EventStatus != Status.Entered) return;
		if (WillSuccess) return;

		if (character.IsAvairableInput && InputController.GetClick())
		{
			--RemainCount;
			if (onEventTap != null) onEventTap(character, RemainCount);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other == null) return;
		if (character == null) return;
		if (EventStatus != Status.Entered) return;

		if (WillSuccess)
		{
			EventStatus = Status.Success;
			StartCoroutine(WaitForSuccess());
			if (onEventSuccess != null) onEventSuccess(character);
			character.OnEventExit();
		}
		else
		{
			EventStatus = Status.Failed;
			GameController.Instance.GameOver();
			if (onEventFailed != null) onEventFailed(character);
		}

		character = null;
	}

	IEnumerator WaitForSuccess()
	{
		yield return new WaitForSeconds(1f);
		EventStatus = Status.None; // 次のパーツの表示と重ならないように戻しておく
	}

	void EventUpdate()
	{
		UIRoot.Instance.EventUpdate(EventStatus, RemainCount);
	}
}
