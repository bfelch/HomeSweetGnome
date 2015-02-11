using UnityEngine;
using System.Collections;

/* Follows target GameObject */
public class scrFollow : MonoBehaviour 
{
	public Transform target; //Target object to follow
	public Vector3 offset; //Offset from target object
	
	//Update is called once per frame
	void Update() 
	{
		//Calculates new position based on target object and offset
		transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
	}
}