using UnityEngine;
using System.Collections;

public class Gargoyle : MonoBehaviour 
{
	public float rotateSpeed = 3.0F; //How fast the spotlight rotates
	private float switchTimer = 0.0F;
	public float switchTime = 8.0f; //How long before the spotlight switches directions
	private bool lookRight = false; //Direction of the spotlight
	private GameObject player; //Player game object
	private GameObject playerTop; //Player game object
	private GameObject playerBottom; //Player game object
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
	private bool checkLos = false; //Player is in light; check line of sight

	private Vector3 fwdTop; //Direction to top part of player
	private float distanceTop; //Distance to top part of player
	private Vector3 fwdBottom;  //Direction to bottom part of player
	private float distanceBottom; //Distance to bottom part of player
	
	private GameObject eyeLight; //To change eye light color
	private cameraShake shakeScript; //Script to shake camera

	public GameObject eye1;
	public GameObject eye2;
	public GameObject mouth;
	
	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerTop = GameObject.Find ("Top");
		playerBottom = GameObject.Find ("Bottom");
		eyeLight = transform.Find("Spotlight").gameObject;
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
		if(checkLos)
		{
			//Enemy layer mask
			int enemyLayer = 9;
			int invisibleLayer = 10;
			int ignoreMask = 1 << enemyLayer | 1 << invisibleLayer;
			
			//Invert bitmask to only ignore this layer
			ignoreMask = ~ignoreMask;

			Debug.Log(playerTop);
			fwdTop = (playerTop.transform.position - transform.position).normalized;
			distanceTop = Vector3.Distance(playerTop.transform.position, transform.position);

			fwdBottom = (playerBottom.transform.position - transform.position).normalized;
			distanceBottom = Vector3.Distance(playerBottom.transform.position, transform.position);

			RaycastHit hit;
			Debug.DrawRay(transform.position, fwdTop * (distanceTop + 0.1F), Color.red);
			Debug.DrawRay(transform.position, fwdBottom * (distanceBottom + 0.1F), Color.red);

			//Double raycast
			if(Physics.Raycast(transform.position, fwdTop, out hit, distanceTop + 0.1F, ignoreMask)
			   || Physics.Raycast(transform.position, fwdBottom, out hit, distanceBottom + 0.1F, ignoreMask))
			//If one true, screech
			{
				activeTarget = hit.collider.gameObject; //Store item being looked at

				//Is the object the player?
				if(activeTarget.tag == "Player")
				{
					//Grab players transform
					target = player.transform;
					
					//Store starting rotation
					initialRot = transform.rotation;
					
					//Trigger the camera shake
					shakeScript.StartShake();

					//On screen damage warning
					//player.GetComponent<GUIDamage>().enterCollider = true;
					
					//Start screeching
					screeching = true;

					//Blur the player's camera view
					player.GetComponentInChildren<BlurEffect>().enabled = true;

					//Adjust light values
					eyeLight.GetComponent<Light>().range = 30;
					eyeLight.GetComponent<Light>().spotAngle = 40;
					
					//Change color red
					eyeLight.light.color = Color.red;
					eye1.light.color = Color.red;
					eye2.light.color = Color.red;
					mouth.light.color = Color.red;
					
					//Play screech sound
					audio.PlayOneShot(screechSound);
				}
			}
		}

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
			transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
		}
		else
		{
			transform.Rotate(Vector3.down * rotateSpeed * Time.deltaTime);
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
				Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
			}
			else
			{
				// Just lookat
				transform.LookAt(target);
			}
		}

		//Decreases the player's energy
		player.GetComponent<Player>().sanity -= 0.05f;
		
		//Decrease the players speed
		player.GetComponent<Player>().charMotor.movement.maxForwardSpeed = 3.0F;
		player.GetComponent<Player>().charMotor.movement.maxSidewaysSpeed = 3.0F;
		player.GetComponent<Player>().charMotor.movement.maxBackwardsSpeed = 3.0F;
		
		//Enemy layer mask
		int enemyLayer = 9;
		int invisibleLayer = 10;
		int ignoreMask = 1 << enemyLayer | 1 << invisibleLayer;
		
		//Invert bitmask to only ignore this layer
		ignoreMask = ~ignoreMask;		
		
		fwdTop = (playerTop.transform.position - transform.position).normalized;
		distanceTop = Vector3.Distance(playerTop.transform.position, transform.position);
		
		fwdBottom = (playerBottom.transform.position - transform.position).normalized;
		distanceBottom = Vector3.Distance(playerBottom.transform.position, transform.position);
		
		RaycastHit hit;
		Debug.DrawRay(transform.position, fwdTop * (distanceTop + 0.1F), Color.red);
		Debug.DrawRay(transform.position, fwdBottom * (distanceBottom + 0.1F), Color.red);
		
		//Double raycast
		if(Physics.Raycast(transform.position, fwdTop, out hit, distanceTop + 0.1F, ignoreMask)
		   || Physics.Raycast(transform.position, fwdBottom, out hit, distanceBottom + 0.1F, ignoreMask))
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
				
				if(timeLost >= 0.5F)
				{
					//Player not in line of sight
					targetLost = true;
				}
			}
		}
		
		//Is the player far enough away or behind another object?
		if(Vector3.Distance(target.position, transform.position) >= 20.0F || targetLost)
		{
			//Fix player speed
			player.GetComponent<Player>().charMotor.movement.maxForwardSpeed = 6.0F;
			player.GetComponent<Player>().charMotor.movement.maxSidewaysSpeed = 6.0F;
			player.GetComponent<Player>().charMotor.movement.maxBackwardsSpeed = 6.0F;
			
			//Remove the blur
			player.GetComponentInChildren<BlurEffect>().enabled = false;

			//On screen damage warning
			player.GetComponent<GUIDamage>().enterCollider = false;

			//Trigger the cameraShake
			shakeScript.EndShake();

			//Adjust light values
			eyeLight.GetComponent<Light>().range = 20;
			eyeLight.GetComponent<Light>().spotAngle = 25;
			
			//Change gargoyle light color
			eyeLight.light.color = Color.white;
			eye1.light.color = Color.white;
			eye2.light.color = Color.white;
			mouth.light.color = Color.white;
			
			//No longer alerted, adjust to normal scouting rotation
			timeLost = 0.0F;
			targetLost = false;
			screeching = false;
			adjusting = true;
		}
	}
	
	void Adjust()
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, initialRot, Time.deltaTime * 2);
		
		if(transform.rotation == initialRot)
		{
			adjusting = false;
		}
	}
	
	//Has an object entered the spotlight?
	void OnTriggerEnter(Collider other)
	{
		//Is the object the player?
		if(other.gameObject == player)
		{   
			checkLos = true;
		}
	}

	//Has an object exited the spotlight?
	void OnTriggerExit(Collider other)
	{
		//Is the object the player?
		if(other.gameObject == player)
		{  
			checkLos = false;
		}
	}
}
