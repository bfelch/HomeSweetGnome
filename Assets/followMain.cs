using UnityEngine;
using System.Collections;

public class followMain : MonoBehaviour 
{
	public Transform target;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(target.position.x-21,
		                                 target.position.y+31, target.position.z+5);
	}
}