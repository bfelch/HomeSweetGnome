using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Useable : MonoBehaviour 
{
    //door, dirt, gate, light, chadelier, elevator...
    public UseableType type;
    //list of items required to use this item
    public List<Item> requiredItems;
    //reference to player gui
    public GUIWrapper playerGUI;

    public bool chandDropped = false;
    public bool activate = false;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public string Interact()
    {
        //checks that player has all required items
        if (playerGUI.HasItems(requiredItems))
        {
            if (type == UseableType.DOOR)
            {
                //opens door
                this.gameObject.GetComponent<DoorInteraction>().DoorKeyOpen();
            }
            else if (type == UseableType.DIRT)
            {
				//Dig the dirt
				this.gameObject.GetComponent<dirtStuff>().Dig();
            }
            else if (type == UseableType.GATE)
            {
                //Run the escape function inside the Player script
                GameObject.Find("Player").GetComponent<Player>().Escape();
            }
            else if(type == UseableType.ATTICBOWL)
            {
                //Run the escape function inside the Player script
                GameObject.Find("Player").GetComponent<Player>().Experiment();
            }
            else if (type == UseableType.LIGHT)
            {
                //toggles light
                Debug.Log("Light");
                gameObject.GetComponent<Light>().enabled = !gameObject.GetComponent<Light>().enabled;
            }
            else if (type == UseableType.CHAND)
            {
                //One time trap
                chandDropped = true;

                //Drop the chandelier
                GameObject.Find("Chandelier").GetComponent<Rigidbody>().isKinematic = false;
                GameObject.Find("Chandelier").GetComponent<Rigidbody>().useGravity = true;

                //Enable trigger only when falling
                GameObject.Find("Chandelier").GetComponentInChildren<chandStuff>().dropping = true;
            }
            else if (type == UseableType.ELEVATOR)
            {
                GameObject.Find("Elevator").GetComponent<elevatorStuff>().Activate();
            }
            return "";
        }

        string items = "";
        for (int i = 0; i < requiredItems.Count; i++)
        {
            if(i == requiredItems.Count -1)
                items += requiredItems.ToArray()[i] + " ";
            else
                items += requiredItems.ToArray()[i] + " and ";

        }
        return items;
    }
}

public enum UseableType { DOOR, DIRT, GATE, LIGHT, CHAND, ELEVATOR, ATTICBOWL };
