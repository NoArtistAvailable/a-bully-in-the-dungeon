#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class EightWayProcessor : InputProcessor<Vector2>
{
#if UNITY_EDITOR
	static EightWayProcessor()
	{
		Initialize();
	}
#endif

	public float threshold = 0.2f;

	[RuntimeInitializeOnLoadMethod]
	static void Initialize()
	{
		InputSystem.RegisterProcessor<EightWayProcessor>();
	}
	public override Vector2 Process(Vector2 value, InputControl control)
	{
		var magnitude = value.magnitude;
		if (magnitude < threshold) return value;
		value.x = value.x > threshold ? 1f : value.x < -threshold ? -1f : 0f;
		value.y = value.y > threshold ? 1f : value.y < -threshold ? -1f : 0f;
		value *= magnitude;
		return value;
	}
}