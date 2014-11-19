using UnityEngine;
using System.Collections;

public class EventScript : MonoBehaviour 
{
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void Play2DAudio(AudioClip ac)
    {
        audio.clip= ac;
        audio.Play();
    }

    void Play3DAudio(AudioClip ac)
    {
        audio.clip = ac;
        AudioSource.PlayClipAtPoint(ac, transform.position);
    }
}
