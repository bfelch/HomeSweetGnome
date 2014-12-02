using UnityEngine;
using System.Collections;

/*
 * Description: This script handles the transition of the moonlight while in certain areas
*/
public class Moonlight : MonoBehaviour 
{
	private float lightIntensity = 1.0F; //Starting moonlight intensity
	private float maxIntensity = 1.0F; //Max moonlight intensity
	private float minIntensity = 0.0F; //Min moonlight intensity
	public bool lightFadeIn = false; //Light is fading in
	public bool lightFadeOut = false; //Light is fading out

	//Use this for initialization
	void Start() 
	{
	
	}
	
	//Update is called once per frame
	void Update() 
	{
		//Check if the light needs to fade in or out
		if(lightFadeIn == true)
		{
			LightFadeIn();
		}
		else if(lightFadeOut == true)
		{
			LightFadeOut();
		}
	}

	void LightFadeIn()
	{
		//Increase intensity till max
		if(lightIntensity < maxIntensity)
		{
			lightIntensity += 0.5F * Time.deltaTime;
			this.light.intensity = lightIntensity;
		}
		else
		{
			lightFadeIn = false;
		}
	}

	void LightFadeOut()
	{
		//Decrease intensity till min
		if(lightIntensity > minIntensity)
		{
			lightIntensity -= 0.5F * Time.deltaTime;
			this.light.intensity = lightIntensity;
		}
		else
		{
			lightFadeOut = false;
		}
	}
}
