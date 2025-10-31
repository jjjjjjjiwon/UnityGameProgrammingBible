using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
	[SerializeField] Text scoreText;
	[SerializeField] Text eventText;

	public void Initialize()
	{
		eventText.gameObject.SetActive(false);
		ScoreUpdate(0);
	}

	public void ScoreUpdate(long score)
	{
		string format = "Score: {0:D6}";
		scoreText.text = string.Format(format, score);
	}

	public void EventUpdate(StageEvent.Status status, int remainCount)
	{
		if ((status == StageEvent.Status.Success) || (status == StageEvent.Status.Entered && remainCount <= 0))
		{
			eventText.text = "OK!";
			eventText.color = Color.yellow;
			eventText.gameObject.SetActive(true);
		}
		else if (status == StageEvent.Status.Entered)
		{
			eventText.text = "Tap!!";
			eventText.color = new Color(1f, 0.5f, 0f); // orange
			eventText.gameObject.SetActive(true);
		}
		else if (status == StageEvent.Status.Failed)
		{
			eventText.text = "Failed...";
			eventText.color = Color.red;
			eventText.gameObject.SetActive(true);
		}
		else
		{
			eventText.gameObject.SetActive(false);
		}
	}
}
