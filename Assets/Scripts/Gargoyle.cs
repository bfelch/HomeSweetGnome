using UnityEngine;
using System.Collections;

public class Gargoyle : MonoBehaviour 
{
	public float rotateSpeed = 0.2f; //How fast the spotlight rotates
	private float oldTime = 0.0f;
	public float switchTime = 8.0f; //How long before the spotlight switches directions
    public float screechTime = 2.0f; //How long the screech lasts
	private bool lookRight = false; //Direction of the spotlight
	private GameObject player; //Player game object
	private bool screeching = false; //Is the gargoyle screeching?

	private GameObject eyeLight;
	private cameraShake shakeScript;
	private Vector3 offset;
	private Transform playerTrans;
	private Transform eyeLightTrans;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		eyeLight = transform.Find("Spotlight").gameObject;
		shakeScript = player.GetComponentInChildren<cameraShake>();

		//Cache the transform properties
		playerTrans = player.transform;
		eyeLightTrans = eyeLight.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
        //Gargoyle is not screeching; look around
        if (!screeching)
        {
            LookAround();
        }
        //Gargoyle is screeching; screech
        else
        {
            Screech();
        }
	}

    //Looks back and forth
	void LookAround()
	{
		if(Time.time > oldTime + switchTime)
		{
			oldTime = Time.time;
			lookRight = !lookRight;
		}

		if(lookRight == true)
		{
			transform.Rotate(Vector3.up * rotateSpeed);
		}
		else
		{
			transform.Rotate(Vector3.down * rotateSpeed);
		}
	}

    //Screech at the player
	void Screech()
	{
		eyeLightTrans.rotation = Quaternion.Lerp(eyeLightTrans.rotation, Quaternion.LookRotation(offset), Time.deltaTime);

		//Stop the player's movement and camera
        player.GetComponent<CharacterMotor>().enabled = false;
        player.GetComponent<MouseLook>().enabled = false;

		//Blur the player's camera view
		player.GetComponentInChildren<BlurEffect>().enabled = true;

        //Decreases the player's energy
        player.GetComponent<Player>().sanity -= .05f;

        //Enable the player's movement and camera
        if (Time.time > oldTime + screechTime)
        {
            oldTime = Time.time;

			//Enable the player controls
            player.GetComponent<CharacterMotor>().enabled = true;
            player.GetComponent<MouseLook>().enabled = true;

			//Remove the blur
			player.GetComponentInChildren<BlurEffect> ().enabled = false;

            screeching = false;
        }
	}

    //Has an object entered the spotlight?
	void OnTriggerEnter(Collider other)
	{
        //Is the object the player?
		if(other.gameObject == player)
		{   
			//Find the offset between the spotlight and player
			offset = playerTrans.position - eyeLightTrans.position;

			//Trigger the cameraShake
			shakeScript.CameraShake();

            //Start screeching
            screeching = true;
            oldTime = Time.time;
		}
	}
}
