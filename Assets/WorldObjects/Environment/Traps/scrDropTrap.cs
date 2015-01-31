using UnityEngine;
using System.Collections;

/* This script can be attached to any object to turn it into a drop trap.
 * The object must include all other objects needed to make the trap work. (Switch, Marker, Tiggers)
 * The drop trap will fall and crush gnomes.
 */
public class scrDropTrap : MonoBehaviour 
{
    public bool dropping = false; //Is the drop trap falling?
	public bool dropped = false; //Has the drop trap been activated yet?

	//Calls when drop trap interaction button is pressed
	public void Drop()
	{
		dropped = true; //Trap has been dropped
		dropping = true; //Trap is dropping

		//Allows object to fall
		transform.parent.Find("DropTrap").gameObject.GetComponent<Rigidbody>().isKinematic = false;
		transform.parent.Find("DropTrap").gameObject.GetComponent<Rigidbody>().useGravity = true;
	}
	
	//Update is called once per frame
	void Update() 
    {
		//Only enable the trigger that crushes gnomes while it is dropping
        if(dropping)
        {
			transform.parent.Find("DropTrap/Trigger").gameObject.collider.enabled = true;
        }
	}

	//Detection between triggers
    void OnTriggerEnter(Collider other)
    {
		//Did the drop trap collide with the drop marker?
		if(other.tag == "DropFloor")
		{
	        dropping = false; //No longer dropping
			transform.parent.Find("DropTrap/Trigger").gameObject.collider.enabled = false; //Crushing trigger disabled while not dropping

			//Temporary destory (Shatter? Fade away? Broken Model?)
			Destroy(this.gameObject);
		}
    }
}
