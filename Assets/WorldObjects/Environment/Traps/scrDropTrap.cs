using UnityEngine;
using System.Collections;

public class scrDropTrap : MonoBehaviour 
{
    public bool dropping = false;
	public bool dropped = false;

	// Use this for initialization
	void Start () 
    {
	
	}

	public void Drop()
	{
		dropping = true;
		transform.Find("DropTrap").GetComponent<Rigidbody>().isKinematic = false;
		transform.Find("DropTrap").GetComponent<Rigidbody>().useGravity = false;
	}
	
	//Update is called once per frame
	void Update() 
    {
        if(dropping)
        {
			transform.Find("DropTrap").collider.enabled = true;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        dropping = false;
		transform.Find("DropTrap").collider.enabled = false;
    }
}
