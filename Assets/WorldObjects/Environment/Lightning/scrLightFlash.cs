using UnityEngine;
using System.Collections;

/*
 * Script for handling nearby lightning flashes.  It will pick a directional light, 
 * set random time intervals, and create a realistic lightning enviornment effect.
*/
public class scrLightFlash : MonoBehaviour 
{
	public GameObject flash2; //Directional light 1
	public GameObject flash3; //Directional light 2

	public AudioClip lightningStrike; //Lightning strike sound

	private float prevFlashTime = 0.0F; //Used to calculate next light flash
	private float nextFlashTime = 10.0F; //When the light will flash next
	private int flash = 0; //Flash counter (flash 3 times out of 10)

	private int direction = 0; //Integer to pick a unique flash direction
	private int pattern; //Integer to pick a unique flash pattern
	private int[][] patterns = {new int[]{1,5,8}, new int[]{2,5,7}, new int[]{3,5,8}}; //Flash patterns

    private AudioSource sound; //To play lightning strike sound

	//Use this for initialization
	void Start () 
	{
		//Get the other two directional lights
		flash2 = GameObject.Find("LightFlash2");
		flash3 = GameObject.Find("LightFlash3");
	}
	
	//Update is called once per frame
	void Update () 
	{
		checkFlashTime();
	}

	//Checks flash timer
	void checkFlashTime()
	{
		//Is the current game time past the last flash time + next flash time?
		if(Time.time > prevFlashTime + nextFlashTime)
		{
			prevFlashTime = Time.time; //Set previous flash time to current game time
			nextFlashTime = Random.Range(15, 25); //Set next flash time

			direction = Random.Range(0,3); //Pick a light direction
			pattern = Random.Range(0,3); //Pick a flash pattern
			InvokeRepeating("Flash", 1.0F, 0.20F); //Start flash sequence
		}
	}

	//The actual light flash sequence
	void Flash()
	{
		//Is the current flash in the current pattern? (Turn light on)
		if(System.Array.IndexOf(patterns[pattern], flash) >= 0)
		{
			switch(direction)
			{
				case 0:
					light.intensity = 0.7F;
					break;
				case 1:
					flash2.light.intensity = 0.7F;
					break;
				case 2:
					flash2.light.intensity = 0.07F;
					break;
				default:
					//Do nothing
					break;
			}
		}
		//The current flash is not in the current pattern. (Turn light off)
		else
		{
			switch(direction)
			{
				case 0:
					light.intensity = 0;
					break;
				case 1:
					flash2.light.intensity = 0;
					break;
				case 2:
					flash2.light.intensity = 0;
					break;
				default:
					//Do nothing
					break;
			}
		}
		
		flash++; //Increase flash counter
		
		//Is the flash counter at 10? (Reset)
		if(flash >= 10)
		{
			//Play the lightning strike sound from directional light's position
			switch(direction)
			{
			case 0:
				sound = PlayClipAt(lightningStrike, light.transform.position);
				break;
			case 1:
				sound = PlayClipAt(lightningStrike, flash2.transform.position);
				break;
			case 2:
				sound = PlayClipAt(lightningStrike, flash2.transform.position);
				break;
			default:
				//Do nothing
				break;
			}

			flash = 0; //Set flash counter back to 0
			CancelInvoke("Flash"); //Cancel flash pattern
		}
	}

	//Custom PlayClipAt method.  Plays desired clip at a desired position
    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
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
