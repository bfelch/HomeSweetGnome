using UnityEngine;
using System.Collections;

public class Gnome : MonoBehaviour
{
    //NavMesh controller
    public NavMeshAgent agent;
    //Target in world
    public GameObject target;
    public Blink targetBlink;

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

	//Gnome Eye prefab
	public GameObject gnomeEye;

    //Starting location of gnome
    private Vector3 startLocation;

	private string trapName;

	Animation walkAnim; //The animation component

    void Start()
    {
        QualitySettings.antiAliasing = 4;
        target = GameObject.Find("Player");
        targetBlink = target.GetComponent<Blink>();
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
            if (!SeenByPlayer() && !fallen && !pushed && !trapped) 
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
            isSeen = isSeen && !targetBlink.blink;
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

		//Random position inside a circle of size 2
		Vector2 newPosition = Random.insideUnitCircle * 2;
		
		//Make sure the eye doesn't spawn under the chand
		if(newPosition.x > 0)
		{
			newPosition.x = newPosition.x + 2;
		}
		else
		{
			newPosition.x = newPosition.x - 2;
		}
		
		if(newPosition.y > 0)
		{
			newPosition.y = newPosition.y + 2;
		}
		else
		{
			newPosition.y = newPosition.y - 2;
		}
		
		//Spawn the gnome eye
		GameObject gnomeEyeClone = (GameObject)Instantiate(gnomeEye, new Vector3(transform.position.x + newPosition.x, transform.position.y + 0.2F, transform.position.z + newPosition.y), gnomeEye.transform.rotation);

		//Change the name
		gnomeEyeClone.name = "GnomeEye";

		//Move head to item list
		gnomeEyeClone.transform.parent = GameObject.Find ("Items").transform;

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
		else if(other.name == "FallCollider")
		{
			//Disable NavMeshAgent
			GetComponent<NavMeshAgent>().enabled = false;
			GetComponent<Gnome>().enabled = false;
			
			//Make the gnome fall
			rigidbody.isKinematic = false;
			rigidbody.useGravity = true;
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
}
