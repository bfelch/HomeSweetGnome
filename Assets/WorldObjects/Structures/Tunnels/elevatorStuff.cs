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
    private static GameObject wallGaurd;

    public static bool doorIsClosed = true;

	private static AudioSource bell1;
	private static AudioSource bell2;
	public static bool bell1Played;
	public static bool bell2Played;

    public static bool thePlayerIsInElevator = false;

	// Use this for initialization
	void Start () 
    {
        topElevatorDoor = GameObject.Find("TopElevatorDoor");
        bottomElevatorDoor = GameObject.Find("BottomElevatorDoor");
        leverTop = GameObject.Find("LeverTop");
        leverBottom = GameObject.Find("LeverBottom");
        wallGaurd = GameObject.Find("WallGuard");
        wallGaurd.SetActive(false);

		bell1 = leverTop.GetComponent<AudioSource>();
		bell2 = leverBottom.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (inElevator && doorIsClosed)
        {
            if(thePlayerIsInElevator)
                GameObject.Find("Player").gameObject.transform.parent = GameObject.Find("ElevatorStructure").transform;
			
            if(!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Play();
			}

            if (direction)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, -19.97f, 0f), Time.deltaTime * 1.7f);
                //transform.Translate(Vector3.down * 3 * Time.deltaTime);

                if (transform.localPosition.y <= -19.96F)
                {
                    elevatorStuff.openBottomElevator = true;
					GetComponent<AudioSource>().Stop();
                    thePlayerIsInElevator = false;
                   // Debug.Log("Out of Elevator?");

                }
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0f, 0f, 0f), Time.deltaTime * 1.7f);
                //transform.Translate(Vector3.up * 3 * Time.deltaTime);
                if (transform.localPosition.y >= -.01)
                {
                    elevatorStuff.openTopElevator = true;
					GetComponent<AudioSource>().Stop();
                    thePlayerIsInElevator = false;
                    //Debug.Log("Out of Elevator?");
                }
            }

            //wallGaurd.SetActive(true);
        }
        else { 
            wallGaurd.SetActive(false);
            if (!thePlayerIsInElevator)
                GameObject.Find("Player").gameObject.transform.parent = GameObject.Find("Entities").transform;
        }
        
	}

    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && elevatorStuff.activate)
        {
            //other.gameObject.transform.parent = GameObject.Find("ElevatorStructure").transform;

            //if (other.gameObject.transform.parent == GameObject.Find("Elevator").transform)
            {
                Debug.Log("In Elevator?");

                elevatorStuff.inElevator = true;
                elevatorStuff.closeBottomElevator = true;
                elevatorStuff.closeTopElevator = true;
                thePlayerIsInElevator = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //thePlayerIsInElevator = false;
            
        }
    }
     * */

    public static void ElevatorDoors()
    {
        if (openTopElevator)
        {
            wallGaurd.SetActive(true);
			//Play bell sound
			if(!bell1Played)
			{
				bell1.Play();
				bell1Played = true;
			}

            topElevatorDoor.transform.localPosition = Vector3.MoveTowards(topElevatorDoor.transform.localPosition, new Vector3(.17f, .13f, -.212f), Time.deltaTime * .02f);
            leverTop.transform.localEulerAngles = Vector3.MoveTowards(leverTop.transform.localEulerAngles, new Vector3(0, 19.531f, 40f), Time.deltaTime * 2f);
            if (topElevatorDoor.transform.localPosition.x >= .169f && !elevatorStuff.inElevator)
            {
                openTopElevator = false;
                elevatorStuff.activate = true;
                elevatorStuff.direction = true;
                wallGaurd.SetActive(false);

            }
            else if (topElevatorDoor.transform.localPosition.x >= .169f)
            {
                openTopElevator = false;
                elevatorStuff.activate = false;
                elevatorStuff.inElevator = false;
                wallGaurd.SetActive(false);
            }
            doorIsClosed = false;

           // Debug.Log(elevatorStuff.doorIsClosed);
        }
        else if (closeTopElevator)
        {
            wallGaurd.SetActive(true);

            //Debug.Log("close top elevator");
            topElevatorDoor.transform.localPosition = Vector3.MoveTowards(topElevatorDoor.transform.localPosition, new Vector3(-1.595f, .01327f, .4454f), Time.deltaTime * .02f);
            leverTop.transform.localEulerAngles = Vector3.MoveTowards(leverTop.transform.localEulerAngles, new Vector3(0, 19.531f, 0f), Time.deltaTime * 2f);
            if (topElevatorDoor.transform.localPosition.x <= -1.594f && !elevatorStuff.inElevator)
            {
                closeTopElevator = false;
                elevatorStuff.activate = true;
                elevatorStuff.direction = true;
                if (!closeBottomElevator)
                    doorIsClosed = true;

            }
            else if (topElevatorDoor.transform.localPosition.x <= -1.594f)
            {
                closeTopElevator = false;
                if (!closeBottomElevator)
				{
                    doorIsClosed = true;
				}
            }

        }
        if (openBottomElevator)
        {
            wallGaurd.SetActive(true);

			//Play bell sound
			if(!bell2Played)
			{
				bell2.Play();
				bell2Played = true;
			}

            bottomElevatorDoor.transform.localPosition = Vector3.MoveTowards(bottomElevatorDoor.transform.localPosition, new Vector3(64.21f, 23.39f, -35.76f), Time.deltaTime * .02f);
            leverBottom.transform.localEulerAngles = Vector3.MoveTowards(leverBottom.transform.localEulerAngles, new Vector3(0, 19.531f, 40f), Time.deltaTime * 2f);
            if (elevatorStuff.callingDown && bottomElevatorDoor.transform.localPosition.x >= 64.19f)
            {
                openBottomElevator = false;
                elevatorStuff.callingDown = false;
                elevatorStuff.direction = false;
                elevatorStuff.inElevator = false;
                doorIsClosed = false;
                wallGaurd.SetActive(false);

            }
            else if (bottomElevatorDoor.transform.localPosition.x >= 64.19f && !elevatorStuff.inElevator)
            {
                openBottomElevator = false;
                elevatorStuff.activate = true;
                elevatorStuff.direction = false;
                doorIsClosed = false;
                wallGaurd.SetActive(false);

            }
            else if (bottomElevatorDoor.transform.localPosition.x >= 64.19f)
            {
                openBottomElevator = false;
                elevatorStuff.activate = false;
                elevatorStuff.inElevator = false;
                doorIsClosed = false;

            }

        }
        else if (closeBottomElevator)
        {
            wallGaurd.SetActive(true);

            bottomElevatorDoor.transform.localPosition = Vector3.MoveTowards(bottomElevatorDoor.transform.localPosition, new Vector3(62.591f, 23.391f, -35.17f), Time.deltaTime * .02f);
            leverBottom.transform.localEulerAngles = Vector3.MoveTowards(leverBottom.transform.localEulerAngles, new Vector3(0, 19.531f, 0f), Time.deltaTime * 2f);

            if (bottomElevatorDoor.transform.localPosition.x <= 62.61f && !elevatorStuff.inElevator)
            {
                closeBottomElevator = false;
                elevatorStuff.activate = true;
                elevatorStuff.direction = false;
                if(!closeTopElevator)
				{
                    doorIsClosed = true;
				}
            }
            else if (bottomElevatorDoor.transform.localPosition.x <= 62.61f)
            {
                closeBottomElevator = false;
                if (!closeTopElevator)
				{
                    doorIsClosed = true;
				}
            }

        }
    }


}
