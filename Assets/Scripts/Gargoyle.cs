using UnityEngine;
using System.Collections;

public class Gargoyle : MonoBehaviour 
{
	public float rotateSpeed = 0.2f;
	private float oldTime = 0.0f;
	public float switchTime = 8.0f;
	private bool lookRight = false;
	private GameObject player;
	private bool screeching = false;

	private Player playerScript;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!screeching)
		{
		 	LookAround();
		}
	}

	void LookAround()
	{
		if(Time.time > oldTime + switchTime)
		{
			Debug.Log("switch");
			oldTime = Time.time;
			lookRight = !lookRight;
		}

		if(lookRight == true)
		{
			//Debug.Log("One");
			transform.Rotate(Vector3.up * rotateSpeed);
		}
		else
		{
			//Debug.Log("Two");
			transform.Rotate(Vector3.down * rotateSpeed);
		}
	}

	void Screech()
	{
		screeching = true;

		//stop player here
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player)
		{
			playerScript = other.GetComponent<Player>();
			Screech();
		}
	}
}
