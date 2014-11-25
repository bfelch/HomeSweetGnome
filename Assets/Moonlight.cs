using UnityEngine;
using System.Collections;

public class Moonlight : MonoBehaviour 
{
	private float lightIntensity = 1.0F;
	public bool lightFadeIn = false;
	public bool lightFadeOut = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
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
		if(lightIntensity < 1.0F)
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
		if(lightIntensity > 0.0F)
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
