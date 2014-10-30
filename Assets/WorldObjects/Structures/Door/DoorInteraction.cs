using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{

    JointMotor hingeMotorThing;
    JointLimits jointLimits;
    private GameObject activeTarget;
    void Start()
    {
        this.hingeMotorThing = gameObject.GetComponent<HingeJoint>().motor;
        this.jointLimits = gameObject.GetComponent<HingeJoint>().limits;
    }

    public void DoorKeyOpen()
    {
        //this.jointLimits.max = 90;
        gameObject.GetComponent<HingeJoint>().limits = this.jointLimits;
        hingeJoint.useMotor = true;
        this.hingeMotorThing.targetVelocity = -this.hingeMotorThing.targetVelocity;
        gameObject.GetComponent<HingeJoint>().motor = this.hingeMotorThing;
    }
}
