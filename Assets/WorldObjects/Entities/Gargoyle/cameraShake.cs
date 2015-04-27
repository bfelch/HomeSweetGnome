using UnityEngine;
using UnityEngine;
using System.Collections;

public class cameraShake : MonoBehaviour 
{
	private float shakeSpeed = 20.0F;
	private Vector3 shakeRange = new Vector3(2.0F, 2.0F, 0);
	
	public bool shake = false;
	
	private Vector3 originalPos;
	private Vector3 adjustedPos;
	
	private void Update()
	{
		if(shake)
		{
			//Shake and bake!
			transform.localPosition = adjustedPos + Vector3.Scale(SmoothRandom.GetVector2(shakeSpeed), shakeRange);
		}
	}
	
	public void StartShake()
	{
		originalPos = transform.localPosition;
		adjustedPos = new Vector3(originalPos.x-0.5F, originalPos.y-0.5F, originalPos.z);
		shake = true;
	}

	public void EndShake()
	{
		shake = false;
		transform.localPosition = originalPos;
	}
}
