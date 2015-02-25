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
    private Animation blinkBottom;
    private Animation blinkTop;

    //public AudioSource bgMusic;
    public AudioSource breathingSound;
	public AudioSource keyPickUpSound;
	public AudioSource itemPickUpSound;
	public AudioSource itemDropSound;

	private bool readyToPush;
    private float fadeIn = 0;
    private float pauseFadeTime = 4;
    private bool pauseFade = false;
    private bool switchFade = false;

	public float pushPower = 1.0F;

	public GameObject activeTarget; //The item being looked at

    public CharacterMotor charMotor;

    //if player just landed or has been on ground
    private bool landed;
    public Font bark;

	//Step sounds
	private AudioSource grassStep;
	private AudioSource woodStep;
	private string floorType;

    // Use this for initialization
    void Start()
    {
        //Sanity instead of health. As they touch you, sanity falls. It also slowly falls over time. Must find items to raise it.
        //The lower it gets, the more hazards are in the level.
        sanity = 100;
        maxSanity = 100;
        minSanity = 0;

		readyToPush = true;

        AudioSource[] playerSounds = transform.Find("PlayerSounds").GetComponents<AudioSource>(); //Grab the audio sources on the graphics child
        //bgMusic = playerSounds[0];
        breathingSound = playerSounds[0];
		keyPickUpSound = playerSounds[1];
		itemPickUpSound = playerSounds[2];
		itemDropSound = playerSounds[3];

		AudioSource[] stepSounds = transform.Find("StepSoundController").GetComponents<AudioSource>(); //Grab the audio sources on the player parent
		grassStep = stepSounds[0];
		woodStep = stepSounds[1];

        if(PlayerPrefs.GetInt("LoadGame") == 1)
        {
            this.GetComponent<SaveLoad>().Load();
            this.GetComponent<ShedTutorial>().tutorial = false;
        }
        else
        {
            StartCoroutine(WaitToLoad(3.0F));
            //this.GetComponent<LoadUnload>().enabled = true;
            LoadUnload.iAmLoaded = true;
        }

		//Ensure game is not frozen
		Time.timeScale = 1.0F;
    }

    // Update is called once per frame
    void Update()
    {

        if (!this.gameObject.animation.IsPlaying("OpeningCut")) 
		{
            Sanity();
            //Walk();
            //Sprint();
            //Crouch();
            Push();
        }
        
    }

    //Note: Bug: enemies will not pathfind close enough to you to actually register the collision.
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Gnome") && other.gameObject.name != "GnomeShed")
        {
            sanity -= 0.2f;
        }
    }

    /*void Sprint()
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
    }*/

	IEnumerator PushTimer(float waitTime, GameObject target)
	{	
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);

		target.GetComponent<Gnome> ().readyToSpawn = true;
		readyToPush = true;
	}

	void Push()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			Transform cam = Camera.main.transform;

			//Player layer mask
			int playerLayer = 8;
			int invisibleLayer = 10;
			LayerMask ignoreMask = 1 << playerLayer | 1 << invisibleLayer;
			
			//Invert bitmask to only ignore this layer
			ignoreMask = ~ignoreMask;
			
			RaycastHit hit;
			
			if (Physics.Raycast(cam.position, cam.forward, out hit, 5, ignoreMask))
			{
				activeTarget = hit.collider.gameObject; //Store item being looked at

				if(activeTarget.tag == "Gnome" && Gnome.gnomeLevel == 1 && activeTarget.GetComponent<Gnome>().pushed == false && activeTarget.name != "GnomeShed")
				{
					//Gnome is pushed
					activeTarget.GetComponent<Gnome>().pushed = true;

					//Start push timer on gnome
					StartCoroutine(activeTarget.GetComponent<Gnome>().SpawnTimer(10.0F));

					//Disable NavMeshAgent
					activeTarget.GetComponent<NavMeshAgent>().enabled = false;
					
					//Make the gnome moveable
					activeTarget.rigidbody.isKinematic = false;
					activeTarget.rigidbody.useGravity = true;

					//Apply a force in the direction of the push
					activeTarget.rigidbody.AddForce((activeTarget.transform.position - this.transform.position).normalized * 6, ForceMode.Impulse);

					//Reduce player's energy by 5
					sanity -= 5.0F;
				}

				if(activeTarget.tag == "Gargoyle")
				{
					//Apply physics to the gargoyle
					activeTarget.transform.parent.rigidbody.GetComponent<Rigidbody>().isKinematic = false;
					activeTarget.transform.parent.rigidbody.GetComponent<Rigidbody>().useGravity = true;

					//Apply a force in the direction of the push
					activeTarget.transform.parent.rigidbody.AddForce((activeTarget.transform.position - this.transform.position).normalized * 4, ForceMode.Impulse);
				}
			}
		}
	}

    void Sanity()
    {
        sanity -= .001f;
        if (sanity < 0 && !this.GetComponent<EndGames>().playerSlept)
        {
            this.GetComponent<EndGames>().playerSlept = true;
            this.GetComponent<EndGames>().GetTime();
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


	void OnTriggerEnter(Collider other)
	{
        if(other.name == "AtticLadder" || other.name == "GarageLadder")
        {
            this.GetComponent<CharacterController>().slopeLimit = 90;
        }
		else if(other.name == "DarknessTrigger")
		{
			GameObject.Find ("Darkness").GetComponent<scrDarkness>().DarknessEvent();
		}
	}

    void OnTriggerExit(Collider other)
    {
        if (this.GetComponent<CharacterController>().slopeLimit == 90)
        {
            this.GetComponent<CharacterController>().slopeLimit = 45;
        }
    }

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//Exit if collision has no tag
		if(hit.collider.tag == null)
		{
			return;
		}

		floorType = hit.collider.tag;
	}

	IEnumerator WaitToReload(float waitTime)
	{
		//Wait before loading the main menu
		yield return new WaitForSeconds (waitTime);

		//Load the main menu
		Application.LoadLevel ("MainMenu");
	}

    IEnumerator WaitToLoad(float waitTime)
    {
        //Wait before loading the main menu
        yield return new WaitForSeconds(waitTime);

        //Enable loading and uloading
        this.GetComponent<LoadUnload>().enabled = true;

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

	void PlayStepSound()
	{
		switch(floorType)
		{
			case "Structure":
				//Play wood step
				woodStep.volume = 0.4F;
				woodStep.pitch = 0.9F + 0.2F * Random.value;
				woodStep.Play();
				break;
			case "Outside":
				//Play grass step
				grassStep.volume = 0.4F;
				grassStep.pitch = 0.9F + 0.2F * Random.value;
				grassStep.Play();
				break;
		}
	}

	void PlayJumpSound()
	{
		switch(floorType)
		{
			case "Structure":
				//Play wood step
				woodStep.volume = 1.0F;
				woodStep.pitch = 0.4F + 0.2F * Random.value;
				woodStep.Play();
				break;
			case "Outside":
				//Play grass step
				grassStep.volume = 1.0F;
				grassStep.pitch = 0.4F + 0.2F * Random.value;
				grassStep.Play();
				break;
		}
	}
}