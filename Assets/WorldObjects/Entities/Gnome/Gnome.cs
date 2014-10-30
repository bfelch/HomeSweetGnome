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
    private bool readyToSpawn = false;

    //Dirt Spawner object
    public GameObject dirtSpawner;

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
        if(readyToSpawn)
        {
            Respawn();
        }
        else
        {
            if (!SeenByPlayer() && !trapped)
            {
                if (TargetInRange())
                {
                    //Do pathing
                    FollowPlayer();
                }
                else
                {
                    Wander();
                }
            }
            else
            {
                agent.speed = 0;
            }
        }

    }

    private void FollowPlayer()
    {
        lastKnownLocation = target.transform.position;
        agent.SetDestination(lastKnownLocation);
        if (targetBlink.blink)
            agent.speed = blinkSpeed;
        else
            agent.speed = moveSpeed;
    }

    private void Wander()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
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
        return Vector3.Distance(transform.position, target.transform.position) < sightRange;
    }

    public bool SeenByPlayer()
    {
        bool isSeen;
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        float viewportX = viewportPos.x;
        float viewportY = viewportPos.y;
        float viewportZ = viewportPos.z;

        float tolerance = 0.2f;
        float lowBound = -tolerance;
        float highBound = 1.0f + tolerance;
        float zBound = -tolerance;

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

    void Respawn()
    {
        Debug.Log("Spawned");

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
            //Crush the gnome
            Destroy(this.gameObject);

            //Emit particle effect here
        }
	}
}
