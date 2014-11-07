﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float sanity;
    public float maxSanity;
    public float minSanity;
    float healthBarLength;
    private float restTime;
    private float maxRestTime;
    public float sprintTime;
    public float maxSprintTime;
    public bool playerSlept;
	public bool playerFell;
    public GUIText deathTextSleep;
	public GUIText deathTextFall;
    private Animation blinkBottom;
    private Animation blinkTop;
    public CharacterMotor charMotor;

    public AudioSource audio1;
    public AudioSource audio2;
    public bool breathe = true; //To check whether to play the heavyBreathing sound

    private bool crouching;
	private bool readyToPush;
    private float yHeight;
    public BoxCollider box;
    public CharacterController controllerBox;
    private float fadeIn = 0;

	public GameObject activeTarget; //The item being looked at

    // Use this for initialization
    void Start()
    {
        //Sanity instead of health. As they touch you, sanity falls. It also slowly falls over time. Must find items to raise it.
        //The lower it gets, the more hazards are in the level.
        sanity = 100;
        maxSanity = 100;
        minSanity = 0;
        restTime = 0;
        maxRestTime = 3.7f;
        sprintTime = maxSprintTime = 5f;
        playerSlept = false;
		playerFell = false;
		readyToPush = true;

        yHeight = box.size.y;

        deathTextSleep = GameObject.Find("DeathTextSleep").guiText;
        deathTextSleep.enabled = false;

		deathTextFall = GameObject.Find("DeathTextFall").guiText;
		deathTextFall.enabled = false;

        AudioSource[] aSources = GetComponentsInChildren<AudioSource>(); //Grab all the audio sources on this object
        Debug.Log(aSources.Length);
        audio1 = aSources[0];
        audio2 = aSources[1];

        if(PlayerPrefs.GetInt("LoadGame") == 1)
        {
            GameObject.Find("Save").GetComponent<SaveLoad>().Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Save the game then go to the main menu
        if (Input.GetKey("m"))
        {
            GameObject.Find("Save").GetComponent<SaveLoad>().Save();
            Application.LoadLevel("MainMenu");
        }
        //temporary close game screen
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            GameObject.Find("Save").GetComponent<SaveLoad>().Save();
            Application.Quit();
        }

        Sanity();
        Sprint();
        Crouch();
		Push();
    }

    //Note: Bug: enemies will not pathfind close enough to you to actually register the collision.
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy") && other.gameObject.name != "GnomeShed")
        {
            sanity -= 0.2f;
        }
    }

    void Sprint()
    {
        if (sprintTime > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                charMotor.movement.maxForwardSpeed = 12;
                sprintTime -= Time.deltaTime;
                this.gameObject.animation.Play("Sprint");
            }
            else if (sprintTime <= maxSprintTime)
            {
                charMotor.movement.maxForwardSpeed = 6;
                sprintTime += Time.deltaTime;
            }
        }
        else
        {
            if (breathe)
            {
                breathe = false;
                audio2.Play();
            }

            charMotor.movement.maxForwardSpeed = 6;
            restTime += Time.deltaTime;
            this.gameObject.animation.Stop("Sprint");
            if (restTime >= maxRestTime)
            {
                sprintTime = 5.0F;
                restTime = 0;
                breathe = true;
            }
        }
    }

    void Crouch()
    {
        if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)) && !crouching){
            crouching = true;
            charMotor.movement.maxForwardSpeed = 3;
            box.size = new Vector3(1, yHeight / 2, 1);
            controllerBox.height = yHeight / 2;
            Vector3 pos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(pos.x, pos.y - .5f, pos.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftCommand))
        {
            crouching = false;
            box.size = new Vector3(1, yHeight, 1);
            controllerBox.height = yHeight;
            Vector3 pos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(pos.x, pos.y + .5f, pos.z);
            pos = this.transform.position;
            this.transform.position = new Vector3(pos.x, pos.y + yHeight / 3, pos.z);
        }
    }

	IEnumerator PushTimer(float waitTime, GameObject target)
	{
		Debug.Log("Push Timer Started");
		
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);

		target.GetComponent<Gnome> ().readyToSpawn = true;
		readyToPush = true;
	}

	void Push()
	{
		if (Input.GetMouseButtonDown(0) && readyToPush)
		{
			Transform cam = Camera.main.transform;

			//Player layer mask
			int playerLayer = 8;
			int playerMask = 1 << playerLayer;
			
			//Invert bitmask to only ignore this layer
			playerMask = ~playerMask;
			
			RaycastHit hit;
			
			if (Physics.Raycast(cam.position, cam.forward, out hit, 5, playerMask))
			{
				activeTarget = hit.collider.gameObject; //Store item being looked at

				if(activeTarget.tag == "Enemy" && activeTarget.GetComponent<Gnome>().gnomeLevel == 1)
				{
					Debug.Log (activeTarget.GetComponent<Gnome>().gnomeLevel);
					readyToPush = false;
					activeTarget.GetComponent<Gnome>().pushed = true;

					//Start push timer
					StartCoroutine(PushTimer(10.0F, activeTarget));

					//Disable NavMeshAgent
					activeTarget.GetComponent<NavMeshAgent>().enabled = false;
					
					//Make the gnome moveable
					activeTarget.rigidbody.isKinematic = false;
					activeTarget.rigidbody.useGravity = true;

					//Apply a force in the direction of the push
					activeTarget.rigidbody.AddForce((activeTarget.transform.position - this.transform.position).normalized * 6, ForceMode.Impulse);
				}
			}
		}
	}

    void Sanity()
    {
        sanity -= .002f;
        if (sanity < 0)
        {
            playerSlept = true;

			StartCoroutine(WaitToReload(5.0F));
        }

        if (sanity > maxSanity)
        {
            sanity = maxSanity;
        }
        else if(sanity < minSanity)
        {
            sanity = 0;
        }
    }

    void OnGUI()
    {
        if (playerSlept)
        {
            deathTextSleep.enabled = true;
            //find the GUI Text for death by sleep
            GUIText sleepDeath = GameObject.Find("DeathTextSleep").GetComponent<GUIText>();
            //create a new color with the changed alpha value
            Color changing = new Color(sleepDeath.color.r, sleepDeath.color.g, sleepDeath.color.b, fadeIn);
            //set the new color
            sleepDeath.color = changing;
            //update the alpha value
            fadeIn += .1f*Time.deltaTime;
        }

		if(playerFell)
		{
			deathTextFall.enabled = true;
            //find the GUI Text for death by fall
            GUIText fallDeath = GameObject.Find("DeathTextFall").GetComponent<GUIText>();
            //create a new color with the changed alpha value
            Color changing = new Color(fallDeath.color.r, fallDeath.color.g, fallDeath.color.b, fadeIn);
            //set the new color
            fallDeath.color = changing;
            //update the alpha value
            fadeIn += .1f * Time.deltaTime;
            
		}
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Fall")
		{
			playerFell = true;

			StartCoroutine(WaitToReload(5.0F));
		}
	}

	IEnumerator WaitToReload(float waitTime)
	{
		//Wait before loading the main menu
		yield return new WaitForSeconds (waitTime);

		//Load the main menu
		Application.LoadLevel ("MainMenu");
	}
}