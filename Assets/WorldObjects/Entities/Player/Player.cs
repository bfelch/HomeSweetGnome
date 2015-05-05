using UnityEngine;
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
	public AudioSource itemEatSound;
	public AudioSource hurtSound;
	public AudioSource yawnSound;

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
	private AudioSource carpetStep;
	private AudioSource concreteStep;
	private AudioSource dirtStep;
	private AudioSource waterSplash;
	private AudioSource waterStep1;
	private AudioSource waterStep2;
	private AudioSource waterStep3;

	private string floorType;
	private scrTunnelEvent tunnelEvent;

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
        breathingSound = playerSounds[0];
		keyPickUpSound = playerSounds[1];
		itemPickUpSound = playerSounds[2];
		itemDropSound = playerSounds[3];
		itemEatSound = playerSounds[4];
		hurtSound = playerSounds[5];
		yawnSound = playerSounds[6];

		AudioSource[] stepSounds = transform.Find("StepSoundController").GetComponents<AudioSource>(); //Grab the audio sources on the player parent
		grassStep = stepSounds[0];
		woodStep = stepSounds[1];
		carpetStep = stepSounds[2];
		concreteStep = stepSounds[3];
		dirtStep = stepSounds[4];
		waterSplash = stepSounds[5];
		waterStep1 = stepSounds[6];
		waterStep2 = stepSounds[7];
		waterStep3 = stepSounds[8];

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

		tunnelEvent = GameObject.Find("TunnelEvent").GetComponent<scrTunnelEvent>();
    }

    // Update is called once per frame
    void Update()
    {
		//Slowly drain energy while game is not paused or during opening cutscene
        if (!this.gameObject.animation.IsPlaying("OpeningCut") 
		    && !PlayerInteractions.pause) 
		{
            Sanity();
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonUp(0) && !PlayerInteractions.delayPause)
		{
			Push();
		}

    }

	IEnumerator PushTimer(float waitTime, GameObject target)
	{	
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);

		target.GetComponent<Gnome> ().readyToSpawn = true;
		readyToPush = true;
	}

	void Push()
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

				if(activeTarget.tag == "Gnome" 
				   && Gnome.gnomeLevel == 1 
				   && activeTarget.GetComponent<Gnome>().pushed == false 
				   && activeTarget.name != "GnomeShed"
				   && activeTarget.name != "DarkGnome(Clone)"
				   && activeTarget.GetComponent<Gnome>().fallen == false)
				{
					PlayerInteractions.displayWarningMsg = false;

					//Play damage sound
					hurtSound.Play();

					//Flicker damage GUI
					GetComponent<GUIDamage>().damageTimer = 0.0F;

					//Gnome is pushed
					activeTarget.GetComponent<Gnome>().pushed = true;

					//Snuff lantern
					activeTarget.GetComponentInChildren<Light>().enabled = false;
					GameObject lightBulb = activeTarget.transform.Find("LightBulb").gameObject;
					(lightBulb.GetComponent("Halo") as Behaviour).enabled = false;

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

    void Sanity()
    {
        sanity -= .002f;

        if (sanity <= 0 && !this.GetComponent<EndGames>().playerSlept)
        {
            this.GetComponent<EndGames>().playerSlept = true;
            this.GetComponent<EndGames>().GetTime();
			//StartCoroutine(WaitToReload(5.0F));
        }

        if(sanity > maxSanity)
        {
			Debug.Log("Over Max");
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

		if(other.name.Equals("StudyArm"))
		{
			if(!GameObject.Find("StudyArm").GetComponent<Animation>().isPlaying)
			{
				GameObject.Find("StudyArm").GetComponent<Animation>().Play();
			}
		}

		if(other.name.Equals("TunnelEventSpawn1"))
		{
			//Check Tunnel Gnome Event Version 2
			tunnelEvent.CheckTunnelEvent(2);
			//Prepare Tunnel Gnome Event Version 2
			tunnelEvent.PrepareTunnelEvent(1);
		}

		if(other.name.Equals("TunnelEventSpawn2"))
		{
			//Check Tunnel Gnome Event Version 1
			tunnelEvent.CheckTunnelEvent(1);
			//Prepare Tunnel Gnome Event Version 2
			tunnelEvent.PrepareTunnelEvent(2);
		}

		if(other.name.Equals("TunnelEventDespawn1"))
		{
			tunnelEvent.StartTunnelEvent(1);
		}
		
		if(other.name.Equals("TunnelEventDespawn2"))
		{
			tunnelEvent.StartTunnelEvent(2);
		}

		if(other.gameObject.tag == "Gnome" 
		   && other.gameObject.name != "GnomeShed" 
		   && other.gameObject.GetComponent<Gnome>().enabled == true  
		   && other.gameObject.GetComponent<Gnome>().pushed == false 
		   && other.gameObject.GetComponent<Gnome>().fallen == false
		   && other.gameObject.GetComponent<Gnome>().trapped == false)
		{
			other.gameObject.GetComponent<Gnome>().touchingPlayer = true;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Gnome" 
		    && other.gameObject.name != "GnomeShed" 
		    && other.gameObject.GetComponent<Gnome>().enabled == true 
		    && other.gameObject.GetComponent<Gnome>().pushed == false
		    && other.gameObject.GetComponent<Gnome>().fallen == false
		    && other.gameObject.GetComponent<Gnome>().trapped == false)
		{
			sanity -= 0.2f;
		}
	}

    void OnTriggerExit(Collider other)
    {
        if (this.GetComponent<CharacterController>().slopeLimit == 90)
        {
            this.GetComponent<CharacterController>().slopeLimit = 50;
        }

		if(other.gameObject.tag == "Gnome" 
		   && other.gameObject.name != "GnomeShed" 
		   && other.gameObject.GetComponent<Gnome>().enabled == true  
		   && other.gameObject.GetComponent<Gnome>().pushed == false 
		   && other.gameObject.GetComponent<Gnome>().fallen == false
		   && other.gameObject.GetComponent<Gnome>().trapped == false)
		{
			other.gameObject.GetComponent<Gnome>().touchingPlayer = false;
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
			case "Wood":
				//Play wood step
				woodStep.volume = 0.4F;
				woodStep.pitch = 0.9F + 0.2F * Random.value;
				woodStep.Play();
				break;
			case "Grass":
				//Play grass step
				grassStep.volume = 0.4F;
				grassStep.pitch = 0.9F + 0.2F * Random.value;
				grassStep.Play();
				break;
			case "Carpet":
				//Play carpet step
				carpetStep.volume = 0.8F;
				carpetStep.pitch = 0.9F + 0.2F * Random.value;
				carpetStep.Play();
				break;
			case "Concrete":
				//Play concrete step
				concreteStep.volume = 0.05F;
				concreteStep.pitch = 1.1F + 0.2F * Random.value;
				concreteStep.Play();
				break;
			case "Water":
				int randNumber = Random.Range(6,9);

				switch(randNumber)
				{
					case 6:
						//Play water step
						waterStep1.volume = 0.8F;
						waterStep1.pitch = 0.9F + 0.2F * Random.value;
						waterStep1.Play();
						break;
					case 7:
						//Play water step
						waterStep2.volume = 0.8F;
						waterStep2.pitch = 0.9F + 0.2F * Random.value;
						waterStep2.Play();
						break;
					case 8:
						//Play water step
						waterStep3.volume = 0.8F;
						waterStep3.pitch = 0.9F + 0.2F * Random.value;
						waterStep3.Play();
						break;
				}
				break;
			case "Dirt":
				//Play dirt step
				dirtStep.volume = 0.4F;
				dirtStep.pitch = 0.9F + 0.2F * Random.value;
				dirtStep.Play();
				break;
		}
	}

	void PlayJumpSound()
	{
		switch(floorType)
		{
			case "Wood":
				//Play wood step
				woodStep.volume = 1.0F;
				woodStep.pitch = 0.4F + 0.2F * Random.value;
				woodStep.Play();
				break;
			case "Grass":
				//Play grass step
				grassStep.volume = 1.0F;
				grassStep.pitch = 0.4F + 0.2F * Random.value;
				grassStep.Play();
				break;
			case "Carpet":
				//Play grass step
				carpetStep.volume = 1.4F;
				carpetStep.pitch = 0.4F + 0.2F * Random.value;
				carpetStep.Play();
				break;
			case "Concrete":
				//Play concrete step
				concreteStep.volume = 0.1F;
				concreteStep.pitch = 0.5F + 0.2F * Random.value;
				concreteStep.Play();
				break;
			case "Water":
				//Play water splash
				waterSplash.volume = 0.4F;
				waterSplash.pitch = 1.0F + 0.2F * Random.value;
				waterSplash.Play();
				break;
			case "Dirt":
				//Play dirt step
				dirtStep.volume = 1.0F;
				dirtStep.pitch = 0.4F + 0.2F * Random.value;
				dirtStep.Play();
				break;
		}
	}
}