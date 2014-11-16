using UnityEngine;
using UnityEngine;
using System.Collections;

public class cameraShake : MonoBehaviour 
{
	private float shakeSpeed = 20.0F;
	private Vector3 shakeRange = new Vector3(0.4F, 0.4F, 0);
	
	private bool shake = false;
	
	private Vector3 originalPos;
	
	private void Update()
	{
		if(shake)
		{
			//Shake and bake!
			transform.localPosition = originalPos + Vector3.Scale(SmoothRandom.GetVector2(shakeSpeed), shakeRange);
		}
	}
	
	public void StartShake()
	{
		originalPos = transform.localPosition;
		shake = true;
	}

	public void EndShake()
	{
		shake = false;
		transform.localPosition = originalPos;
	}
}
