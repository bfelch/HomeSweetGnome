using UnityEngine;
using System.Collections;

public class cameraShake : MonoBehaviour 
{
	private float shakeSpeed = 50f;
	private Vector3 shakeRange = new Vector3(1, 1, 1);
	private float shakeTimer = 0f;
	private const float shakeTime = 2f;

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

				Camera.main.transform.position = originalPos;
			}
			else
			{
				shakeTimer += Time.deltaTime;

				//Shake and bake!
				Camera.main.transform.position = originalPos + Vector3.Scale(SmoothRandom.GetVector2(shakeSpeed--), shakeRange);

				shakeSpeed *= -1;
				shakeRange = new Vector3((shakeRange.x * -1), shakeRange.y);
			}
		}
	}

	public void CameraShake()
	{
		originalPos = Camera.main.transform.position;

		shakeSpeed = 50;
		shake = true;
	}
}