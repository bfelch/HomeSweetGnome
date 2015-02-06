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
		//distToEdge = collider.bounds.extents.x;
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
				BreakApart(true);
			}
		}
	}

	//Shatters the gargoyle when pushed and spawns the gargoyle head pickup
	public void BreakApart(bool shatter)
	{
		//Emit shatter effect
        if (shatter)
        {
            Instantiate(shatterEffect, new Vector3(transform.position.x, transform.position.y - 0.8F, transform.position.z), shatterEffect.transform.rotation);
        }
            Debug.Log("breaking");
		//Debug.Log (transform.parent.gameObject.name);

		//Get the gargoyle head game object
		GameObject gargoyleHead = transform.parent.Find("Head").gameObject;
		GameObject gargoyleHeadItem = transform.parent.Find("GargoyleHead").gameObject;
		
		//Remove gargoyle script component
        DestroyImmediate(gargoyleHead.GetComponent<Gargoyle>());
		
		//Remove children
		foreach(Transform child in gargoyleHead.transform)
		{
			if(child.gameObject.name != "GargoyleHead")
			{
                DestroyImmediate(child.gameObject);
			}
		}
		
		//Remove audio source component
        DestroyImmediate(gargoyleHead.GetComponent<AudioSource>());
		
		//Apply physics to the head
		gargoyleHeadItem.GetComponent<Rigidbody>().isKinematic = false;
		gargoyleHeadItem.GetComponent<Rigidbody>().useGravity = true;
		
		//Enable useable script
		gargoyleHeadItem.GetComponent<Item>().enabled = true;;
		
		//Change tag
		gargoyleHeadItem.tag = "PickUp";
		
		//Move head to item list
		gargoyleHeadItem.transform.parent = GameObject.Find("Items").transform;
		
		//Delete gargoyle
		DestroyImmediate(transform.parent.gameObject);
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

		//Debug.Log (transform);
		Debug.DrawRay (transform.position, Vector3.down * (distToGround + 0.1F), Color.cyan);
		//Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z + distToEdge), Vector3.down * (distToGround + 0.1F), Color.red);
		//Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z - distToEdge), Vector3.down * (distToGround + 0.1F), Color.red);

		if(Physics.Raycast(transform.position, Vector3.down, distToGround + 2.1F, ignoreMask)
		   || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + distToEdge), Vector3.down, distToGround + 2.1F, ignoreMask)
		   || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - distToEdge), Vector3.down, distToGround + 2.1F, ignoreMask))
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
