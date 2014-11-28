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
				BreakApart();
			}
		}
	}

	//Shatters the gargoyle when pushed and spawns the gargoyle head pickup
	void BreakApart()
	{
		//Emit shatter effect
		Instantiate(shatterEffect, new Vector3(transform.position.x, transform.position.y - 0.8F, transform.position.z), shatterEffect.transform.rotation);
		
		//Get the gargoyle head game object
		GameObject gargoyleHead = transform.parent.Find("GargoyleHead").gameObject;
		
		//Remove gargoyle script component
		Destroy (gargoyleHead.GetComponent<Gargoyle>());
		
		//Remove children
		foreach(Transform child in gargoyleHead.transform)
		{
			Destroy(child.gameObject);
		}
		
		//Remove audio source component
		Destroy(gargoyleHead.GetComponent<AudioSource>());
		
		//Apply physics to the head
		gargoyleHead.GetComponent<Rigidbody>().isKinematic = false;
		gargoyleHead.GetComponent<Rigidbody>().useGravity = true;
		
		//Add useable script
		gargoyleHead.AddComponent<Item>();
		
		//Change tag
		gargoyleHead.tag = "PickUp";

		//Change item type
		gargoyleHead.GetComponent<Item> ().type = ItemType.ATTIC;
		
		//Move head to item list
		gargoyleHead.transform.parent = GameObject.Find("Items").transform;
		
		//Delete gargoyle
		Destroy(transform.parent.gameObject);
	}

	//Checks if gargyole is on the ground
	bool IsGrounded() 
	{
		//Enemy layer mask
		int enemyLayer = 9;
		int invisibleLayer = 10;
		int ignoreMask = 1 << enemyLayer | 1 << invisibleLayer;
		
		//Invert bitmask to only ignore this layer
		ignoreMask = ~ignoreMask;

		if(Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1F, ignoreMask)
		   || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + distToEdge), Vector3.down, distToGround + 0.1F, ignoreMask)
		   || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - distToEdge), Vector3.down, distToGround + 0.1F, ignoreMask))
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
