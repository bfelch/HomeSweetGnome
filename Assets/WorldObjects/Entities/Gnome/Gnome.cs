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
    private float blinkSpeed;
    //Wander speed
    public float wanderSpeed;

    //Last known location of target
    private Vector3 lastKnownLocation;

    //Is the gnome trapped?
    public bool trapped = false;
    public bool readyToSpawn = false;
	public bool pushed = false;

	public int gnomeLevel = 1;

    //Dirt Spawner object
    public GameObject dirtSpawner;

	//Shatter Effect Prefab
	public GameObject shatterEffect;

    void Start()
    {
        QualitySettings.antiAliasing = 4;
        target = GameObject.Find("Player");
        targetBlink = target.GetComponent<Blink>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        blinkSpeed = float.MaxValue;
        dirtSpawner = GameObject.Find("DirtSpawner");
    }

    void Update()
    {
		//Climb or Stand when the player is not looking
        if(readyToSpawn && !SeenByPlayer())
        {
			if(trapped && gnomeLevel == 2)
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
            //if not on screen, trapped, or pushed
            if (!SeenByPlayer() && !trapped && !pushed) {
                if (TargetInRange()) {
                    //if player in range
                    FollowPlayer();
                } else {
                    //if player not in range
                    Wander();
                }
            }
            else
            {
                //prevent movement
                agent.speed = 0;
            }
        }

    }

    private void FollowPlayer()
    {
        //set last know location of player
        lastKnownLocation = target.transform.position;
        //set destination
        agent.SetDestination(lastKnownLocation);
        if (targetBlink.blink) {
            //if player is blinking move super fast
            agent.speed = blinkSpeed;
        } else {
            //move at normal speed
            agent.speed = moveSpeed;
        }
    }

    private void Wander()
    {
        //if done with current path
        if (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            //find new destination
            float targetX = Random.Range(transform.position.x - (sightRange / 2), transform.position.x + (sightRange / 2));
            float targetZ = Random.Range(transform.position.z - (sightRange / 2), transform.position.z + (sightRange / 2));
            Vector3 targetPos = new Vector3(targetX, 0, targetZ);

            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position + targetPos, out hit, sightRange, 1);

            agent.SetDestination(hit.position);
            agent.speed = wanderSpeed;
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
        isSeen = isSeen && !targetBlink.blink;
        return isSeen;
    }

    private bool SeesPlayer()
    {
        return true;
    }

    IEnumerator SpawnTimer(float waitTime)
    {
        Debug.Log("Spawner Timer Started");

        //Wait spawn time
        yield return new WaitForSeconds(waitTime);

        readyToSpawn = true;
    }

    void Climb()
    {
        Debug.Log("Climbed");

        //Gnome is no longer trapped
        trapped = false;

        //Set position
        transform.position = dirtSpawner.transform.position;

        //Enable NavMeshAgent
        GetComponent<NavMeshAgent>().enabled = true;

        //Make the gnome kinematic again
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;

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

    void OnTriggerEnter(Collider other)
	{
		if(other.name == "DirtTrap")
		{
            Debug.Log("Trapped");

            //Gnome is trapped
            trapped = true;

			//Disable NavMeshAgent
            GetComponent<NavMeshAgent>().enabled = false;

            //Make the gnome fall
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;

            //Set spawn timer
            StartCoroutine(SpawnTimer(5.0F));
		}
        else if(other.tag == "DropTrap")
        {
			//Emit particle effect at shatter location
			Instantiate(shatterEffect, new Vector3(transform.position.x, transform.position.y - 0.8F, transform.position.z), shatterEffect.transform.rotation);

            //Crush the gnome
            Destroy(this.gameObject);
        }
	}
}
