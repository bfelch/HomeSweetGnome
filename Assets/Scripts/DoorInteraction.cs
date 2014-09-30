using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour {

    void Start()
    {
    }
    //Main function
    void Update()
    {
       DoorKeyOpen();
    }

    void DoorKeyOpen()
    {
        if (Input.GetKey("o"))
        {
            hingeJoint.useMotor = true;
        }
        else
        {
            hingeJoint.useMotor = false;
        }
    }
}
