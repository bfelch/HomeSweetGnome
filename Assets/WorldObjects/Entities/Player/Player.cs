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

    public AudioSource audio1;
    public AudioSource audio2;
    public bool breathe = true; //To check whether to play the heavyBreathing sound

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

    // Use this for initialization
    void Start()
    {
        //Sanity instead of health. As they touch you, sanity falls. It also slowly falls over time. Must find items to raise it.
        //The lower it gets, the more hazards are in the level.
        sanity = 100;
        maxSanity = 100;
        minSanity = 0;

		readyToPush = true;

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

    /*void Walk() {
        if (!this.gameObject.animation.IsPlaying("Landing")) 
		{
            if (!Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))) 
			{
                //play walk animation
                this.gameObject.animation.Play("Walk");
            }
        }

        if (controller.isGrounded) 
		{
            if (!landed) 
			{
                landed = true;
                this.gameObject.animation.Play("Landing");
            }
        } 
		else 
		{
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
    }*/

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

				if(activeTarget.tag == "Gnome" && activeTarget.GetComponent<Gnome>().gnomeLevel == 1)
				{
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

				if(activeTarget.tag == "Gargoyle")
				{
					//Apply a force in the direction of the push
					activeTarget.transform.parent.rigidbody.AddForce((activeTarget.transform.position - this.transform.position).normalized * 8, ForceMode.Impulse);
				}
			}
		}
	}

    void Sanity()
    {
        sanity -= .002f;
        if (sanity < 0)
        {
            this.GetComponent<EndGames>().playerSlept = true;

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