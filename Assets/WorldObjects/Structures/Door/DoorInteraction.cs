using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    JointMotor hingeMotorThing;
    JointLimits jointLimits;
    //the player object
    private GameObject activeTarget;
    void Start()
    {
        //get the hinge joint motor and the hinge limites
        this.hingeMotorThing = gameObject.GetComponent<HingeJoint>().motor;
        this.jointLimits = gameObject.GetComponent<HingeJoint>().limits;
    }

    public void DoorKeyOpen()
    {
        gameObject.GetComponent<HingeJoint>().limits = this.jointLimits;
        //set the motor to true so the door opens
        hingeJoint.useMotor = true;
        //switch the direction the velocity will be so the door does the opposite of what it just did
        this.hingeMotorThing.targetVelocity = -this.hingeMotorThing.targetVelocity;
        //set the hingeMotorThing to the actual HingJoint.motor
        gameObject.GetComponent<HingeJoint>().motor = this.hingeMotorThing;
    }
}
