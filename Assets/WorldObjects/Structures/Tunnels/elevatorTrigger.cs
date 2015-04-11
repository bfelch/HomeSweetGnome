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
        if (other.tag == "Player")
        {
            elevatorStuff.closeBottomElevator = true;
            elevatorStuff.closeTopElevator = true;
        }
    }


}
