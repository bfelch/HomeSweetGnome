using UnityEngine;
using System.Collections;

public class groundCheck : MonoBehaviour 
{
	private float distToGround; //Center of gargoyle to floor
	private float distToEdge; //Center of gargoyle to edge
	private bool falling = false;
	public GameObject shatterEffect; //Shatter Effect Prefab

	// Use this for initialization
	void Start () 
	{
		//Get distance to ground
		distToGround = collider.bounds.extents.y;
		//Get distance to sides
		distToEdge = collider.bounds.extents.x;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!IsGrounded())
		{
			falling = true;
		}
		else
		{
			if(falling == true)
			{
				//Emit shatter effect
				Instantiate(shatterEffect, new Vector3(transform.position.x, transform.position.y - 0.8F, transform.position.z), shatterEffect.transform.rotation);

				//Delete gargoyle
				Destroy(transform.parent.gameObject);
			}
			else
			{
				//Do nothing
			}
		}
	}

	bool IsGrounded() 
	{
		//Enemy layer mask
		int enemyLayer = 9;
		int enemyMask = 1 << enemyLayer;
		
		//Invert bitmask to only ignore this layer
		enemyMask = ~enemyMask;

		if(Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1F, enemyMask)
		   || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + distToEdge), Vector3.down, distToGround + 0.1F, enemyMask)
		   || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - distToEdge), Vector3.down, distToGround + 0.1F, enemyMask))
		{
			//Grounded
			return true;
		}
		else
		{
			//Not grounded
			return false;
		}
	}
}
