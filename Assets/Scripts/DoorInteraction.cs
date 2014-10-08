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
    //Main function
    void Update()
    {
        DoorKeyOpen();
    }

    void DoorKeyOpen()
    {
        Transform cam = Camera.main.transform;       
		Ray ray = new Ray(cam.position, cam.forward);
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        if (Physics.Raycast(cam.position, cam.forward, out hit))
        {
            activeTarget = hit.collider.gameObject; //Store item being looked at

            //Am I looking at the door?
            if (hit.distance <= 5.0 && activeTarget.tag == "Door")
            {
                this.jointLimits.max = 90;
                gameObject.GetComponent<HingeJoint>().limits = this.jointLimits;

                if (Input.GetKey("e"))
                {
                    hingeJoint.useMotor = true;
                }
                if (Input.GetKeyUp(KeyCode.E))
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

    }
}
