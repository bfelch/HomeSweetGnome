using UnityEngine;
using System.Collections;

public class elevatorStuff : MonoBehaviour 
{
    public static bool direction = false;
    public static bool inElevator = false;
    public static bool activate = false;

    public static bool callingDown = false;

    public static bool openTopElevator = false;
    public static bool closeTopElevator = false;
    public static bool closeBottomElevator = false;
    public static bool openBottomElevator = false;

    private static GameObject topElevatorDoor;
    private static GameObject bottomElevatorDoor;
    private static GameObject leverBottom;
    private static GameObject leverTop;
	// Use this for initialization
	void Start () 
    {
        topElevatorDoor = GameObject.Find("TopElevatorDoor");
        bottomElevatorDoor = GameObject.Find("BottomElevatorDoor");
        leverTop = GameObject.Find("LeverTop");
        leverBottom = GameObject.Find("LeverBottom");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (inElevator)
        {
            if (direction)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(62.59f, 25.12f, -38.09f), Time.deltaTime*1.7f);
                //transform.Translate(Vector3.down * 3 * Time.deltaTime);

                if (transform.localPosition.y <= 25.2F)
                {
                    elevatorStuff.openBottomElevator = true;
                }
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(62.588f, 44.88f, -37.79f), Time.deltaTime * 1.7f);
                //transform.Translate(Vector3.up * 3 * Time.deltaTime);
                if (transform.localPosition.y >= 44.8f)
                {
                    elevatorStuff.openTopElevator = true;
                }
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && elevatorStuff.activate)
        {
            //other.gameObject.transform.parent = GameObject.Find("Elevator").transform;

            //if (other.gameObject.transform.parent == GameObject.Find("Elevator").transform)
            {
                elevatorStuff.inElevator = true;
                elevatorStuff.closeBottomElevator = true;
                elevatorStuff.closeTopElevator = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //other.gameObject.transform.parent = GameObject.Find("Entities").transform;

        }
    }

    public static void ElevatorDoors()
    {
        if (openTopElevator)
        {
            topElevatorDoor.transform.localPosition = Vector3.MoveTowards(topElevatorDoor.transform.localPosition, new Vector3(.17f, .13f, -.212f), Time.deltaTime * .02f);
            leverTop.transform.localEulerAngles = Vector3.MoveTowards(leverTop.transform.localEulerAngles, new Vector3(0, 19.531f, 40f), Time.deltaTime * 2f);
            if (topElevatorDoor.transform.localPosition.x >= .169f && !elevatorStuff.inElevator)
            {
                openTopElevator = false;
                elevatorStuff.activate = true;
                elevatorStuff.direction = true;
            }
            else if (topElevatorDoor.transform.localPosition.x >= .169f)
            {
                openTopElevator = false;
                elevatorStuff.activate = false;
                elevatorStuff.inElevator = false;
            }
        }
        if (closeTopElevator)
        {
            //Debug.Log("close top elevator");
            topElevatorDoor.transform.localPosition = Vector3.MoveTowards(topElevatorDoor.transform.localPosition, new Vector3(-1.595f, .01327f, .4454f), Time.deltaTime * .02f);
            leverTop.transform.localEulerAngles = Vector3.MoveTowards(leverTop.transform.localEulerAngles, new Vector3(0, 19.531f, 0f), Time.deltaTime * 2f);
            if (topElevatorDoor.transform.localPosition.x <= -1.594f && !elevatorStuff.inElevator)
            {
                closeTopElevator = false;
                elevatorStuff.activate = true;
                elevatorStuff.direction = true;
            }
            else if (topElevatorDoor.transform.localPosition.x <= -1.594f)
            {
                closeTopElevator = false;
            }

        }
        if (openBottomElevator)
        {
            bottomElevatorDoor.transform.localPosition = Vector3.MoveTowards(bottomElevatorDoor.transform.localPosition, new Vector3(64.21f, 23.39f, -35.76f), Time.deltaTime * .02f);
            leverBottom.transform.localEulerAngles = Vector3.MoveTowards(leverBottom.transform.localEulerAngles, new Vector3(0, 19.531f, 40f), Time.deltaTime * 2f);
            if (elevatorStuff.callingDown && bottomElevatorDoor.transform.localPosition.x >= 64.19f)
            {
                openBottomElevator = false;
                elevatorStuff.callingDown = false;
                elevatorStuff.direction = false;
                elevatorStuff.inElevator = false;
            }
            else if (bottomElevatorDoor.transform.localPosition.x >= 64.19f && !elevatorStuff.inElevator)
            {
                openBottomElevator = false;
                elevatorStuff.activate = true;
                elevatorStuff.direction = false;
            }
            else if (bottomElevatorDoor.transform.localPosition.x >= 64.19f)
            {
                openBottomElevator = false;
                elevatorStuff.activate = false;
                elevatorStuff.inElevator = false;
            }

        }
        if (closeBottomElevator)
        {
            bottomElevatorDoor.transform.localPosition = Vector3.MoveTowards(bottomElevatorDoor.transform.localPosition, new Vector3(62.591f, 23.391f, -35.17f), Time.deltaTime * .02f);
            leverBottom.transform.localEulerAngles = Vector3.MoveTowards(leverBottom.transform.localEulerAngles, new Vector3(0, 19.531f, 0f), Time.deltaTime * 2f);

            if (bottomElevatorDoor.transform.localPosition.x <= 62.61f && !elevatorStuff.inElevator)
            {
                closeBottomElevator = false;
                elevatorStuff.activate = true;
                elevatorStuff.direction = false;
            }
            else if (bottomElevatorDoor.transform.localPosition.x <= 62.61f)
            {
                closeBottomElevator = false;
            }

        }
    }


}
