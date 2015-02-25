using UnityEngine;
using System.Collections;

public class scrDarkness : MonoBehaviour 
{
	bool eventStarted = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void DarknessEvent()
	{
		if(!eventStarted)
		{
			eventStarted = true;
			transform.GetComponent<MeshCollider>().enabled = true;
			transform.Find("DarknessTrigger").GetComponent<SphereCollider>().enabled = false;
		}
	}
}
