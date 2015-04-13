using UnityEngine;
using System.Collections;

/* This script can be attached to any object to turn it into a drop trap.
 * The object must include all other objects needed to make the trap work. (Switch, Marker, Tiggers)
 * The drop trap will fall and crush gnomes.
 */
public class scrDropTrap : MonoBehaviour 
{
    public static bool dropping = false; //Is the drop trap falling?
	public bool dropped = false; //Has the drop trap been activated yet?

	//Gnome eye object
	public GameObject gnomeEye;

	public AudioClip chandCrash;

	//Calls when drop trap interaction button is pressed
	public void Drop()
	{
		//Play Spark Sound
		GameObject.Find("ChandLight").GetComponent<AudioSource>().Play();
		GameObject.Find("Sparks").particleSystem.Emit(40);

		StartCoroutine(DelayedDrop(0.6F));
	}

	public IEnumerator DelayedDrop(float waitTime)
	{
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);
		
		GameObject.Find("Player").GetComponent<LookAway>().DropAnimationEventStart();
		
		dropped = true; //Trap has been dropped
		dropping = true; //Trap is dropping
		
		transform.parent.Find("DropTrap/Trigger").gameObject.collider.enabled = true;
		
		//Allows object to fall
		transform.parent.Find("DropTrap").gameObject.GetComponent<Rigidbody>().isKinematic = false;
		transform.parent.Find("DropTrap").gameObject.GetComponent<Rigidbody>().useGravity = true;
	}

	//Detection between triggers
    void OnTriggerEnter(Collider other)
    {
		//Did the drop trap collide with the drop marker?
		if(other.tag == "DropFloor")
		{
			SoundController.PlayClipAt(chandCrash, new Vector3(25.31F, 37.34F, -34.14F));

	        dropping = false; //No longer dropping
			transform.parent.Find("DropTrap/Trigger").gameObject.collider.enabled = false; //Crushing trigger disabled while not dropping

			//Remove gnome circle
			GameObject.Find("gnomeTrapCircle").renderer.enabled = false;
			
			//Spawn the gnome eye
			gnomeEye.transform.position = new Vector3(25.31F, 37.34F + 0.4F, -34.14F);

			GameObject.Find("Player").GetComponent<LookAway>().DropAnimationEventEnd();

			//Temporary destory (Shatter? Fade away? Broken Model?)
			Destroy(this.gameObject);
		}
    }
}
