using UnityEngine;
using System.Collections;

/* This script is an adaptable heartbeat sound system
 * that plays a heartbeat (slower to faster and visa versa) 
 * based on the time the player is in contact with danger.
 */
public class scrHeartbeat : MonoBehaviour 
{
	private GUIDamage guiDamageScript; //GUIDamage Script
	private float contactCounter; //How fast is the heartbeat?
	private float waitGap; //How long to wait between each heartbeat
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

	//Use this for initialization
	void Start() 
	{
		//Get player's GUI damage script
		guiDamageScript = GameObject.Find("Player").GetComponent<GUIDamage>();

		//Grab all the audio sources on this object
		AudioSource[] aSources = GetComponents<AudioSource>();
		//Set all the audio sources
		beat1 = aSources[0];
		beat2 = aSources[1];
		beat3 = aSources[2];
		beat4 = aSources[3];
		beat5 = aSources[4];
		beat6 = aSources[5];
		beat7 = aSources[6];
		beat8 = aSources[7];
	}
	
	//Update is called once per frame
	void Update () 
	{
		//Get the death variables from the pllayer
		deathFall = GameObject.Find("Player").GetComponent<EndGames>().playerFell;
		deathSleep = GameObject.Find("Player").GetComponent<EndGames>().playerSlept;

		//Is the player still alive?
		if(!deathFall && !deathSleep)
		{
			//Update variable from other script (how long player is in contact with danger)
			contactCounter = guiDamageScript.damageTimer;

			//Is the player not in contact with danger?
			if(contactCounter >= 20.0F)
			{
				enableBeat = false; //Enable heartbeat
			}
			//Is the player in contact with danger?
			else
			{
				enableBeat = true; //Disable hearbeat
			}

			//Make sure one beat sound is played at a time
			if(enableBeat && !beating)
			{
				beating = true; //Beat sound is being played
				waitGap = (contactCounter / 20.0F) + 0.5F; //Calculate time between beats based on contact with danger
				StartCoroutine(HeartTimer(waitGap)); //Heat beat coroutine
			}
		}
	}

	//Called when in contact with danger based on waitGap (time between beats)
	IEnumerator HeartTimer(float waitTime)
	{
		Heart(); //Play the heat beat

		yield return new WaitForSeconds(waitTime); //Wait the time between beats

		beating = false; //Beat sound is done playing
	}

	//Chooses a beat to play based off contact with danger
	void Heart()
	{
		//Higher contactCounter plays a slower heartbeat
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
