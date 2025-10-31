using UnityEngine;
using System.Collections;

public class FieldRoot : MonoBehaviour
{
	public static FieldRoot Instance { get; private set; }

	[SerializeField] GameObject characterRoot;
	[SerializeField] GameObject characterManagerPrefab;
	[SerializeField] GameObject stageRoot;
	[SerializeField] GameObject stageManagerPrefab;
	[SerializeField] CameraController fieldCamera;

	public StageManager Stage { get; private set; }
	public CharacterManager Character { get; private set; }
	public CameraController Camera { get { return fieldCamera; } }

	public static T InstantiateTo<T>(GameObject parent, GameObject go)
		where T : Component
	{
		GameObject obj = (GameObject)GameObject.Instantiate(go);
		obj.transform.parent = parent.transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localEulerAngles = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		return obj.GetComponent<T>();
	}

	void Awake()
	{
		Instance = this;
		Character = InstantiateTo<CharacterManager>(characterRoot, characterManagerPrefab);
		Stage = InstantiateTo<StageManager>(stageRoot, stageManagerPrefab);
	}

	public void Select(int index)
	{
		Character.Select(index, characterRoot);
		Stage.Initialize(Character.RunCharacter);
		fieldCamera.Initialize(Character.RunCharacter);
	}

	public void ChangeBase(Vector3 oldBase, Vector3 newBase)
	{
		Vector3 offset = oldBase - newBase;
		Character.Shift(offset);
		Stage.Shift(offset);
		Camera.transform.position += offset;
	}
}
