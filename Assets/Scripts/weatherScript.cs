using UnityEngine;
using System.Collections;

public class weatherScript : MonoBehaviour 
{
	public GameObject lightning;
	public GameObject stormClouds;
	public GameObject fog;
	public GameObject heavyRain;
	public GameObject rainSheet;
	float minRate = 0.2f;
	float maxRate = 0.8f;
	float rate = 0.2f;
	float oldTime = 0.0f;
	float newTime = 2.0f;

	float oldWeatherTime = 0.0f;
	float newWeatherTime = 20.0f;
	int weatherType = 1;

	public GameObject[] weather;

	/*
	int NONE = 0;
	int FOG = 1;
	int CLOUDS = 2;
	int THUNDERSTORM = 3;
	int RAIN = 4;
	int ALL = 5;
	*/

	// Use this for initialization
	void Start () 
	{
		weather = GameObject.FindGameObjectsWithTag("Weather");

		for(int i = 0; i < weather.Length; i++)
		{
			weather[i].particleSystem.enableEmission = false;
		}

		lightning = GameObject.Find("Clouds Stormy/Lightning");
		stormClouds = GameObject.Find("Clouds Stormy");
		fog = GameObject.Find("Fog");
		heavyRain = GameObject.Find("Rain Heavy");
		rainSheet = GameObject.Find ("Rain Heavy/RainSheet");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time > oldTime + newTime)
		{
			oldTime = Time.time;
			rate = Random.Range(minRate, maxRate);
			lightning.particleSystem.emissionRate = rate;
		}

		if (Time.time > oldWeatherTime + newWeatherTime) 
		{
			oldWeatherTime = Time.time;
			weatherType = Random.Range(0, 5);

			for(int i = 0; i < weather.Length; i++)
			{
				weather[i].particleSystem.enableEmission = false;
			}

			Debug.Log(weatherType);

			switch(weatherType)
			{
				case 0:
					//do nothing
					break;
				case 1:
					fog.particleSystem.enableEmission = true;
					break;
				case 2:
					stormClouds.particleSystem.enableEmission = true;
					break;
				case 3:
					stormClouds.particleSystem.enableEmission = true;
					lightning.particleSystem.enableEmission = true;
					break;
				case 4:
					stormClouds.particleSystem.enableEmission = true;
					heavyRain.particleSystem.enableEmission = true;
					rainSheet.particleSystem.enableEmission = true;
					fog.particleSystem.enableEmission = true;
					break;
				case 5:
					for(int i = 0; i < weather.Length; i++)
					{
						Debug.Log(weather[i].name);
						weather[i].particleSystem.enableEmission = true;
					}
					break;
				default:
					//do nothing
					break;
			}
		}
	}
}
