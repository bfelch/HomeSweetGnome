using UnityEngine;
using System.Collections;

public class Gargoyle : MonoBehaviour 
{
	public float rotateSpeed = 3.0F; //How fast the spotlight rotates
	private float switchTimer = 0.0F;
	public float switchTime = 8.0f; //How long before the spotlight switches directions
	private bool lookRight = false; //Direction of the spotlight
	private GameObject player; //Player game object
	private bool screeching = false; //Is the gargoyle screeching?
	private bool adjusting = false; //Gargoyle is adjusting
	
	public AudioClip screechSound; //Sreeching sound
	
	public Transform target; //Target to look at
	private float damping = 6.0F; //How fast to look at target
	private bool smooth = true;
	private Quaternion initialRot; //For gargoyle adjust
	private GameObject activeTarget; //The object being looked at
	private bool targetLost = false; //Is the play within view?
	private float timeLost = 0.0F; //Counter to break line of sight
	
	private GameObject eyeLight; //To change eye light color
	private cameraShake shakeScript; //Script to shake camera
	
	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		eyeLight = transform.Find("Head/Spotlight").gameObject;
		shakeScript = player.GetComponentInChildren<cameraShake>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Gargoyle is not screeching; look around
		if (!screeching && !adjusting)
		{
			LookAround();
		}
		//Gargoyle is screeching; screech
		else if (screeching)
		{
			Screech();
		}
		else
		{
			Adjust();
		}
	}
	
	//Looks back and forth
	void LookAround()
	{
		if(switchTimer > switchTime)
		{
			switchTimer = 0;
			lookRight = !lookRight;
		}
		else
		{
			switchTimer += Time.deltaTime;
		}
		
		if(lookRight == true)
		{
			transform.Find("Head").transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
		}
		else
		{
			transform.Find("Head").transform.Rotate(Vector3.down * rotateSpeed * Time.deltaTime);
		}
	}
	
	//Screech at the player
	void Screech()
	{	
		if(target) 
		{
			if(smooth)
			{
				// Look at and dampen the rotation
				Quaternion rotation = Quaternion.LookRotation(target.position - transform.Find("Head").transform.position);
				transform.Find("Head").transform.rotation = Quaternion.Slerp(transform.Find("Head").transform.rotation, rotation, Time.deltaTime * damping);
			}
			else
			{
				// Just lookat
				transform.Find("Head").transform.LookAt(target);
			}
		}
		
		//Blur the player's camera view
		//player.GetComponentInChildren<BlurEffect>().enabled = true;
		
		//Decreases the player's energy
		player.GetComponent<Player>().sanity -= 0.1f;
		
		//Decrease the players speed
		player.GetComponent<Player>().charMotor.movement.maxForwardSpeed = 3;
		
		//Enemy layer mask
		int enemyLayer = 9;
		int enemyMask = 1 << enemyLayer;
		
		//Invert bitmask to only ignore this layer
		enemyMask = ~enemyMask;		
		
		RaycastHit hit;
		Debug.DrawRay(transform.position, transform.forward * 20, Color.white);
		
		if(Physics.Raycast(transform.Find("Head").transform.position, transform.Find("Head").transform.forward, out hit, 20, enemyMask))
		{
			activeTarget = hit.collider.gameObject; //Store item being looked at
			
			//Is the object the player?
			if(activeTarget.tag == "Player")
			{
				//Reset time lost
				timeLost = 0.0F;
			}
			else
			{
				//Increase time lost
				timeLost += 1.0F * Time.deltaTime;
				
				if(timeLost >= 1.0F)
				{
					//Player not in line of sight
					targetLost = true;
				}
			}
		}
		
		//Is the player far enough away or behind another object?
		if(Vector3.Distance(target.position, transform.Find("Head").transform.position) >= 20.0F || targetLost)
		{
			//Fix player speed
			player.GetComponent<Player>().charMotor.movement.maxForwardSpeed = 6;
			
			//Remove the blur
			//player.GetComponentInChildren<BlurEffect> ().enabled = false;
			
			//Change gargoyle light color
			eyeLight.light.color = Color.yellow;
			
			//No longer alerted, adjust to normal scouting rotation
			timeLost = 0.0F;
			targetLost = false;
			screeching = false;
			adjusting = true;
		}
	}
	
	void Adjust()
	{
		transform.Find("Head").transform.rotation = Quaternion.Lerp(transform.Find("Head").transform.rotation, initialRot, Time.deltaTime * 2);
		
		if(transform.Find("Head").transform.rotation == initialRot)
		{
			adjusting = false;
		}
	}
	
	//Has an object entered the spotlight?
	void OnTriggerEnter(Collider other)
	{
		//Is the object the player?
		if(other.gameObject == player && !adjusting && !screeching)
		{   
			//Grab players transform
			target = other.gameObject.transform;
			
			//Store starting rotation
			initialRot = transform.Find("Head").transform.rotation;
			
			//Trigger the cameraShake
			shakeScript.CameraShake();
			
			//Start screeching
			screeching = true;
			
			//Change color red
			eyeLight.light.color = Color.red;
			
			//Play screech sound
			audio.PlayOneShot(screechSound);
		}
	}
}
