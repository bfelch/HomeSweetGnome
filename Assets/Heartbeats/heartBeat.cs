using UnityEngine;
using System.Collections;

public class heartBeat : MonoBehaviour 
{
	private GUIDamage guiDamageScript; //GUIDamage Script
	private float contactCounter; //How fast is the heartbeat?
	private float waitGap; //How long to wait between each heart beat
	private bool enableBeat = false; //Play the heartbeat?
	private bool beating = false; //Play one beat at a time
	private bool deathSleep = false;
	private bool deathFall = false;
	private AudioSource beat1; //Slowest heartbeat
	private AudioSource beat2; 
	private AudioSource beat3;
	private AudioSource beat4;
	private AudioSource beat5;
	private AudioSource beat6;
	private AudioSource beat7;
	private AudioSource beat8; //Fastest hearbeat

	// Use this for initialization
	void Start() 
	{
		guiDamageScript = GameObject.Find ("Player").GetComponent<GUIDamage> ();

		AudioSource[] aSources = GetComponents<AudioSource>(); //Grab all the audio sources on this object
		beat1 = aSources[0];
		beat2 = aSources[1];
		beat3 = aSources[2];
		beat4 = aSources[3];
		beat5 = aSources[4];
		beat6 = aSources[5];
		beat7 = aSources[6];
		beat8 = aSources[7];
	}
	
	// Update is called once per frame
	void Update () 
	{
		//get the death variables from player
		deathFall = GameObject.Find("Player").GetComponent<EndGames>().playerFell;
		deathSleep = GameObject.Find("Player").GetComponent<EndGames>().playerSlept;

		if(!deathFall && !deathSleep)
		{
			//Update variables from other script
			contactCounter = guiDamageScript.damageTimer;

			if(contactCounter >= 20.0F)
			{
				enableBeat = false;
			}
			else
			{
				enableBeat = true;
			}

			if(enableBeat && !beating)
			{
				beating = true;
				waitGap = (contactCounter / 20.0F) + 0.5F;
				StartCoroutine(HeartTimer(waitGap));
			}
		}
	}

	IEnumerator HeartTimer(float waitTime)
	{
		//Play the sound
		Heart();
		
		//Wait time
		yield return new WaitForSeconds(waitTime);

		beating = false;
	}

	void Heart()
	{
		if(contactCounter >= 18)
		{
			//Play beat1
			beat1.Play();
		}
		else if(contactCounter >= 16)
		{
			//Play beat2
			beat2.Play();
		}
		else if(contactCounter >= 14)
		{
			//Play beat3
			beat3.Play();
		}
		else if(contactCounter >= 12)
		{
			//Play beat4
			beat4.Play();
		}
		else if(contactCounter >= 10)
		{
			//Play beat5
			beat5.Play();
		}
		else if(contactCounter >= 8)
		{
			//Play beat6
			beat6.Play();
		}
		else if(contactCounter >= 6)
		{
			//Play beat7
			beat7.Play();
		}
		else
		{
			//Play beat8
			beat8.Play();
		}
	}
}
