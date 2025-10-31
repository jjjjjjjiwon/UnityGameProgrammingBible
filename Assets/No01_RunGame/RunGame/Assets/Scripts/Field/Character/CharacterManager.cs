using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
	[SerializeField] GameObject[] prefabs;

	public List<string> SelectableNames
	{
		get
		{
			var names = new List<string>();
			foreach (var p in prefabs) names.Add(p.name);
			return names;
		}
	}
	public RunCharacter RunCharacter { get; private set; }

	public void Select(int index, GameObject parent)
	{
		RunCharacter = FieldRoot.InstantiateTo<RunCharacter>(parent, prefabs[index]);
	}

	public void Shift(Vector3 offset)
	{
		RunCharacter.transform.position += offset;
	}
}
