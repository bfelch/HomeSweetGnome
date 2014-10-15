using UnityEngine;
using System.Collections;

public class weatherScript : MonoBehaviour 
{
	public GameObject lightning;
	public GameObject clouds;
	public GameObject fog;
	public GameObject lightRain;
	public GameObject heavyRain;

	public GameObject[] weather; //Array to hold all weather particle systems

	//Distant lightning variables
	float minRate = 0.2f;
	float maxRate = 0.8f;
	float rate = 0.2f;
	float oldTime = 0.0f;
	float newTime = 2.0f;

	float oldWeatherTime = 0.0f;
	float newWeatherTime = 20.0f; //How often to change weather
	int weatherType = 0; //What's the weather?

	// Use this for initialization
	void Start () 
	{
		weather = GameObject.FindGameObjectsWithTag("Weather");

		for(int i = 0; i < weather.Length; i++)
		{
			weather[i].particleSystem.enableEmission = false;
		}

		lightning = GameObject.Find("Lightning");
		clouds = GameObject.Find("Clouds");
		fog = GameObject.Find("Fog");
		lightRain = GameObject.Find ("Light Rain");
		heavyRain = GameObject.Find("Heavy Rain");
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkWeatherTime();
	}

	void checkWeatherTime()
	{
		//Random time for distant lightning
		if(Time.time > oldTime + newTime)
		{
			oldTime = Time.time;
			rate = Random.Range(minRate, maxRate);
			lightning.particleSystem.emissionRate = rate;
		}
		
		//Change weather?
		if(Time.time > oldWeatherTime + newWeatherTime) 
		{
			oldWeatherTime = Time.time;
			weatherType = Random.Range(0, 5); //Pick random weather
			
			//Turn off weather
			for(int i = 0; i < weather.Length; i++)
			{
				weather[i].particleSystem.enableEmission = false;
			}
			
			//Turn on weather
			switch(weatherType)
			{
				//Fog
				case 0:
					//fog.particleSystem.enableEmission = true;
					break;
				//Light Rain
				case 1:
					clouds.particleSystem.enableEmission = true;
					lightRain.particleSystem.enableEmission = true;
					break;
				//Heavy Rain
				case 2:
					clouds.particleSystem.enableEmission = true;
					lightRain.particleSystem.enableEmission = true;
					heavyRain.particleSystem.enableEmission = true;
					break;
				//Thunderstorm
				case 3:
					clouds.particleSystem.enableEmission = true;
					lightRain.particleSystem.enableEmission = true;
					heavyRain.particleSystem.enableEmission = true;
					lightning.particleSystem.enableEmission = true;
					break;
				//All weather
				case 4:
					for(int i = 0; i < weather.Length; i++)
					{
						weather[i].particleSystem.enableEmission = true;
					}
					break;
				//Catch all
				default:
					//do nothing
					break;
			}
		}
	}
}
