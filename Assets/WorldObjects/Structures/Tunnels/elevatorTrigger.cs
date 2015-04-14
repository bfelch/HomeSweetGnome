using UnityEngine;
using System.Collections;

public class elevatorTrigger : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && elevatorStuff.activate && this.name != "ExitTrigger" && !elevatorStuff.inElevator)
        {
            //other.gameObject.transform.parent = GameObject.Find("ElevatorStructure").transform;

            //if (other.gameObject.transform.parent == GameObject.Find("Elevator").transform)
            {
                elevatorStuff.inElevator = true;
                elevatorStuff.closeBottomElevator = true;
                elevatorStuff.closeTopElevator = true;
                elevatorStuff.thePlayerIsInElevator = true;
            }
        }

        if (other.tag == "Player" && this.name == "ExitTrigger")
        {
            elevatorStuff.closeBottomElevator = true;
            elevatorStuff.closeTopElevator = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //other.gameObject.transform.parent = GameObject.Find("Entities").transform;
        }
    }


}
