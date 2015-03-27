using UnityEngine;
using System.Collections;

public class groundCheck : MonoBehaviour 
{
	private float distToGround; //Center of gargoyle to floor
	private bool falling = false;
	public GameObject gargoyleHead;
	public GameObject shatterEffect; //Shatter Effect Prefab

	// Use this for initialization
	void Start () 
	{
		//Get distance to ground
		distToGround = collider.bounds.extents.y;
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
	public void BreakApart()
	{
		//Emit shatter effect
        Instantiate(shatterEffect, new Vector3(transform.position.x, transform.position.y - 0.8F, transform.position.z), shatterEffect.transform.rotation);

		//Spawn the gargoyle head
		gargoyleHead.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2F, transform.position.z);
		
		//Delete gargoyle
		DestroyImmediate(transform.parent.gameObject);
	}

	//Checks if gargyole is on the ground
	bool IsGrounded() 
	{
		//Enemy layer mask
		int playerLayer = 8;
		int enemyLayer = 9;
		int invisibleLayer = 10;
		int ignoreMask = 1 << playerLayer | 1 << enemyLayer | 1 << invisibleLayer;
		
		//Invert bitmask to only ignore this layer
		ignoreMask = ~ignoreMask;

		//Debug.Log (transform);
		Debug.DrawRay (new Vector3(transform.position.x, transform.position.y, transform.position.z- 1.2F), Vector3.down * (distToGround + 2.1F), Color.cyan);
		//Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z + distToEdge), Vector3.down * (distToGround + 0.1F), Color.red);
		//Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z - distToEdge), Vector3.down * (distToGround + 0.1F), Color.red);

		if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.2F), Vector3.down, distToGround + 2.1F, ignoreMask))
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
