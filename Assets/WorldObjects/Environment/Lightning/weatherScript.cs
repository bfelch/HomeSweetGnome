using UnityEngine;
using System.Collections;

public class weatherScript : MonoBehaviour 
{
	public GameObject farLightning1;
    public GameObject farLightning2;
    public GameObject farLightning3;
    public GameObject farLightning4;
	public GameObject rain;
	//public GameObject fog;
	public GameObject lightRain;
	public GameObject heavyRain;

	public GameObject[] weather; //Array to hold all weather particle systems

	//Distant lightning variables
	float minRate = 0.05F;
	float maxRate = 0.2F;
	float rate = 0.2F;
	float oldTime = 0.0F;
	float newTime = 2.0F;

    scrLightFlash lightningScript;

    float rainVolume = 0.07F;
    bool rainFadeIn = false;
    bool rainFadeOut = false;
	private bool newWeather = false;
	private bool weatherOn = true;
	public bool changeWeather = false; //Tutorial

	float oldWeatherTime = 0.0F;
	float newWeatherTime = 20.0F; //How often to change weather
	public int weatherType = 0; //What's the weather?

	// Use this for initialization
	void Start () 
	{
		weather = GameObject.FindGameObjectsWithTag("Weather");

		//Game starts with thunderstorm
		for(int i = 0; i < weather.Length; i++)
		{
			weather[i].particleSystem.enableEmission = true;
		}

		//farLightning1 = GameObject.Find("FarLightning1");
        //farLightning2 = GameObject.Find("FarLightning2");
        //farLightning3 = GameObject.Find("FarLightning3");
        //farLightning4 = GameObject.Find("FarLightning4");
		//fog = GameObject.Find("Fog");
		lightRain = GameObject.Find ("Light Rain");
		//heavyRain = GameObject.Find("Heavy Rain");
        rain = GameObject.Find("Rain");

        lightningScript = GameObject.Find("LightFlash1").GetComponent<scrLightFlash>();
	}
	
	//Update is called once per frame
	void Update() 
	{
		if(changeWeather)
		{
			CheckWeather();
		}

        if(newWeather && weatherOn)
        {
            ChangeWeather();
		}

	    if (rainFadeIn == true)
	    {
	        AudioFadeIn();
	    }
	    else if (rainFadeOut == true)
	    {
	        AudioFadeOut();
	    }
	}

	void CheckWeather()
	{
		//Random time for distant lightning
		if(Time.time > oldTime + newTime)
		{
			oldTime = Time.time;
			rate = Random.Range(minRate, maxRate);
			//farLightning1.particleSystem.emissionRate = rate;
            //farLightning2.particleSystem.emissionRate = rate;
            //farLightning3.particleSystem.emissionRate = rate;
            //farLightning4.particleSystem.emissionRate = rate;
		}
		
		//Change weather?
		if(Time.time > oldWeatherTime + newWeatherTime) 
		{
			oldWeatherTime = Time.time;
			weatherType = Random.Range(0, 3); //Pick random weather
			newWeather = true;
		}
	}

	void ChangeWeather()
	{
		//Turn off weather
		for(int i = 0; i < weather.Length; i++)
		{
			weather[i].particleSystem.enableEmission = false;
		}

    	lightningScript.enabled = false;
		
		//Turn on weather
		switch(weatherType)
		{
			//Fog
			case 0:
				//fog.particleSystem.enableEmission = true;
				rainFadeIn = false;
            	rainFadeOut = true;
				break;
			//Thunderstorm
			case 1:
				lightRain.particleSystem.enableEmission = true;
	            //heavyRain.particleSystem.enableEmission = true;
	            //farLightning1.particleSystem.enableEmission = true;
	            //farLightning2.particleSystem.enableEmission = true;
	            //farLightning3.particleSystem.enableEmission = true;
	            //farLightning4.particleSystem.enableEmission = true;
	            lightningScript.enabled = true;
	            rainFadeIn = true;
				break;
			//All weather
			case 2:
				for(int i = 0; i < weather.Length; i++)
				{
					weather[i].particleSystem.enableEmission = true;
				}
	            lightningScript.enabled = true;
	            rainFadeIn = true;
				break;
			//Catch all
			default:
				//do nothing
            	rainFadeOut = true;
				break;
		}

		newWeather = false;
	}

	public void StopWeather()
	{
		//Turn off weather
		for(int i = 0; i < weather.Length; i++)
		{
			weather[i].particleSystem.enableEmission = false;
		}
		
		lightningScript.enabled = false;
		rainFadeIn = false;
		rainFadeOut = true;
		weatherOn = false;
	}

	public void StartWeather()
	{
		newWeather = true;
		weatherOn = true;

	}

    void AudioFadeIn()
    {
        if (rainVolume < 0.07F)
        {
            rainVolume += 0.02F * Time.deltaTime;
            rain.audio.volume = rainVolume;
        }
        else
        {
            rainFadeIn = false;
        }
    }

    void AudioFadeOut()
    {
        if (rainVolume > 0.0F)
        {
            rainVolume -= 0.02F * Time.deltaTime;
            rain.audio.volume = rainVolume;
        }
        else
        {
            rainFadeOut = false;
        }
    }
}
