using UnityEngine;
using System.Collections;

public class scrDirtTrap : MonoBehaviour 
{
	public GameObject dirtEffect; //Dig particle effect
	public GameObject dirtPile1; //Dirt pile prefab
	public GameObject dirtPile2; //Dirt pile prefab
	public GameObject dirtPile3; //Dirt pile prefab

	private int timesToDig = 3; //Times to dig a pile before it's a hole
	private int count = 1;

	//Calls when dirt is dug
	public void Dig()
	{
		//Emit particle effect at dirt location
		Instantiate(dirtEffect, new Vector3(transform.position.x, transform.position.y - 0.2F, transform.position.z), dirtEffect.transform.rotation);

		//Play dig sound
		//SoundController.PlayClipAt(GameObject.Find("Player").GetComponent<Player>().digSound.clip, this.gameObject.transform.position);

		timesToDig--; //Dirt has to be dug three times

		//Random position inside a circle of size 3
		Vector2 newPosition = Random.insideUnitCircle * 2;

		//Place unique dirt piles
		if(count == 1)
		{
			Instantiate(dirtPile1, new Vector3 (transform.position.x + 3.0F, transform.position.y, transform.position.z), dirtPile1.transform.rotation);
		}
		else if(count == 2)
		{
			Instantiate(dirtPile2, new Vector3 (transform.position.x + 3.0F, transform.position.y, transform.position.z), dirtPile2.transform.rotation);
		}
		else
		{
			Instantiate(dirtPile3, new Vector3 (transform.position.x + 3.0F, transform.position.y, transform.position.z), dirtPile3.transform.rotation);
		}

		count++;

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
