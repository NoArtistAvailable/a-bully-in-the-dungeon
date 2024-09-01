using System.Collections.Generic;
using elZach.Common;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private static AudioManager _instance;
	public static AudioManager Instance => _instance.OrSet(ref _instance, FindObjectOfType<AudioManager>);

	public AudioSource template;

	private static Dictionary<AudioClip, float> lastPlayed = new Dictionary<AudioClip, float>();

	public static async void PlayClip(AudioClip clip, float pitch = 1f, float volume = 1f)
	{
		if (lastPlayed.ContainsKey(clip) && Time.time - lastPlayed[clip] < 0.02f) return;
		lastPlayed[clip] = Time.time;
		var clone = Instance.template.Spawn();
		clone.gameObject.hideFlags = HideFlags.DontSave;
		clone.pitch = pitch;
		clone.volume = volume;
		clone.PlayOneShot(clip);
		await WebTask.Delay(clip.length / pitch);
		clone.Despawn();
	}
}
