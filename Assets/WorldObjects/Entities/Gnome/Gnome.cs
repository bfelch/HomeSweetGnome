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


    void Start()
    {
        QualitySettings.antiAliasing = 4;
        target = GameObject.Find("Player");
        targetBlink = target.GetComponent<Blink>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        blinkSpeed = float.MaxValue;
    }

    void Update()
    {
        if (!SeenByPlayer())
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

    private bool TargetInRange() {
        return Vector3.Distance(transform.position, target.transform.position) < sightRange;
    }

    private bool SeenByPlayer()
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

    
}
