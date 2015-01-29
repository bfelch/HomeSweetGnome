using UnityEngine;
using System.Collections;

/* This script can be attached to any light to make it flicker.
 * The game object name must be specified.
 */
public class scrFlicker : MonoBehaviour 
{
	private float prevFlickerTime = 0.0f; //Used to calculate next light flicker
	private float nextFlickerTime = 10.0f; //When the light will flicker next
	private int flicker = 0; //Flicker counter. (flicker 3 times out of 10)

    private int pattern; //Integer to pick a unique flicker pattern
    private int[][] patterns = {new int[]{1,5,8}, new int[]{2,5,7}, new int[]{3,5,8}}; //Flicker patterns

    public Material matLightOn; //Bright lightbulb material
    public Material matLightOff; //Transparent lightbulb material
	
	//Update is called once per frame
	void Update () 
	{
        checkFlickerTime();
	}

	//Checks flicker timer
	void checkFlickerTime()
	{
		//Is the current game time past the last flicker time + next flicker time?
		if(Time.time > prevFlickerTime + nextFlickerTime)
		{
			prevFlickerTime = Time.time; //Set previous flicker time to current game time
			nextFlickerTime = Random.Range(5, 15); //Set next flicker time

		    pattern = Random.Range(0,3); //Pick a flicker pattern
            InvokeRepeating("Flicker", 1.0F, 0.1F); //Start flicker pattern
		}
	}

	//Flickers the light
	void Flicker()
	{
		//Is the current flicker in the current pattern? (Turn light off)
        if(System.Array.IndexOf(patterns[pattern], flicker) >= 0)
        {
			//Is this game object a Lightbulb?
            if(this.gameObject.name == "LightBulb")
            {
                this.light.enabled = false; //Disable light
                this.renderer.material = matLightOff; //Switch material
                (GetComponent("Halo") as Behaviour).enabled = false; //Disable halo
            }
        }
		// The current flicker is not in the current pattern. (Turn light on)
        else
        {
			//Is this game object a Lightbulb?
            if(this.gameObject.name == "LightBulb")
            {
                this.light.enabled = true; //Enable light
                this.renderer.material = matLightOn; //Switch material
                (GetComponent("Halo") as Behaviour).enabled = true; //Enable halo
            }
        }

		flicker++; //Increase flicker count

		//Is the flicker counter at 10? (Reset)
		if(flicker >= 10)
		{
			flicker = 0; //Set flicker counter back to 0
			CancelInvoke("Flicker"); //Cancel flicker pattern
		}
	}
}
