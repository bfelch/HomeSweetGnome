using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Useable : MonoBehaviour 
{
    public UseableType type;
    public List<Item> requiredItems;
    public GUIWrapper playerGUI;

    public bool chandDropped = false;

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
        if (playerGUI.HasItems(requiredItems))
        {
            if (type == UseableType.DOOR)
            {
                this.gameObject.GetComponent<DoorInteraction>().DoorKeyOpen();
            }
            else if (type == UseableType.DIRT)
            {
                Destroy(this.gameObject);

                //Spawn the trap trigger
                GameObject.Find("DirtTrap").collider.enabled = true;
            }
            else if (type == UseableType.GATE)
            {
                Application.LoadLevel("MainMenu");
            }
            else if (type == UseableType.LIGHT)
            {
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

public enum UseableType { DOOR, DIRT, GATE, LIGHT, CHAND };
