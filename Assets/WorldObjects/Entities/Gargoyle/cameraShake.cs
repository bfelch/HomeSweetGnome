using UnityEngine;
using UnityEngine;
using System.Collections;

public class cameraShake : MonoBehaviour 
{
	private float shakeSpeed = 30.0F;
	private Vector3 shakeRange = new Vector3(0.5F, 0.5F, 0);
	private float shakeTimer = 0.0F;
	private const float shakeTime = 2.0F;
	
	private bool shake = false;
	
	private Vector3 originalPos;
	
	private void Update()
	{
		if(shake)
		{
			if(shakeTimer > shakeTime)
			{
				shakeTimer = 0;
				shake = false;
				
				transform.localPosition = originalPos;
			}
			else
			{
				shakeTimer += Time.deltaTime;
				
				//Shake and bake!
				transform.localPosition = originalPos + Vector3.Scale(SmoothRandom.GetVector2(shakeSpeed--), shakeRange);
				
				shakeSpeed *= -1;
				shakeRange = new Vector3((shakeRange.x * -1), shakeRange.y);
			}
		}
	}
	
	public void CameraShake()
	{
		originalPos = transform.localPosition;
		
		shakeSpeed = 30;
		shake = true;
	}
}
