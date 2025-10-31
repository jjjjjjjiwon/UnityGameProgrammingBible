using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameController Instance { get; private set; }

	bool isPlaying = false;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		var items = FieldRoot.Instance.Character.SelectableNames;
		UIRoot.Instance.SetupTitle(items, StartGame);
	}

	void StartGame(int index)
	{
		FieldRoot.Instance.Select(index);
		UIRoot.Instance.SetupScore();
		startTime = DateTime.Now;
		isPlaying = true;
	}

	public void GameOver()
	{
		isPlaying = false;
	}

#region Scoring
	DateTime startTime;
	void LateUpdate()
	{
		if (!isPlaying) return;

		TimeSpan timeSpan = DateTime.Now - startTime;
		long milliSec = (long)Math.Round(timeSpan.TotalMilliseconds);
		UIRoot.Instance.ScoreUpdate(milliSec);
	}
#endregion
}
