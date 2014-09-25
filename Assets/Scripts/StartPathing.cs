using UnityEngine;
using System.Collections;

public class StartPathing : MonoBehaviour
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

    void Start()
    {
        QualitySettings.antiAliasing = 4;
        target = GameObject.Find("Player");
        agent = gameObject.GetComponent<NavMeshAgent>();
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
        agent.speed = moveSpeed;
    }

    private void Wander()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            float targetX = Random.Range(transform.position.x - (sightRange / 2), transform.position.x + (sightRange / 2));
            float targetZ = Random.Range(transform.position.z - (sightRange / 2), transform.position.z + (sightRange / 2));
            Vector3 targetPos = new Vector3(targetX, 0, targetZ);

            agent.SetDestination(targetPos);
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
        return isSeen;
    }

    private bool SeesPlayer()
    {
        return true;
    }
}
