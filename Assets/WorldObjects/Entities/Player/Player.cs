﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    //player health
    public float sanity;
    public float maxSanity;
    public float minSanity;
    float healthBarLength;
    //time until can sprint
    private float restTime;
    private float maxRestTime;
    //time player can sprint
    public float sprintTime;
    public float maxSprintTime;
    public bool playerSlept;
	public bool playerFell;
    public bool playerEscaped;
    public bool experimentComplete;
    public GUIText deathTextSleep;
	public GUIText deathTextFall;
    public GUIText winTextEscaped;
    public GUIText winTextExperiment;
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
    public CharacterController controller;
    private float fadeIn = 0;
    private float pauseFadeTime = 4;
    private bool pauseFade = false;
    private bool switchFade = false;

	public float pushPower = 1.0F;

	public GameObject activeTarget; //The item being looked at

    //if player just landed or has been on ground
    private bool landed;

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

        winTextEscaped = GameObject.Find("WinTextEscape").guiText;
        winTextEscaped.enabled = false;

        winTextExperiment = GameObject.Find("WinTextExperiment").guiText;
        winTextExperiment.enabled = false;

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

        //temporary close game screen
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            GameObject.Find("Save").GetComponent<SaveLoad>().Save();
            Application.Quit();
        }

        if (!this.gameObject.animation.IsPlaying("OpeningCut")) {
            Sanity();
            Walk();
            Sprint();
            Crouch();
            Push();
        }
    }

    //Note: Bug: enemies will not pathfind close enough to you to actually register the collision.
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy") && other.gameObject.name != "GnomeShed")
        {
            sanity -= 0.2f;
        }
    }

    void Walk() {
        if (!this.gameObject.animation.IsPlaying("Landing")) {
            if (!Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))) {
                //play walk animation
                this.gameObject.animation.Play("Walk");
            }
        }

        if (controller.isGrounded) {
            if (!landed) {
                landed = true;
                this.gameObject.animation.Play("Landing");
            }
        } else {
            landed = false;
        }
    }

    void Sprint()
    {
        if (sprintTime > 0)
        {
            //listent for shift press
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                //make player faster
                charMotor.movement.maxForwardSpeed = 12;
                sprintTime -= Time.deltaTime;
                //play sprint animation
                if (!this.gameObject.animation.IsPlaying("Landing")) {
                    this.gameObject.animation.Play("Sprint");
                }
            } else if (sprintTime <= maxSprintTime) {
                //recharge sprint time
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
        //listen for button press if not already crouching
        if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)) && !crouching){
            crouching = true;
            //alter speed
            charMotor.movement.maxForwardSpeed = 3;
            //alter size
            box.size = new Vector3(1, yHeight / 2, 1);
            controller.height = yHeight / 2;
            //alter camera position
            Vector3 pos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(pos.x, pos.y - .5f, pos.z);
        }
        //listen for key up
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftCommand))
        {
            crouching = false;
            //alter size
            box.size = new Vector3(1, yHeight, 1);
            controller.height = yHeight;
            //alter camera position
            Vector3 pos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(pos.x, pos.y + .5f, pos.z);
            //move player up to prevent falling through items
            pos = this.transform.position;
            this.transform.position = new Vector3(pos.x, pos.y + yHeight / 3, pos.z);
        }
    }

	void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		//Only push gargoyles
		if(hit.gameObject.tag == "Gargoyle")
		{
			//Get the rigidbody
			Rigidbody body = hit.collider.attachedRigidbody;
			
			//Make sure there is a body and it is kinematic
			if (body == null || body.isKinematic)
			{
				return;
			}
			
			//Don't want to push down
			if (hit.moveDirection.y < -0.3F)
			{
				return;
			}
			
			//Get push direction opposite of the player
			Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
			//Push
			body.velocity = pushDir * pushPower;
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

    public void Escape()
    {
        playerEscaped = true;

        StartCoroutine(WaitToReload(5.0F));
    }

    public void Experiment()
    {
        experimentComplete = true;

        StartCoroutine(WaitToReload(5.0F));
    }


    void OnGUI()
    {
        if (experimentComplete)
        {

            //create a new color with the changed alpha value
            Color changing = new Color(winTextExperiment.color.r, winTextExperiment.color.g, winTextExperiment.color.b, fadeIn);
            winTextExperiment.enabled = true;

            //set the new color
            winTextExperiment.color = changing;
            //update the alpha value
            fadeIn += .1f * Time.deltaTime;
        }

        if(playerEscaped)
        {

            //create a new color with the changed alpha value
            Color changing = new Color(winTextEscaped.color.r, winTextEscaped.color.g, winTextEscaped.color.b, fadeIn);
            winTextEscaped.enabled = true;

            //set the new color
            winTextEscaped.color = changing;
            //update the alpha value
            fadeIn += .1f * Time.deltaTime;
        }

        if (playerSlept)
        {
            //create a new color with the changed alpha value
            Color changing = new Color(deathTextSleep.color.r, deathTextSleep.color.g, deathTextSleep.color.b, fadeIn);
            deathTextSleep.enabled = true;

            //set the new color
            deathTextSleep.color = changing;
            //update the alpha value
            fadeIn += .1f*Time.deltaTime;
        }

		if(playerFell)
		{
            //create a new color with the changed alpha value
            Color changing = new Color(deathTextFall.color.r, deathTextFall.color.g, deathTextFall.color.b, fadeIn);
            deathTextFall.enabled = true;

            //set the new color
            deathTextFall.color = changing;
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
        if(other.name == "AtticStairs")
        {
            this.GetComponent<CharacterController>().slopeLimit = 85;
        }
	}

    void OnTriggerExit(Collider other)
    {
        if (this.GetComponent<CharacterController>().slopeLimit == 85)
        {
            this.GetComponent<CharacterController>().slopeLimit = 45;
        }
    }

	IEnumerator WaitToReload(float waitTime)
	{
		//Wait before loading the main menu
		yield return new WaitForSeconds (waitTime);

		//Load the main menu
		Application.LoadLevel ("MainMenu");
	}

    public void flashFade()
    {
        if(pauseFade)
        {
            pauseFadeTime -= Time.deltaTime;
            if(pauseFadeTime < 0)
            {
                pauseFade = false;
                pauseFadeTime = 4;
            }

        }
        
        else if (switchFade)
        {
            Color changing = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeIn);
            //set the new color
            GUI.color = changing;
            //update the alpha value
            fadeIn += 1f * Time.deltaTime;
            if (fadeIn > 1)
            {
                switchFade = false;
                pauseFade = true;
            }
        }
        else if (!switchFade)
        {
            Color changing = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeIn);
            //set the new color
            GUI.color = changing;
            //update the alpha value
            fadeIn -= 1f * Time.deltaTime;

            if (fadeIn < 0)
                switchFade = true;
        }
    }
}