using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Useable : MonoBehaviour {
    public UseableType type;
    public List<Item> requiredItems;
    public GUIWrapper playerGUI;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Interact()
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
        }
    }
}

public enum UseableType { DOOR, DIRT };
