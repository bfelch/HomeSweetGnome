using UnityEngine;
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

    //public AudioClip breathingSound;

    private bool crouching;
	private bool readyToPush;
    private float yScale;

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
        maxRestTime = 4f;
        sprintTime = maxSprintTime = 5f;
        playerSlept = false;
		playerFell = false;
		readyToPush = true;

        yScale = this.transform.localScale.y;

        deathTextSleep = GameObject.Find("DeathTextSleep").guiText;
        deathTextSleep.enabled = false;

		deathTextFall = GameObject.Find("DeathTextFall").guiText;
		deathTextFall.enabled = false;

        if(PlayerPrefs.GetInt("LoadGame") == 1)
        {
            GameObject.Find("Save").GetComponent<SaveLoad>().Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Quit the game
        if (Input.GetKey("escape"))
        {
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
                sprintTime += Time.deltaTime;
            }
        }
        else
        {
            //breathingSound.
            charMotor.movement.maxForwardSpeed = 6;
            restTime += Time.deltaTime;
            this.gameObject.animation.Stop("Sprint");
            if (restTime >= maxRestTime)
            {
                sprintTime = 5.0F;
                restTime = 0;
            }
        }
    }

    void Crouch()
    {
        if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)) && !crouching){
            crouching = true;
            charMotor.movement.maxForwardSpeed = 3;
            gameObject.transform.localScale = new Vector3(1, yScale / 2, 1);
            Camera.main.transform.localScale = new Vector3(1, 2 * yScale, 1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftCommand))
        {
            crouching = false;
            gameObject.transform.localScale = new Vector3(1, yScale, 1);
            Vector3 pos = gameObject.transform.position;
            gameObject.transform.position = new Vector3(pos.x, pos.y + .6f, pos.z);
            Camera.main.transform.localScale = new Vector3(1, yScale, 1);
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
        }

		if(playerFell)
		{
			deathTextFall.enabled = true;
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