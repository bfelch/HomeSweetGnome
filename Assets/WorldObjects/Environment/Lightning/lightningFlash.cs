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

	public AudioClip lightningStrike; //Lightning strike sound

	private float oldTime = 0.0f;
	private float newTime = 10.0f; //When the lightning flash happens next
	private int slot = 0; //Flash 3 times witin 10 slots
	private int direction = 0; //Lightning direction

    private AudioSource sound;

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
			newTime = Random.Range(15, 20); //Set next lightning flash
			oldTime = Time.time;

			direction = Random.Range(0,3); //Pick a light direction
			InvokeRepeating("Flash", 1.0f, 0.20f); //Start flash sequence
		}
	}

	void Flash()
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
			//Play the lightning strike sound
			//audio.PlayOneShot(lightningStrike);
			switch(direction)
			{
				case 0:
                    sound = PlayClipAt(lightningStrike, light.transform.position);
					break;
				case 1:
                    sound = PlayClipAt(lightningStrike, dir1.transform.position);
					break;
				case 2:
                    sound = PlayClipAt(lightningStrike, dir2.transform.position);
					break;
				default:
					//do nothing
					break;
			}

			slot = 0;
			CancelInvoke("Flash");
		}
	}

    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempSound = new GameObject("TempSound"); // create the temp object
        tempSound.transform.position = pos; // set its position
        AudioSource aSource = tempSound.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip

        // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        aSource.minDistance = 20;
        Destroy(tempSound, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }
}
