using UnityEngine;
using System.Collections;

public class LookAway : MonoBehaviour 
{
	//player movement
	public CharacterMotor charMotor;
	//horizontal look
	public MouseLook mouseLook;
	//vertical look
	public MouseLook cameraLook;

	public PlayerMovement playerMove;

	public Transform target;
	private float damping = 8.0F;
	private Quaternion rotation;

	private bool dropAnimEventOn = false;
	
	void Update () 
	{
		if(dropAnimEventOn)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
		}
	}

	public void DropAnimationEventStart()
	{
		//Disable Controls
		//toggle movements, looking, cursor
		charMotor.enabled = false;
		mouseLook.enabled = false;
		cameraLook.enabled = true;
		playerMove.enabled = false;

		rotation = Quaternion.LookRotation(target.position - transform.position);
		rotation.x = 0.0F;
		rotation.z = 0.0F;

		//GameObject.Find("Player").GetComponent<Player>().gaspSound.Play();

		dropAnimEventOn = true;
	}

	public void DropAnimationEventEnd()
	{
		dropAnimEventOn = false;

		//Enable Controls
		//toggle movements, looking, cursor
		charMotor.enabled = true;
		mouseLook.enabled = true;
		cameraLook.enabled = true;
		playerMove.enabled = true;
	}
}