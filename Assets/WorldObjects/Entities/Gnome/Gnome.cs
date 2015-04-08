using UnityEngine;
using System.Collections;

public class Gnome : MonoBehaviour
{
    //NavMesh controller
    public NavMeshAgent agent;
    //Target in world
    public GameObject target;

    //Range at which will follow target
    public float sightRange;
    //Movement speed
    public float moveSpeed;
    //Wander speed
    public float wanderSpeed;

    //Last known location of target
    private Vector3 lastKnownLocation;

    //Is the gnome fallen?
    public bool fallen = false;

	//Is the gnome trapped
	public bool trapped = false;

    public bool readyToSpawn = false;
	public bool pushed = false;

	public static int gnomeLevel = 1;

    //Dirt Spawner object
    private GameObject dirtSpawner1;
	private GameObject dirtSpawner2;
	private GameObject dirtSpawner3;
	private GameObject dirtSpawner4;

	public Vector3 spawnPosition;

	//Shatter Effect Prefab
	public GameObject shatterEffect;

    //Starting location of gnome
    private Vector3 startLocation;

	private string trapName;

	public bool touchingPlayer = false;

	private Animation walkAnim; //The animation component

    void Start()
    {
        QualitySettings.antiAliasing = 4;
        target = GameObject.Find("Player");
        agent = gameObject.GetComponent<NavMeshAgent>();

        dirtSpawner1 = GameObject.Find("TrapSpawner1");
		dirtSpawner2 = GameObject.Find("TrapSpawner2");
		dirtSpawner3 = GameObject.Find("TrapSpawner3");
		dirtSpawner4 = GameObject.Find("TrapSpawner4");

        startLocation = this.transform.position;

		walkAnim = GetComponent<Animation>();
    }

    void Update()
    {
		//Climb or Stand when the player is not looking
        if(readyToSpawn && !SeenByPlayer())
        {
			if(fallen && gnomeLevel == 2)
			{
            	Climb();
			}
			else if(pushed)
			{
				Stand();
			}
        }
        else
        {
            //if not on screen, fallen, or pushed
            if (!SeenByPlayer() && !fallen && !pushed && !trapped && !touchingPlayer) 
			{
                if (TargetInRange()) 
				{
					//if player in range
					FollowPlayer();
                } 
				else 
				{
                    //if player not in range
                    GoHome();
                }
            }
            else
            {
				if(this.gameObject.name == "GnomeLvl2")
				{
					walkAnim.animation["GnomeWalk"].speed = 0.0F; //Play animation fowards
				}

                //prevent movement
                agent.speed = 0;
            }
        }
    }

    private void FollowPlayer()
    {
		if(this.gameObject.name == "GnomeLvl2")
		{
			walkAnim.animation["GnomeWalk"].speed = 1.0F; //Play animation fowards

			if(!walkAnim.animation.IsPlaying("GnomeWalk"))
			{
				walkAnim.Play();
			}
		}

        //set last know location of player
        lastKnownLocation = target.transform.position;
        //set destination
        agent.SetDestination(lastKnownLocation);
        //move at normal speed
        agent.speed = moveSpeed;
    }

    private void GoHome()
    {
		if(this.gameObject.name == "GnomeLvl2")
		{
			//Debug.Log("animate2");
			walkAnim.animation["GnomeWalk"].speed = 1.0F; //Play animation fowards
		}

        if (Vector3.Distance(transform.position, startLocation) > sightRange * 1.5)
        {
            //start moving back to start location
            agent.SetDestination(startLocation);
            //move at normal speed
            agent.speed = moveSpeed;
        }
    }

    private bool TargetInRange() 
    {
        //checks distance between player and gnome
        return Vector3.Distance(transform.position, target.transform.position) < sightRange;
    }

    public bool SeenByPlayer()
    {
        //sets viewport coordinates
        bool isSeen;
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        float viewportX = viewportPos.x;
        float viewportY = viewportPos.y;
        float viewportZ = viewportPos.z;

        //adjusts for tolerance
        float tolerance = 0.2f;
        float lowBound = -tolerance;
        float highBound = 1.0f + tolerance;
        float zBound = -tolerance;

        //checks for gnomes in view
        isSeen = (viewportX >= lowBound && viewportX <= highBound);
        isSeen = isSeen && (viewportY >= lowBound && viewportY <= highBound);
        isSeen = isSeen && viewportZ > zBound;
        if(this.name != "GnomeShed")
		{
			isSeen = isSeen && !GameObject.Find("Player").GetComponent<Blink>().blink;
		}
        return isSeen;
    }

    private bool SeesPlayer()
    {
        return true;
    }

    public IEnumerator SpawnTimer(float waitTime)
    {
        //Wait spawn time
        yield return new WaitForSeconds(waitTime);

        readyToSpawn = true;
    }

    void Climb()
    {
        //Gnome is no longer fallen
        fallen = false;

		/*
		transform.position = spawnPosition;

        //Enable NavMeshAgent
        GetComponent<NavMeshAgent>().enabled = true;

        //Make the gnome kinematic again
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;*/

        readyToSpawn = false;
    }

	void Stand()
	{	
		//Gnome is no longer pushed
		pushed = false;
		
		//Set position
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		//Set rotation
		transform.rotation = new Quaternion(0, 0, 0, 0);

		//Light latnern
		GetComponentInChildren<Light>().enabled = true;
		GameObject lightBulb = transform.Find("LightBulb").gameObject;
		(lightBulb.GetComponent("Halo") as Behaviour).enabled = true;

		//Enable NavMeshAgent
		GetComponent<NavMeshAgent>().enabled = true;
		
		//Make the gnome kinematic again
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
		
		readyToSpawn = false;
	}

	void BreakApart()
	{
		//Emit particle effect at shatter location
		Instantiate(shatterEffect, new Vector3(transform.position.x, transform.position.y - 0.8F, transform.position.z), shatterEffect.transform.rotation);

		//Crush the gnome
		Destroy(this.gameObject);
	}

    void OnTriggerEnter(Collider other)
	{
		if(other.name == "DirtTrap")
		{
			trapName = other.transform.parent.name;

			//Save spawn position
			if(trapName == "DirtTrap1")
			{
				spawnPosition = dirtSpawner1.transform.position;
			}
			else if(trapName == "DirtTrap2")
			{
				spawnPosition = dirtSpawner2.transform.position;
			}
			else if(trapName == "DirtTrap3")
			{
				spawnPosition = dirtSpawner3.transform.position;
			}
			else if(trapName == "DirtTrap4")
			{
				spawnPosition = dirtSpawner4.transform.position;
			}

            //Gnome is fallen
            fallen = true;

			//Disable NavMeshAgent
            GetComponent<NavMeshAgent>().enabled = false;

            //Make the gnome fall
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;

            //Set spawn timer
            StartCoroutine(SpawnTimer(5.0F));
		}
		else if(other.name == "CircleTrap")
		{	
			//Gnome is trapped
			trapped = true;

			//Disable NavMeshAgent
			GetComponent<Gnome>().enabled = false;
			GetComponent<NavMeshAgent>().enabled = false;

			//Make the gnome fall
			//rigidbody.isKinematic = false;
			//rigidbody.useGravity = true;
		}
        else if(other.tag == "DropTrap")
        {
			//Break the gnome
			BreakApart();
        }
	}

	void OnTriggerStay(Collider other)
	{
		if(other.name == "FallCollider")
		{
			//Disable NavMeshAgent
			GetComponent<NavMeshAgent>().enabled = false;
			GetComponent<Gnome>().enabled = false;
			
			//Make the gnome fall
			rigidbody.isKinematic = false;
			rigidbody.useGravity = true;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if(this.gameObject.name == "GnomeShed")
		{
			if(col.gameObject.tag == "Wood")
			{
				audio.volume = 0.6F;
				audio.pitch = 0.4F + 0.2F * Random.value;
				audio.Play();
			}
		}
	}
}
