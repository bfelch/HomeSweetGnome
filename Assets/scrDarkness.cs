using UnityEngine;
using System.Collections;

public class scrDarkness : MonoBehaviour 
{
	bool eventStarted = false;
	GameObject[] lights;
	public GameObject darknessSpawner1;
	public GameObject darknessSpawner2;
	public GameObject gnome;
	public GameObject frontDoor1;
	public GameObject frontDoor2;

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
			frontDoor1.SetActive(false);
			frontDoor2.SetActive(false);
			transform.Find("DarknessTrigger").GetComponent<SphereCollider>().enabled = false;
			TurnOffLights();
		}

		//Play the laugh sound
		GetComponent<AudioSource>().Play();

		StartCoroutine(EventTimer(5.0F));
	}

	public void EndDarkEvent()
	{
		Debug.Log ("End");
		transform.GetComponent<MeshCollider>().enabled = false;
		frontDoor1.SetActive(true);
		frontDoor2.SetActive(true);
		TurnOnLights();
	}

	public IEnumerator EventTimer(float waitTime)
	{
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);

		//Spawn Gnomes
		Instantiate(gnome, darknessSpawner1.transform.position, Quaternion.identity);
		Instantiate(gnome, darknessSpawner2.transform.position, Quaternion.identity);
	}

	//Turn off all lights
	void TurnOffLights()
	{
		foreach(GameObject light in lights)
		{
			light.GetComponent<Light>().enabled = false;
		}

		weatherScript weather = GameObject.Find ("Weather").GetComponent<weatherScript>();
		weather.StopWeather();

		RenderSettings.ambientLight = Color.black;
	}

	//Turn on all lights
	void TurnOnLights()
	{
		foreach(GameObject light in lights)
		{
			light.GetComponent<Light>().enabled = true;
		}
		
		weatherScript weather = GameObject.Find ("Weather").GetComponent<weatherScript>();
		weather.StartWeather();
		
		RenderSettings.ambientLight =  new Color32(62, 64, 73, 255);
	}
}
