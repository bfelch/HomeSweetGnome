using UnityEngine;
using System.Collections;

public class scrPaintNoise : MonoBehaviour 
{
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Wood")
		{
			audio.volume = 1.0F;
			audio.pitch = 0.4F + 0.2F * Random.value;
			audio.Play();
		}
	}
}
