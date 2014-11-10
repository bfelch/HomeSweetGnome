using UnityEngine;
using System.Collections;

public class dirtStuff : MonoBehaviour 
{
	public GameObject dirtEffect; //Dig particle effect
	public GameObject dirtPile; //Dirt pile prefab
	private int timesToDig = 3; //Times to dig a pile before it's a hole

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	//Calls when dirt is dug
	public void Dig()
	{
		//Emit particle effect at shatter location
		Instantiate(dirtEffect, new Vector3(transform.position.x, transform.position.y - 0.2F, transform.position.z), dirtEffect.transform.rotation);

		//Dirt has to be dug three times
		timesToDig--;

		//Spawn dirt piles
		for (int i = 0; i < 3; i++) 
		{
			//Random position inside a circle of size 3
			Vector2 newPosition = Random.insideUnitCircle * 3;

			//Make sure the dirt pile doesn't spawn over the hole
			if(newPosition.x > 0)
			{
				newPosition.x = newPosition.x + 2;
			}
			else
			{
				newPosition.x = newPosition.x - 2;
			}

			if(newPosition.y > 0)
			{
				newPosition.y = newPosition.y + 2;
			}
			else
			{
				newPosition.y = newPosition.y - 2;
			}

			//Spawn dirt pile
			Instantiate (dirtPile, new Vector3 (transform.position.x + newPosition.x, transform.position.y + 0.3F, transform.position.z + newPosition.y), dirtPile.transform.rotation);
		}

		//Hole is dug
		if(timesToDig <= 0)
		{
			//Destroy Dirt
			Destroy(this.gameObject);

			//Spawn the trap trigger
			GameObject.Find("DirtTrap").collider.enabled = true;
		}
	}
}
