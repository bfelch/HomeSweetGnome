using UnityEngine;
using System.Collections;

public class BoatSounds : MonoBehaviour 
{
	public AudioSource boatStartSound;
	public AudioSource boatMotorSound;

	// Use this for initialization
	void Start () 
	{
		AudioSource[] boatSounds = GameObject.Find("BoatSounds").GetComponents<AudioSource>();
		boatStartSound = boatSounds[0];
		boatMotorSound = boatSounds[1];
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
