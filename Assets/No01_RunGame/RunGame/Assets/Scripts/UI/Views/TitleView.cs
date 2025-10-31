using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleView : MonoBehaviour
{
	[SerializeField] GameObject buttonPrefab;

	Action<int> onSelect;

	public void Initialize(List<string> items, Action<int> onSelect)
	{
		const float interval = 120f;

		this.onSelect = onSelect;

		int count = items.Count;
		int half = count / 2;
		float startPos = ((count % 2) == 0) ? interval * (half - 0.5f) : interval * half;
		for (int i = 0; i < count; ++i)
		{
			int bind = i;
			var button = UIRoot.InstantiateTo<Button>(gameObject, buttonPrefab);
			button.transform.localPosition = new Vector3(0f, startPos - i * interval);
			button.onClick.AddListener(() => OnButtonClicked(bind));
			var text = button.GetComponentInChildren<Text>();
			text.text = items[i];
		}
	}

	void OnButtonClicked(int index)
	{
		if (onSelect != null) onSelect(index);
	}
}
