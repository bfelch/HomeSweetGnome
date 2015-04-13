using UnityEngine;
using System.Collections;

public class scrTunnelEvent : MonoBehaviour 
{
	private bool enableEvent1 = false;
	private bool enableEvent2 = false;

	private bool eventCompleted = false;

	public GameObject gnome1;
	public GameObject gnome2;

	public AudioClip laugh;

	public GameObject[] tunnelLights1;
	public GameObject[] tunnelLights2;

	public Material matLightOff;

	// Use this for initialization
	void Start () 
	{
	
	}

	public void CheckTunnelEvent(int spawnCheck)
	{
		if(enableEvent1 && spawnCheck == 1)
		{
			enableEvent1 = false;

			gnome1.SetActive(false);
		}

		if(enableEvent2 && spawnCheck == 2)
		{
			enableEvent2 = false;
			
			gnome2.SetActive(false);
		}
	}

	public void PrepareTunnelEvent(int spawn)
	{
		if(spawn == 1 && !enableEvent1 && !eventCompleted)
		{
			enableEvent1 = true;
			//Turn off music
			
			//Enable gnome1
			gnome1.SetActive(true);
		}
		else if(spawn == 2 && !enableEvent2 && !eventCompleted)
		{
			enableEvent2 = true;
			//Turn off music
			
			//Enable gnome2
			gnome2.SetActive(true);
		}
	}

	public void StartTunnelEvent(int spawn)
	{
		if(spawn == 1 && enableEvent1)
		{
			enableEvent1 = false;
			
			//Disable gnome1
			gnome1.SetActive(false);

			//Disable nearby tunnel lights
			foreach(GameObject light in tunnelLights1)
			{
				light.GetComponent<Light>().enabled = false;
			}

			//Play tense music
			GameObject.Find("GlobalSoundController").GetComponent<SoundController>().violinSound.Play();

			StartCoroutine(EndTunnelEvent(4.5F, 1));
		}
		else if(spawn == 2 && enableEvent2)
		{
			enableEvent2 = false;
			
			//Disable gnome2
			gnome2.SetActive(false);
			
			//Disable nearby tunnel lights
			foreach(GameObject light in tunnelLights2)
			{
				light.GetComponent<Light>().enabled = false;
			}
			
			//Play tense music
			GameObject.Find("GlobalSoundController").GetComponent<SoundController>().violinSound.Play();
			
			StartCoroutine(EndTunnelEvent(4.5F, 2));
		}
	}

	IEnumerator EndTunnelEvent(float waitTime, int spawn)
	{
		//Wait before loading the main menu
		yield return new WaitForSeconds(waitTime);

		if(spawn == 1)
		{
			//Play gnome laugh
			AudioSource.PlayClipAtPoint(laugh, new Vector3(24.28F, -19.73F, 32.93F));
			
			//Flicker Lights On
			foreach(GameObject light in tunnelLights1)
			{
				light.GetComponent<Light>().enabled = true;
				light.GetComponent<scrFlicker>().oneTimeFlicker();
			}
		}
		else if(spawn == 2)
		{
			//Play gnome laugh
			AudioSource.PlayClipAtPoint(laugh, new Vector3(-66.9F, -1.09F, 48.53F));
			
			//Flicker Lights On
			foreach(GameObject light in tunnelLights2)
			{
				light.GetComponent<Light>().enabled = true;
				light.GetComponent<scrFlicker>().oneTimeFlicker();
			}
		}

		eventCompleted = true;
	}
}
