using UnityEngine;
using System.Collections;

/*
	Script for handling nearby lightning flashes.  It will pick a directional light, 
	set random time intervals, and create a realistic environment lightning effect.
*/
public class lightningFlash : MonoBehaviour 
{
	public GameObject dir1; //Light1
	public GameObject dir2; //Light2

	float oldTime = 0.0f;
	float newTime = 5.0f; //When the lightning flash happens next
	int slot = 0; //Flash 3 times witin 10 slots
	int direction = 0; //Lightning direction

	// Use this for initialization
	void Start () 
	{
		dir1 = GameObject.Find("Dir1");
		dir2 = GameObject.Find("Dir2");
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkFlashTime();
	}

	void checkFlashTime()
	{
		if(Time.time > oldTime + newTime)
		{
			//newTime = Random.Range(15, 20); //Set next lightning flash
            newTime = 4; //for testing
			oldTime = Time.time;

			direction = Random.Range(0,3); //Pick a light direction
			InvokeRepeating("flash", 1.0f, 0.20f); //Start flash sequence
		}
	}

	void flash()
	{
		//Preset flash pattern
		if(slot == 2 || slot == 4 || slot == 8)
		{
			//Turn on light
			switch(direction)
			{
				case 0:
					light.intensity = 1.0f;
					break;
				case 1:
					dir1.light.intensity = 1.0f;
					break;
				case 2:
					dir2.light.intensity = 1.0f;
					break;
				default:
					//do nothing
					break;
			}
		}
		else
		{
			//Turn off light
			switch(direction)
			{
			case 0:
				light.intensity = 0;
				break;
			case 1:
				dir1.light.intensity = 0;
				break;
			case 2:
				dir2.light.intensity = 0;
				break;
			default:
				//do nothing
				break;
			}
		}

		slot++;

		//Reset
		if(slot >= 10)
		{
			slot = 0;
			CancelInvoke("flash");
		}
	}
}
