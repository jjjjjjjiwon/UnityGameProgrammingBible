using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRoot : MonoBehaviour
{
	public static UIRoot Instance { get; private set; }

	[SerializeField] TitleView titleView;
	[SerializeField] ScoreView scoreView;

	public static T InstantiateTo<T>(GameObject parent, GameObject go)
		where T : UIBehaviour
	{
		GameObject obj = (GameObject)GameObject.Instantiate(go);
		obj.transform.SetParent(parent.transform);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localEulerAngles = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		return obj.GetComponent<T>();
	}

	void Awake()
	{
		Instance = this;
		titleView.gameObject.SetActive(false);
		scoreView.gameObject.SetActive(false);
	}

	public void SetupTitle(List<string> items, Action<int> onSelect)
	{
		titleView.gameObject.SetActive(true);
		scoreView.gameObject.SetActive(false);
		titleView.Initialize(items, onSelect);
	}

	public void SetupScore()
	{
		titleView.gameObject.SetActive(false);
		scoreView.gameObject.SetActive(true);
		scoreView.Initialize();
	}

	public void ScoreUpdate(long score)
	{
		scoreView.ScoreUpdate(score);
	}

	public void EventUpdate(StageEvent.Status status, int remainCount)
	{
		scoreView.EventUpdate(status, remainCount);
	}
}
