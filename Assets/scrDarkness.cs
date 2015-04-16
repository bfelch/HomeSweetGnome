using UnityEngine;
using System.Collections;

public class scrDarkness : MonoBehaviour 
{
	bool eventStarted = false;
	bool eventOver = false;
	GameObject[] lights;
	private GameObject darkChild;
	public GameObject chandTrap;
	public GameObject darknessSpawner1;
	public GameObject darknessSpawner2;
	public GameObject gnome1;
	public GameObject gnome2;
	public GameObject frontDoor1;
	public GameObject frontDoor2;

	private bool dropOnce = false;

	void Start()
	{
		StartCoroutine(DelayedStart(5.0F));
	}

	void Update()
	{
		if(eventOver  && Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) >= 7.0F && !dropOnce)
		{
			chandTrap.GetComponent<scrDropTrap>().Drop();
			dropOnce = true;
		}
	}

	public IEnumerator DelayedStart(float waitTime)
	{
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);

		//darkChild = transform.Find("DarknessTrigger").gameObject;

		GetComponent<MeshCollider>().enabled = true;
		GetComponent<MeshCollider>().enabled = false;
		//this.gameObject.GetComponent<MeshCollider> ().enabled = false;
		
		GetComponentInChildren<SphereCollider>().enabled = true;
		GetComponentInChildren<SphereCollider>().enabled = false;
	}

	public void PrepareEvent()
	{
		//Trigger
		GetComponentInChildren<SphereCollider>().enabled = true;
		//Player turns on Chand (Some Sparks)
	}

	public void DarknessEvent()
	{
		if(!eventStarted)
		{
			eventStarted = true;
			transform.GetComponent<MeshCollider>().enabled = true;
			//frontDoor1.SetActive(false);
			//frontDoor2.SetActive(false);
			TurnOffLights();

			//Play the laugh sound
			GetComponent<AudioSource>().Play();

			StartCoroutine(EventTimer(5.0F));
		}
	}

	public void EndDarkEvent()
	{
		transform.GetComponent<MeshCollider>().enabled = false;
		//frontDoor1.SetActive(true);
		//frontDoor2.SetActive(true);
		TurnOnLights();
		GetComponentInChildren<SphereCollider>().enabled = false;

		eventOver = true;
	}

	public IEnumerator EventTimer(float waitTime)
	{
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);

		//Get player object
		GameObject player = GameObject.Find("Player");

		//retrieve current player sanity values
		float playerSanity = player.GetComponent<Player>().sanity;
		float playerSanityMax = player.GetComponent<Player>().maxSanity;
		//calculate sanity percentage
		float sanityPercentage = playerSanity / playerSanityMax;

		//Spawn Gnomes
		if(sanityPercentage > 0.4F)
		{
			Instantiate(gnome1, darknessSpawner1.transform.position, Quaternion.identity);
			Instantiate(gnome1, darknessSpawner2.transform.position, Quaternion.identity);
		}
		else
		{
			Instantiate(gnome2, darknessSpawner1.transform.position, Quaternion.identity);
			Instantiate(gnome2, darknessSpawner2.transform.position, Quaternion.identity);
		}
	}

	//Turn off all lights
	void TurnOffLights()
	{
		lights = GameObject.FindGameObjectsWithTag("Light");

		foreach(GameObject light in lights)
		{
			if(light.name == "Lamp")
			{
				light.GetComponentInChildren<Light>().enabled = false;
			}
			else
			{
				light.GetComponent<Light>().enabled = false;
			}

            if((light.GetComponent("Halo") as Behaviour) != null)
			{
                (light.GetComponent("Halo") as Behaviour).enabled = false;
			}
		}

		weatherScript weather = GameObject.Find ("Weather").GetComponent<weatherScript>();
		weather.StopWeather();

		RenderSettings.ambientLight = Color.black;
	}

	//Turn on all lights
	void TurnOnLights()
	{
		lights = GameObject.FindGameObjectsWithTag("Light");

		foreach(GameObject light in lights)
		{
			if(light.name == "Lamp")
			{
				light.GetComponentInChildren<Light>().enabled = true;
			}
			else
			{
				light.GetComponent<Light>().enabled = true;
			}
            if ((light.GetComponent("Halo") as Behaviour) != null)
                (light.GetComponent("Halo") as Behaviour).enabled = true;
		}
		
		weatherScript weather = GameObject.Find ("Weather").GetComponent<weatherScript>();
		weather.StartWeather();
		
		RenderSettings.ambientLight =  new Color32(35, 35, 45, 255);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "Player")
		{
			DarknessEvent();
		}
	}
}
