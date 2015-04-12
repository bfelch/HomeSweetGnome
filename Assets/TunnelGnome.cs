using UnityEngine;
using System.Collections;

public class TunnelGnome : MonoBehaviour 
{
	private Animation walkAnim; //The animation component

	// Use this for initialization
	void Start () 
	{
		walkAnim = GetComponent<Animation>();
		walkAnim.animation["GnomeWalk"].speed = 0.2F; //Play animation fowards
		walkAnim.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
