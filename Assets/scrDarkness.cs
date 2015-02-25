using UnityEngine;
using System.Collections;

public class scrDarkness : MonoBehaviour 
{
	bool eventStarted = false;
	GameObject[] lights; 

	// Use this for initialization
	void Start () 
	{
		lights = GameObject.FindGameObjectsWithTag("Light");
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void DarknessEvent()
	{
		if(!eventStarted)
		{
			eventStarted = true;
			transform.GetComponent<MeshCollider>().enabled = true;
			transform.Find("DarknessTrigger").GetComponent<SphereCollider>().enabled = false;
			TurnOffLights();
		}
	}

	void TurnOffLights()
	{
		foreach(GameObject light in lights)
		{
			light.GetComponent<Light>().enabled = false;
		}

		weatherScript weather = GameObject.Find ("Weather").GetComponent<weatherScript>();
		weather.StopWeather();

		RenderSettings.ambientLight = Color.black;

		(Camera.main.GetComponent("GlobalFog") as MonoBehaviour).enabled = false;
	}
}
