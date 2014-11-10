using UnityEngine;
using System.Collections;

public class particleDeleter : MonoBehaviour 
{
	private float timer = 0.0F; //Counter
	private float deleteTime = 5.0F; //Time to delete object
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Destroy this object after 5 seconds
		if(timer > deleteTime)
		{
			Destroy(this);
		}
		else
		{
			timer += Time.deltaTime;
		}
	}
}