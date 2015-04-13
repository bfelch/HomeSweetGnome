﻿using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour 
{
	public enum Fade {In, Out};
	float fadeTime = 4.0F;

	public AudioSource eerieSound;
	public AudioSource birdsSound;
	public AudioSource violinSound;
	public AudioSource windyCaveSound;
	
	void Start () 
	{
		AudioSource[] bgSounds = GetComponents<AudioSource>();
		eerieSound = bgSounds[0];
		birdsSound = bgSounds[1];
		violinSound = bgSounds[2];
		windyCaveSound = bgSounds[3];
	}
	
	public static IEnumerator FadeAudio (float timer, Fade fadeType, AudioSource aSound, float max) 
	{
		if(fadeType.ToString().Equals("In"))
		{
			aSound.Play();
		}

		float start = fadeType == Fade.In? 0.0F : max;
		float end = fadeType == Fade.In? max : 0.0F;
		float i = 0.0F;
		float step = 1.0F/timer;
		
		while (i <= 1.0F) 
		{
			i += step * Time.deltaTime;
			aSound.volume = Mathf.Lerp(start, end, i);
			yield return new WaitForSeconds(step * Time.deltaTime);
		}

		if(fadeType.ToString().Equals("Out"))
		{
			aSound.Stop();
		}
	}

	//Custom PlayClipAt method.  Plays desired clip at a desired position
	public static AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
	{
		GameObject tempSound = new GameObject("TempSound"); //Create the temp object
		tempSound.transform.position = pos; //Set its position
		AudioSource aSource = tempSound.AddComponent<AudioSource>(); //Add an audio source
		aSource.clip = clip; //Define the clip
		
		//Set other aSource properties here if desired
		aSource.Play(); //Start the sound
		aSource.minDistance = 20;
		Destroy(tempSound, clip.length); //Sestroy object after clip duration
		return aSource; //Return the AudioSource reference
	}
}
