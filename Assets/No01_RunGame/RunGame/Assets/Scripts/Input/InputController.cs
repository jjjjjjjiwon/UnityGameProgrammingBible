using UnityEngine;

public class InputController : MonoBehaviour
{
	static InputController instance;
	bool down = false;
	bool prev = false;

	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		prev = down;

#if UNITY_EDITOR
		if (down)
		{
			if (Input.GetMouseButtonUp(0)) down = false;
		}
		else
		{
			if (Input.GetMouseButtonDown(0)) down = true;
		}
#else
		if (down)
		{
			if (Input.touchCount <= 0) down = false;
		}
		else
		{
			if (Input.touchCount > 0) down = true;
		}
#endif
	}

	public static bool GetClick()
	{
		return (instance.down != instance.prev) && instance.down;
	}
}
