﻿using UnityEngine;
using System.Collections;

public class fogSphere : MonoBehaviour 
{
	public Transform target;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(target.position.x-15,
		      target.position.y+36, target.position.z+15);
	}
}
