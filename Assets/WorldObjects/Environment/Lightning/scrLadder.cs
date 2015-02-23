using UnityEngine;
using System.Collections;

public class scrLadder : MonoBehaviour 
{
	public GameObject player;
	public bool canClimb = false;
	public float speed = 3.0F;

	//Use this for initialization
	void Start() 
	{
		player = GameObject.Find ("Player");
	}
	
	//Update is called once per frame
	void Update() 
	{
		if(canClimb)
		{
			if(Input.GetKey(KeyCode.W))
			{
				player.transform.Translate(Vector3.up * Time.deltaTime * speed);
			}
			if(Input.GetKey(KeyCode.S))
			{
				player.transform.Translate(Vector3.down * Time.deltaTime * speed);
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player)
		{
			canClimb = true;
		}
	}
}
