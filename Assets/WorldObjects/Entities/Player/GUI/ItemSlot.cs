using UnityEngine;
using System.Collections;

public class ItemSlot : MonoBehaviour {
    //reference to held item
    public Item heldItem;
    //reference to the gui slot
    public GUISlot gui;

	// Use this for initialization
	void Start () {
        heldItem = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (gui.hovering)
        {
            //if hovering, draw name on mouse position
            Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 30);
            if (heldItem != null)
            {
                GUI.Box(box, heldItem.name);
            }
            else
            {
                GUI.Box(box, "Empty");
            }
        }
    }

    public bool AddItem(Item item)
    {
        //if no item in slot, add it
        if (heldItem == null)
        {
            heldItem = item;
            return true;
        }

        return false;
    }
}
