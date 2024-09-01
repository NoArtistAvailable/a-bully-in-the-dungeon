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

	[RuntimeInitializeOnLoadMethod]
	static void Initialize()
	{
		InputSystem.RegisterProcessor<EightWayProcessor>();
	}
	public override Vector2 Process(Vector2 value, InputControl control)
	{
		var magnitude = value.magnitude;
		value.x = value.x > 0.05f ? 1f : value.x < -0.05f ? -1f : 0f;
		value.y = value.y > 0.05f ? 1f : value.y < -0.05f ? -1f : 0f;
		value *= magnitude;
		return value;
	}
}