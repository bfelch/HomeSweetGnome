using UnityEngine;
using System.Collections;

public class scrDirtTrap : MonoBehaviour 
{
	public GameObject dirtEffect; //Dig particle effect
	public GameObject dirtPile; //Dirt pile prefab
	private int timesToDig = 3; //Times to dig a pile before it's a hole

	//Calls when dirt is dug
	public void Dig()
	{
		//Emit particle effect at dirt location
		Instantiate(dirtEffect, new Vector3(transform.position.x, transform.position.y - 0.2F, transform.position.z), dirtEffect.transform.rotation);

		//Play dig sound
		SoundController.PlayClipAt(GameObject.Find("Player").GetComponent<Player>().digSound.clip, this.gameObject.transform.position);

		timesToDig--; //Dirt has to be dug three times

		//Spawn dirt piles around hole
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

			//Make sure the dirt pile doesn't spawn over the hole
			if(newPosition.y > 0)
			{
				newPosition.y = newPosition.y + 2;
			}
			else
			{
				newPosition.y = newPosition.y - 2;
			}

			//Spawn dirt pile prefab
			Instantiate(dirtPile, new Vector3 (transform.position.x + newPosition.x, transform.position.y + 0.3F, transform.position.z + newPosition.y), dirtPile.transform.rotation);
		}

		//Hole is dug
		if(timesToDig <= 0)
		{
			//Enable the trap trigger
			transform.parent.Find("DirtTrap").GetComponent<SphereCollider>().enabled = true;

			//Destroy Dirt object
			Destroy(this.gameObject);
		}
	}
}
