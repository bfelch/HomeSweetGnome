using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour 
{
	public enum Fade {In, Out};
	float fadeTime = 4.0F;

	public AudioSource eerieSound;
	public AudioSource birdsSound;
	
	void Start () 
	{
		AudioSource[] bgSounds = GetComponents<AudioSource>();
		eerieSound = bgSounds[0];
		birdsSound = bgSounds[1];
	}
	
	public static IEnumerator FadeAudio (float timer, Fade fadeType, AudioSource aSound) 
	{
		float start = fadeType == Fade.In? 0.0F : 1.0F;
		float end = fadeType == Fade.In? 1.0F : 0.0F;
		float i = 0.0F;
		float step = 1.0F/timer;
		
		while (i <= 1.0F) 
		{
			i += step * Time.deltaTime;
			aSound.volume = Mathf.Lerp(start, end, i);
			yield return new WaitForSeconds(step * Time.deltaTime);
		}
	}
}
