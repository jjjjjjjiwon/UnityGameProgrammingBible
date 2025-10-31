using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
	[SerializeField] List<GameObject> stagePrefabs;

	RunCharacter runCharacter;
	StreetNumber current;
	Dictionary<StreetNumber, StageParts> stageParts = new Dictionary<StreetNumber, StageParts>();

	public void Initialize(RunCharacter runCharacter)
	{
		this.runCharacter = runCharacter;
		Construct();
	}

	void Construct()
	{
		Clear();
		current = StageSettings.GetStreetNumber(runCharacter.Position);

		int halfX = StageSettings.tileX / 2;
		int halfZ = StageSettings.tileZ / 2;
		for (int i = -halfX; i <= halfX; ++i)
		{
			for (int j = -halfZ; j <= halfZ; ++j)
			{
				bool isCenter = (i == 0 && j == 0);
				var s = current;
				s.x = current.x + i;
				s.z = current.z + j;
				var prefab = (isCenter) ? stagePrefabs[0] : SelectStagePartsPrefab();
				var g = GenerateStageParts(prefab, s.x, s.z);
				if (isCenter) g.gameObject.name += "_c"; // 中心が分かるように目印付けとく
				stageParts.Add(s, g);
			}
		}
	}

	GameObject SelectStagePartsPrefab()
	{
		int index = UnityEngine.Random.Range(0, stagePrefabs.Count);
		return stagePrefabs[index];
	}

	StageParts GenerateStageParts(GameObject prefab, int xNumber, int zNumber)
	{
		var parts = FieldRoot.InstantiateTo<StageParts>(gameObject, prefab);
		parts.Initialize(xNumber, zNumber);
		return parts;
	}

	void Clear()
	{
		foreach (var g in stageParts.Values) Destroy(g.gameObject);
		stageParts.Clear();
	}

	void LateUpdate()
	{
		if (runCharacter == null) return;

		var street = StageSettings.GetStreetNumber(runCharacter.Position);
		if (street.x == current.x && street.z == current.z) return;

		Vector3 oldBase = stageParts[current].Position;
		if (Mathf.Abs(street.x - current.x) <= 1 && Mathf.Abs(street.z - current.z) <= 1)
		{
			stageParts[current].UpdateName(); // 名前をリセットしとく

			UpdatePartsLine(street.x, current.x, StageSettings.tileX,
				(succ, index) => new StreetNumber(current.x - succ, current.z + index),
				(succ, index) => new StreetNumber(street.x + succ, current.z + index));
			current.x = street.x;

			UpdatePartsLine(street.z, current.z, StageSettings.tileZ,
				(succ, index) => new StreetNumber(current.x + index, current.z - succ),
				(succ, index) => new StreetNumber(current.x + index, street.z + succ));
			current.z = street.z;

			stageParts[current].gameObject.name += "_c"; // 中心が分かるように目印付けとく
		}
		else
		{
			// ワープしたときなどの隣接部分の移動で対応できない場合
			// まるっと作り直す
			Construct();
		}

		FieldRoot.Instance.ChangeBase(oldBase, stageParts[current].Position);
	}

	void UpdatePartsLine(int standNumber, int currentNumber, int tile,
			Func<int, int, StreetNumber> oldGetter, Func<int, int, StreetNumber> newGetter)
	{
		int half = tile / 2;
		var movePairs = new Dictionary<StreetNumber, StreetNumber>();
		int succ = 0;
		if (standNumber > currentNumber) succ = half;
		else if (standNumber < currentNumber) succ = -half;

		if (succ != 0)
		{
			// 離れた一列があれば移動させる
			for (int i = -half; i <= half; ++i)
			{
				var o = oldGetter(succ, i);
				if (!stageParts.ContainsKey(o)) continue;
				if (movePairs.ContainsKey(o)) continue;
				var n = newGetter(succ, i);
				movePairs.Add(o, n);
			}
			Move(movePairs);
		}
	}

	void Move(Dictionary<StreetNumber, StreetNumber> movePairs)
	{
		foreach (var kv in movePairs)
		{
			var old = kv.Key;
			var n = kv.Value;

			// 削除して作り直す
			var oldParts = stageParts[old];
			stageParts.Remove(old);
			Destroy(oldParts.gameObject);
			stageParts.Add(n, GenerateStageParts(SelectStagePartsPrefab(), n.x, n.z));
		}
	}

	public void Shift(Vector3 offset)
	{
		var parts = new List<StageParts>(stageParts.Values); // copy
		stageParts.Clear();
		foreach (var p in parts)
		{
			p.Position += offset;
			p.UpdateName();
			stageParts.Add(StageSettings.GetStreetNumber(p.Position), p);
		}
		current = StageSettings.GetStreetNumber(runCharacter.Position);
		stageParts[current].gameObject.name += "_c"; // 中心が分かるように目印付けとく
	}
}
