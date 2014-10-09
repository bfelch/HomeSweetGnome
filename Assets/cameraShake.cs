using UnityEngine;
using System.Collections;

public class cameraShake : MonoBehaviour 
{
	public float startingShakeDistance = 0.8f;
	public float decreasePercentage = 0.5f;
	public float shakeSpeed = 50.0f;
	public int numberOfShakes = 10;

	public void CameraShake()
	{
		float hitTime = Time.time;
		Vector3 originalPosition = transform.position;
		int shake = numberOfShakes;
		float shakeDistance = startingShakeDistance;

		while(shake > 0)
		{
			float timer = (Time.time - hitTime) * shakeSpeed;
			transform.position = new Vector3(originalPosition.x + Mathf.Sin(timer) * shakeDistance, transform.position.y, transform.position.z);

			if(timer > Mathf.PI * 2)
			{
				hitTime = Time.time;
				shakeDistance *= decreasePercentage;
				shake--;
			}
		}
		transform.position = originalPosition;
	}
}