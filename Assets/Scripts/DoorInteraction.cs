using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{

    JointMotor hingeMotorThing;
    void Start()
    {
        this.hingeMotorThing = gameObject.GetComponent<HingeJoint>().motor;

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
        if (Input.GetKeyUp(KeyCode.O))
        {
            this.hingeMotorThing.targetVelocity = -this.hingeMotorThing.targetVelocity;
            gameObject.GetComponent<HingeJoint>().motor = this.hingeMotorThing;

        }
        else
        {
            hingeJoint.useMotor = false;
        }


    }
}
