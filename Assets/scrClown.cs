using UnityEngine;
using System.Collections;

public class scrClown : MonoBehaviour 
{
	public Transform player;
	
	//Update is called once per frame
	void Update () 
	{
		//Looks at player
		transform.LookAt(player);
	}
}
